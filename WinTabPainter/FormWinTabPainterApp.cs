using System;
using SD=System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;
using WinTabPainter.Painting;
using WinTabPainter.Geometry;
using System.Drawing;
using WinTabUtils;
using WintabDN;

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


        private WinTabUtils.TabletSession tabsession;
        public Painting.PaintSettings paint_settings = new Painting.PaintSettings();
        Painting.PaintData old_paintdata;

        private Painting.BitmapDocument bitmap_doc;

        public string filename = null;
        string FileSaveDefaultFilename = "Untitled.png";
        string FileSaveDefaultExt = "png";
        string FileOpenDefaultExt = "png";
        string DefaultTabletDeviceName = "UKNOWN_DEVICE";

        ColorARGB clr_black = new Painting.ColorARGB(255, 0, 0, 0);
        Numerics.Range SMOOTHING_TRACKBAR_RANGE = new Numerics.Range(-100, 100);


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

            this.tabsession = new WinTabUtils.TabletSession();
            this.tabsession.PacketHandler = this.PacketHandler;
            this.tabsession.Open(WinTabUtils.TabletContextType.System);

            this.tabsession.ButtonChangedHandler = this.HandleButtonChange;

            Reposition_app();

            this.trackBar_BrushSize.Value = paint_settings.BrushWidth;
            this.label_BrushSizeValue.Text = paint_settings.BrushWidth.ToString();

            // Default to no smoothing
            paint_settings.PositionSmoother.SmoothingAmount = 0.0;
            paint_settings.PressureSmoother.SmoothingAmount = 0.0;
        }

        private void Reposition_app()
        {

            var monitorname = (string)Properties.Settings.Default["Monitor"];

            // if no value, do nothing
            if (monitorname.Length == 0)
            {
                return;
            }

            // find the screen with the same name
            foreach (var s in System.Windows.Forms.Screen.AllScreens)
            {
                if (s.DeviceName == monitorname)
                {
                    // place this app on that screen
                    this.Left = s.Bounds.Left + (s.Bounds.Width / 2) - (this.Width / 2);
                    this.Top = s.Bounds.Top + (s.Bounds.Height / 2) - (this.Height / 2);
                    break;
                }
            }

        }


        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.tabsession.Close();

            if (this.bitmap_doc != null)
            {
                this.bitmap_doc.Dispose();
            }

            var s = Screen.FromControl(this);

            Properties.Settings.Default["Monitor"] = s.DeviceName;
            Properties.Settings.Default.Save();

        }

        char[] button_status = new char[3] {
            get_press_change_as_letter(PenButtonPressChangeType.Released),
            get_press_change_as_letter(PenButtonPressChangeType.Released),
            get_press_change_as_letter(PenButtonPressChangeType.Released)};

        private void PacketHandler(WintabDN.WintabPacket wintab_pkt)
        {
            var button_info = new WinTabUtils.PenButtonPressChange(wintab_pkt.pkButtons);

            Update_UI_Button_status(button_info);

            // collect all the information we need to start painting
            var paint_data = new Painting.PaintData(wintab_pkt, this.tabsession.TabletInfo, paint_settings, Screen_loc_to_canvas_loc);

            HandlePainting(paint_data);
            this.old_paintdata = paint_data;
        }

        private void Update_UI_Button_status(WinTabUtils.PenButtonPressChange button_info)
        {
            if (button_info.Change != 0)
            {
                int index = button_info.ButtonId switch
                {
                    WinTabUtils.PenButtonIdentifier.Tip => 0,
                    WinTabUtils.PenButtonIdentifier.LowerButton => 1,
                    WinTabUtils.PenButtonIdentifier.UpperButton => 2,
                    _ => throw new System.ArgumentOutOfRangeException()
                };

                button_status[index] = get_press_change_as_letter(button_info.Change);

            }

            this.label_ButtonsValue.Text = new string(this.button_status);
        }

        private static char get_press_change_as_letter(PenButtonPressChangeType change)
        {
            return change switch
            {
                WinTabUtils.PenButtonPressChangeType.Pressed => 'D',
                WinTabUtils.PenButtonPressChangeType.Released => 'U',
                _ => throw new System.ArgumentOutOfRangeException()
            };
        }

        private void HandlePainting(PaintData paint_data)
        {
            double dist_from_last_canv_pos;

            if (old_paintdata.Status == PaintDataStatus.INVALID)
            {
                dist_from_last_canv_pos = 0;
            }
            else
            {
                dist_from_last_canv_pos = paint_data.PosCanvasEffective.DistanceTo(old_paintdata.PosCanvasEffective);
            }

            // Update the UI 
            UpdateUIWithPaintData(paint_data, paint_data.PosCanvas);

            if ((paint_data.PressureRaw > 0)
                && (this.bitmap_doc.Contains(paint_data.PosCanvas)))
            {
                bool need_to_draw = (this.old_paintdata.Status == PaintDataStatus.INVALID)
                    || (this.old_paintdata.PosCanvasEffective != paint_data.PosCanvasEffective)
                    || (this.old_paintdata.PressureEffective != paint_data.PressureEffective);

                if (need_to_draw)
                {

                    this.bitmap_doc.DrawDabCenteredAt(
                        clr_black,
                        paint_data.PosCanvasEffective,
                        paint_data.BrushWidthEffective);

                    this.pictureBox_Canvas.Invalidate();
                }
            }
        }

        public void HandleButtonChange(WintabDN.WintabPacket pkt, WinTabUtils.PenButtonPressChange change)
        {
            if (change.ButtonId == PenButtonIdentifier.Tip)
            {
                if (change.Change == PenButtonPressChangeType.Pressed)
                {
                    // we need to reset the smoothing 
                    // whenever the pen tip touches the tablet
                    // if we don't do this the previous stroke will
                    // have influence on the new stroke
                    paint_settings.PositionSmoother.Reset();
                    paint_settings.PressureSmoother.Reset();
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
            this.label_ScreenPosValue.Text = paint_data.PosScreen.ToStringXY();
            this.label_CanvasPos.Text = penpos_canvas.ToStringXY();
            this.label_PressureValue.Text =
                string.Format("{0} → {1}", Math.Round(paint_data.PressureNormalized, 5), Math.Round(paint_data.PressureEffective, 5));
            this.label_TiltValue.Text = string.Format("{0:0.0}°, {1:0.0}°", paint_data.TiltAltitude, paint_data.TiltAzimuth);
            this.labeltilt_xy.Text = string.Format("{0:0.0}°, {1:0.0}°", paint_data.TiltX, paint_data.TiltY);
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
            paint_settings.BrushWidth = this.trackBar_BrushSize.Value;
            this.label_BrushSizeValue.Text = paint_settings.BrushWidth.ToString();
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
            var w = paint_settings.BrushWidth + value;
            paint_settings.BrushWidth = w;
            this.label_BrushSizeValue.Text = paint_settings.BrushWidth.ToString();
            this.trackBar_BrushSize.Value = paint_settings.BrushWidth;
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
            this.bitmap_doc.Save(this.filename);

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
            var form = new FormBrushSettings(paint_settings.pressure_curve.BendAmount);
            form.PaintSettings = this.paint_settings;
            var r = form.ShowDialog(this);
            if (r == DialogResult.OK)
            {

                paint_settings.pressure_curve.BendAmount = form.CurveAmount;
                paint_settings.PressureSmoother.SmoothingAmount = form.PressureSmoothingValue;
                paint_settings.PositionSmoother.SmoothingAmount = form.PositionSmoothingValue;
                paint_settings.PressureQuantizeLevels = form.PressureQuant;

            }

        }

        private void aboutTabletToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormTablet();
            form.tablet_info = this.tabsession.TabletInfo;

            var r = form.ShowDialog(this);
        }
    }
}
