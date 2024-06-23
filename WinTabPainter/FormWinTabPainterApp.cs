using System;
using SD=System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;

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

        private BitmapDocument bitmap_doc;

        public string filename = null;
        TabletInfo tablet_info = new TabletInfo();

        public PaintSettings paintsettings = new PaintSettings();

        string FileSaveDefaultFilename = "Untitled.png";
        string FileSaveDefaultExt = "png";
        string FileOpenDefaultExt = "png";
        string DefaultTabletDeviceName = "UKNOWN_DEVICE";



        public FormWinTabPainterApp()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // All actual drawing will be done to this bitmap
            this.bitmap_doc = new BitmapDocument(500, 500);

            // Create a graphics object for the canvas bitmap and enable smoothing
            // by default for better looking stroke edges


            // Double-buffering to reduce flicket
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            // Link the bitmap canvas to the picturebox
            // changes to the bitmap will be rendered
            // whenever the picturebox is redrawn

            this.pictureBox_Canvas.Image = this.bitmap_doc.Bitmap;
            this.EraseCanvas();

            this.wintab_context = OpenTabletContext();

            // This is for debugging only
            // brings the app window into the middle of the third monitor
            // allieviates the hassle of dragging it over every time


            if (System.Windows.Forms.SystemInformation.MonitorCount >= 3)
            {
                var screen = System.Windows.Forms.Screen.AllScreens[2];
                this.Left = screen.Bounds.Left + (screen.Bounds.Width / 2) - (this.Width / 2);
                this.Top = screen.Bounds.Top + (screen.Bounds.Height/2) - (this.Height / 2);
            }

            this.trackBar_BrushSize.Value = this.paintsettings.BrushWidth;
            this.label_BrushSizeValue.Text = this.paintsettings.BrushWidth.ToString();

            // Default to no smoothing
            this.paintsettings.PositionSmoother = new Geometry.EMAPositionSmoother(0);
            this.paintsettings.PressureSmoother = new Geometry.EMASmoother(0);
            this.trackBar_Smoothing.Value = 0;
            this.set_position_smoothing(0);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.CloseTabletContext();

            if (this.bitmap_doc != null)
            {
                this.bitmap_doc.Dispose();
            }

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

        public bool IsPointInsideCanvas(Geometry.Point p)
        {
            if (p.X < 0) { return false; }
            if (p.Y < 0) { return false; }
            if (p.X >= this.pictureBox_Canvas.Width) { return false; }
            if (p.Y >= this.pictureBox_Canvas.Height) { return false; }
            return true;
        }

        private void WinTabPacketHandler(Object sender, WintabDN.MessageReceivedEventArgs args)
        {

            uint pktId = (uint)args.Message.WParam;
            var wintab_pkt = this.wintab_data.GetDataPacket((uint)args.Message.LParam, pktId);

            if (wintab_pkt.pkContext == wintab_context.HCtx)
            {
                // collect all the information we need to start painting
                var paint_data = new PaintData(wintab_pkt, tablet_info, this.paintsettings);

                // Take the position of the pen - which is pixel units on the screen
                // and convert that to coordinates of the bitmap document canvas
                double scale = 2.5; // hardcoded to deal with windows scaling. Need to find a more general way
                var penpos_canvas = this.Screen_loc_to_canvas_loc(paint_data.PenPos, scale);
                var penpos_canvas_smoothed = this.Screen_loc_to_canvas_loc(paint_data.PenPosSmoothed, scale);

                // Update the UI 
                UpdateUIWithPaintData(paint_data, penpos_canvas);

                var clr_black = new ColorARGB(255, 0, 0, 0);
                if ((paint_data.PressureRaw > 0) 
                    && (this.IsPointInsideCanvas(penpos_canvas)))
                {

                    this.bitmap_doc.DrawDabCenteredAt(
                        clr_black,
                        penpos_canvas_smoothed,
                        paint_data.BrushWidthAdjusted);

                    this.pictureBox_Canvas.Invalidate();
                }
            }
        }

        public Geometry.Point Screen_loc_to_canvas_loc( Geometry.Point screen_loc, double scale)
        {
            var canv_loc = this.pictureBox_Canvas.Location;
            var px = (int)((screen_loc.X / scale) - canv_loc.X);
            var py = (int)((screen_loc.Y / scale) - canv_loc.Y);
            var penpos_canvas = this.PointToClient(new SD.Point(px, py)).ToPoint();
            return penpos_canvas;
        }
        private void UpdateUIWithPaintData(PaintData paint_data, Geometry.Point penpos_canvas)
        {
            this.label_ScreenPosValue.Text = paint_data.PenPos.ToStringXY();
            this.label_CanvasPos.Text = penpos_canvas.ToStringXY();
            this.label_PressureValue.Text = Math.Round(paint_data.PressureNormalized, 5).ToString();
            this.label_PressureAdjusted.Text = Math.Round(paint_data.PressureCurved, 5).ToString();
            this.label_TiltValue.Text = string.Format("(ALT:{0}, AZ:{1})", paint_data.TiltAltitude, paint_data.TiltAzimuth);
        }

        private void CloseTabletContext()
        {
            if (this.wintab_context == null)
            {
                return;
            }

            this.wintab_context.Close();
            this.wintab_context = null;
            this.wintab_data.ClearWTPacketEventHandler();
            this.wintab_data = null;
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

        private void trackBarPressureCurve_Scroll(object sender, EventArgs e)
        {
            this.paintsettings.pressure_curve.SetBendAmount(((double)this.trackBarPressureCurve.Value) / 100.0);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {

            if (keyData == (Keys.Delete))
            {
                this.EraseCanvas();
                return true;
            }
            else if (keyData == ( Keys.OemOpenBrackets) )
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
            this.paintsettings.BrushWidth = this.paintsettings.BrushWidth + value;
            this.paintsettings.BrushWidth = Math.Max(1, this.paintsettings.BrushWidth);
            this.paintsettings.BrushWidth = Math.Min(100, this.paintsettings.BrushWidth);

            this.label_BrushSizeValue.Text = this.paintsettings.BrushWidth.ToString();
            this.trackBar_BrushSize.Value= this.paintsettings.BrushWidth;
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
                this.pictureBox_Canvas.Image = this.bitmap_doc.Bitmap;
                this.pictureBox_Canvas.Invalidate();
            }
            else 
            {
                // do nothing
            }
        }


        private void trackBar_PositionSmoothing_Scroll(object sender, EventArgs e)
        {
            this.set_position_smoothing(this.trackBar_Smoothing.Value / this.trackBar_Smoothing.Maximum);
        }



        private void trackBar_PressureSmoothing_Scroll(object sender, EventArgs e)
        {
            this.set_pressure_smoothing(this.trackBar_PressureSmoothing.Value / this.trackBar_PressureSmoothing.Maximum);
        }

        public void set_position_smoothing(double value)
        {
            this.paintsettings.PositionSmoothingAmount = HelperMethods.ClampRange(value, this.paintsettings.SMOOTHING_MIN, this.paintsettings.SMOOTHING_MAX);
            this.paintsettings.PositionSmoother.Alpha = this.paintsettings.PositionSmoothingAmount;
        }

        public void set_pressure_smoothing(double value)
        {
            this.paintsettings.PressureSmoothingAmount = HelperMethods.ClampRange(value, this.paintsettings.SMOOTHING_MIN, this.paintsettings.SMOOTHING_MAX);
            this.paintsettings.PressureSmoother.Alpha = this.paintsettings.PressureSmoothingAmount;
        }

    }
}
