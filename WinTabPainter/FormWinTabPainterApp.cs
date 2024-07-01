using System;
using SD=System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;
using WinTabPainter.Painting;
using WinTabPainter.Geometry;

// References:
// https://github.com/DennisWacom/WintabControl/tree/master/WintabControl
// https://github.com/DennisWacom/InkPlatform/tree/master/WintabDN
// https://developer-docs.wacom.com/docs/icbt/windows/wintab/wintab-basics/
// https://www.nuget.org/packages/WacomSolutionPartner.WintabDotNet
// https://github.com/Wacom-Developer/wacom-device-kit-windows/tree/master/Wintab%20TiltTest

namespace WinTabPainter
{
    public partial class FormWinTabPainterApp : Form
    {

        private WintabDN.CWintabContext wintab_context = null;
        private WintabDN.CWintabData wintab_data = null;

        private Painting.BitmapDocument bitmap_doc;

        public string filename = null;
        TabletInfo tablet_info = new TabletInfo();

        public Painting.PaintSettings paintsettings = new Painting.PaintSettings();

        string FileSaveDefaultFilename = "Untitled.png";
        string FileSaveDefaultExt = "png";
        string FileOpenDefaultExt = "png";
        string DefaultTabletDeviceName = "UKNOWN_DEVICE";

        ColorARGB clr_black = new Painting.ColorARGB(255, 0, 0, 0);
        Numerics.Range SMOOTHING_TRACKBAR_RANGE = new Numerics.Range(-100, 100);

        Painting.PaintData old_paintdata;
        public FormWinTabPainterApp()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.old_paintdata = new Painting.PaintData();

            // All actual drawing will be done to this bitmap
            this.bitmap_doc = new Painting.BitmapDocument(1500, 1500);

            // Create a graphics object for the canvas bitmap and enable smoothing
            // by default for better looking stroke edges


            // Double-buffering to reduce flicket
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            // Link the bitmap canvas to the picturebox
            // changes to the bitmap will be rendered
            // whenever the picturebox is redrawn

            this.pictureBox_Canvas.Image = this.bitmap_doc.background_layer.Bitmap;
            this.EraseCanvas();

            this.wintab_context = OpenTabletContext();

            Recenter_on_prev_monitor();

            this.trackBar_BrushSize.Value = this.paintsettings.BrushWidth;
            this.label_BrushSizeValue.Text = this.paintsettings.BrushWidth.ToString();

            // Default to no smoothing
            this.AppSetPositionSmoothing(0);
            this.AppSetPressureSmoothing(0);
        }

        private void Recenter_on_prev_monitor()
        {
            // brings the app window into the middle of the third monitor
            // allieviates the hassle of dragging it over every time

            var devicename = (string)Properties.Settings.Default["Monitor"];

            if ((devicename.Length > 0))
            {
                foreach (var s in System.Windows.Forms.Screen.AllScreens)
                {
                    if (s.DeviceName == devicename)
                    {
                        this.Left = s.Bounds.Left + (s.Bounds.Width / 2) - (this.Width / 2);
                        this.Top = s.Bounds.Top + (s.Bounds.Height / 2) - (this.Height / 2);
                    }
                }
            }
            else
            {
                // do nothing
            }

        }

        public void AppSetPositionSmoothing(double value)
        {
            value = PaintSettings.SMOOTHING_RANGE.Clamp(value);
            this.set_position_smoothing(value);
            this.trackBar_PositionSmoothing.Value = this.SMOOTHING_TRACKBAR_RANGE.Clamp((int)(100 * value));
        }

        public void AppSetPressureSmoothing(double value)
        {
            value = PaintSettings.SMOOTHING_RANGE.Clamp(value);
            this.set_pressure_smoothing(value);
            this.trackBar_PressureSmoothing.Value = this.SMOOTHING_TRACKBAR_RANGE.Clamp((int)(100 * value));
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.CloseTabletContext();

            if (this.bitmap_doc != null)
            {
                this.bitmap_doc.Dispose();
            }

            var s = Screen.FromControl(this);

            Properties.Settings.Default["Monitor"] = s.DeviceName;
            Properties.Settings.Default.Save();

        }

        private WintabDN.CWintabContext OpenTabletContext()
        {
            var context = WintabDN.CWintabInfo.GetDefaultSystemContext(WintabDN.ECTXOptionValues.CXO_MESSAGES);

            if (context == null)
            {
                System.Windows.Forms.MessageBox.Show("Failed to get digitizing context");
            }

            context.Name = "Digitizer Context";
            context.Options |= (uint)WintabDN.ECTXOptionValues.CXO_SYSTEM;

            this.tablet_info.Initialize();

            this.label_DeviceValue.Text = this.tablet_info.DeviceName ?? DefaultTabletDeviceName;

            // In Wintab, the tablet origin is lower left.  Move origin to upper left
            // so that it coincides with screen origin.

            context.OutExtY = -context.OutExtY;

            var status = context.Open();
            this.wintab_data = new WintabDN.CWintabData(context);
            this.wintab_data.SetWTPacketEventHandler(WinTabPacketHandler);

            return context;
        }

        private void WinTabPacketHandler(Object sender, WintabDN.MessageReceivedEventArgs args)
        {
            if (this.wintab_data == null)
            {
                // this case can happen when you use the pen to close the window
                // by clicking x in the upper right of the window
                return;
            }

            uint pktId = (uint)args.Message.WParam;
            var wintab_pkt = this.wintab_data.GetDataPacket((uint)args.Message.LParam, pktId);

            if (wintab_pkt.pkContext == wintab_context.HCtx)
            {
                // collect all the information we need to start painting
                var paint_data = new Painting.PaintData(wintab_pkt, tablet_info, this.paintsettings, Screen_loc_to_canvas_loc);
                HandlePainting(paint_data);
                this.old_paintdata = paint_data;
            }
        }


        private void HandlePainting( PaintData paint_data)
        {
            var pos = paint_data.CanvasPos;
            var pos_smoothed = paint_data.CanvasPosSmoothed;

            // Update the UI 
            UpdateUIWithPaintData(paint_data, pos);

            if ((paint_data.PressureRaw > 0)
                && (this.bitmap_doc.Contains(pos))) 
            {
                bool need_to_draw = (this.old_paintdata.Status == PaintDataStatus.INVALID)
                    || (this.old_paintdata.CanvasPosSmoothed != paint_data.CanvasPosSmoothed)
                    || (this.old_paintdata.PressureEffective != paint_data.PressureEffective);

                if (need_to_draw)
                {
                    this.bitmap_doc.DrawDabCenteredAt(
                        clr_black,
                        pos_smoothed,
                        paint_data.BrushWidthEffective);

                    this.pictureBox_Canvas.Invalidate();
                }
            }
        }

        public Geometry.Point Screen_loc_to_canvas_loc(Geometry.Point screen_loc)
        {
            var canv_loc = this.pictureBox_Canvas.Location.ToPoint();
            var adjusted_pos = screen_loc.Subtract(canv_loc);
            var penpos_canvas = this.PointToClient(adjusted_pos).ToPoint();
            return penpos_canvas;
        }
        private void UpdateUIWithPaintData(Painting.PaintData paint_data, Geometry.Point penpos_canvas)
        {
            this.label_ScreenPosValue.Text = paint_data.PenPos.ToStringXY();
            this.label_CanvasPos.Text = penpos_canvas.ToStringXY();
            this.label_PressureValue.Text =
                string.Format("{0} → {1}", Math.Round(paint_data.PressureNormalized, 5), Math.Round(paint_data.PressureEffective, 5));
            this.label_TiltValue.Text = string.Format("(ALT:{0}, AZ:{1})", paint_data.TiltAltitude, paint_data.TiltAzimuth);
        }

        private void CloseTabletContext()
        {
            this.wintab_data.ClearWTPacketEventHandler();
            this.wintab_data = null;

            if (this.wintab_context == null)
            {
                return;
            }

            this.wintab_context.Close();
            this.wintab_context = null;
        }

        private void button_Clear_Click(object sender, EventArgs e)
        {
            this.EraseCanvas();
        }

        private void EraseCanvas()
        {
            this.bitmap_doc.Erase();
            this.pictureBox_Canvas.Invalidate();
        }

        private void trackBar_BrushSize_Scroll(object sender, EventArgs e)
        {
            this.paintsettings.BrushWidth = this.trackBar_BrushSize.Value;
            this.label_BrushSizeValue.Text = this.paintsettings.BrushWidth.ToString();
        }



        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {

            if (keyData == (Keys.Delete))
            {
                this.EraseCanvas();
                return true;
            }
            else if (keyData == (Keys.OemOpenBrackets))
            {
                this.relative_modify_brush_size(-1);
                return true;
            }
            else if (keyData == (Keys.OemCloseBrackets))
            {
                this.relative_modify_brush_size(1);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        void relative_modify_brush_size(int value)
        {
            var w = this.paintsettings.BrushWidth + value;

            this.paintsettings.BrushWidth = PaintSettings.BRUSHSIZE_RANGE.Clamp(w);
            this.label_BrushSizeValue.Text = this.paintsettings.BrushWidth.ToString();
            this.trackBar_BrushSize.Value = this.paintsettings.BrushWidth;
        }
        private void MenuFileSave_Click(object sender, EventArgs e)
        {
            AppSave();
        }

        private void AppSave()
        {
            if (this.filename != null)
            {
                _save();
            }
            else
            {
                AppSaveAs();

            }
        }

        private void _save()
        {
            try
            {
                this.bitmap_doc.Save(this.filename);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to save " + this.filename);
            }
        }

        private void AppSaveAs()
        {

            var sfd = new SaveFileDialog();
            sfd.FileName = this.filename ?? FileSaveDefaultFilename;
            sfd.DefaultExt = FileSaveDefaultExt;
            sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            var v = sfd.ShowDialog();

            if (v == DialogResult.OK)
            {
                string mydocs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                this.filename = sfd.FileName;
                this._save();
            }
            else
            {
                //do nothing
            }
        }

        private void MenuItem_SaveAs_Click(object sender, EventArgs e)
        {
            this.AppSaveAs();
        }

        private void MenuItem_Open_Click(object sender, EventArgs e)
        {
            this.AppOpen();

        }

        public void AppOpen()
        {


            var ofd = new OpenFileDialog();
            ofd.DefaultExt = FileOpenDefaultExt;
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            var v = ofd.ShowDialog();
            if (v == DialogResult.OK)
            {
                this.bitmap_doc.Load(ofd.FileName);
                this.filename = ofd.FileName;
                this.pictureBox_Canvas.Image = this.bitmap_doc.background_layer.Bitmap;
                this.pictureBox_Canvas.Invalidate();
            }
            else
            {
                // do nothing
            }
        }


        private void trackBar_PositionSmoothing_Scroll(object sender, EventArgs e)
        {
            this.set_position_smoothing(this.trackBar_PositionSmoothing.Value / this.trackBar_PositionSmoothing.Maximum);
        }

        private void trackBar_PressureSmoothing_Scroll(object sender, EventArgs e)
        {
            this.set_pressure_smoothing(this.trackBar_PressureSmoothing.Value / this.trackBar_PressureSmoothing.Maximum);
        }

        public void set_position_smoothing(double value)
        {
            this.paintsettings.PositionSmoothingAmount = PaintSettings.SMOOTHING_RANGE_LIMITED.Clamp(value);
            this.paintsettings.PositionSmoother.SmoothingAmount = this.paintsettings.PositionSmoothingAmount;
        }

        public void set_pressure_smoothing(double value)
        {
            this.paintsettings.PressureSmoothingAmount = PaintSettings.SMOOTHING_RANGE_LIMITED.Clamp(value);
            this.paintsettings.PressureSmoother.SmoothingAmount = this.paintsettings.PressureSmoothingAmount;
        }

        private void showShortcutsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormShortcuts();
            form.ShowDialog();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormAbout();
            form.ShowDialog();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.EraseCanvas();

        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void pressureCurveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormCurve(this.paintsettings.pressure_curve.BendAmount);
            var r = form.ShowDialog(this);
            if (r == DialogResult.OK)
            {

                this.paintsettings.pressure_curve.BendAmount = form.CurveAmount;
            }

        }

    }
}
