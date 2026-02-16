using SevenLib.WinTab.Stylus;
using System;
// References:
// https://github.com/DennisWacom/WintabControl/tree/master/WintabControl
// https://github.com/DennisWacom/InkPlatform/tree/master/WintabDN
// https://developer-docs.wacom.com/docs/icbt/windows/wintab/wintab-basics/
// https://www.nuget.org/packages/WacomSolutionPartner.WintabDotNet
// https://github.com/Wacom-Developer/wacom-device-kit-windows/tree/master/Wintab%20TiltTest

using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WinTabPainter.GeometryExtensions;
using WinTabPainter.Painting;

namespace WinTabPainter
{
    public partial class FormWinTabPainterApp : Form
    {



        private SevenLib.WinTab.Tablet.WinTabSession2 tabsession;
        public Painting.PaintSettings paint_settings = new Painting.PaintSettings();
        Painting.PaintData old_paintdata;

        private Painting.BitmapDocument bitmap_doc;

        public string filename = null;
        string FileSaveDefaultFilename = "Untitled.png";
        string FileSaveDefaultExt = "png";
        string FileOpenDefaultExt = "png";

        ColorARGB clr_black = new Painting.ColorARGB(255, 0, 0, 0);
        SevenLib.Numerics.OrderedRange SMOOTHING_TRACKBAR_RANGE = new SevenLib.Numerics.OrderedRange(-100, 100);



        public FormWinTabPainterApp()
        {
            InitializeComponent();
        }

        Graphics gfx_pressure_guage2;
        Pen np_pressure_guage = new Pen(Color.Black, 11);
        Pen ep_pressure_guage = new Pen(Color.Red, 11);

        private void Form1_Load(object sender, EventArgs e)
        {
            this.pictureBoxPressureGuage.Image = new System.Drawing.Bitmap(this.pictureBoxPressureGuage.Width, this.pictureBoxPressureGuage.Height);
            this.gfx_pressure_guage2 = System.Drawing.Graphics.FromImage(this.pictureBoxPressureGuage.Image);

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


            this.tabsession = new SevenLib.WinTab.Tablet.WinTabSession2();
            this.tabsession.OnRawPacketReceived = this.PacketHandler;
            this.tabsession.Open(SevenLib.WinTab.Tablet.TabletContextType.System);

            this.tabsession.OnButtonStateChanged = this.HandleButtonChange;

            this.labelMaxPressure.Text = this.tabsession.TabletInfo.MaxPressure.ToString();
            Reposition_app();

            this.trackBar_BrushSize.Value = paint_settings.BrushWidth;
            this.label_BrushSizeValue.Text = paint_settings.BrushWidth.ToString();


            this.recorded_packets = new List<SevenLib.WinTab.Structs.WintabPacket>(this.max_rec_packets);

            this.UpdateRecStatus();

            // Default to no smoothing
            paint_settings.PositionSmoothingAmount = 0.0;
            paint_settings.PressureSmoothingAmount = 0.0;

            this.paint_settings.AntiAliasing = true;
            this.bitmap_doc.AntiAliasing = this.paint_settings.AntiAliasing;

            this.update_config();
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
            // Resource cleanup moved to Dispose()
            
            var s = Screen.FromControl(this);

            Properties.Settings.Default["Monitor"] = s.DeviceName;
            Properties.Settings.Default.Save();

        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                 if (components != null)
                 {
                    components.Dispose();
                 }

                 this.tabsession?.Dispose();
                 this.bitmap_doc?.Dispose();
                 this.gfx_pressure_guage2?.Dispose();
                 this.np_pressure_guage?.Dispose();
                 this.ep_pressure_guage?.Dispose();
                 this.pictureBoxPressureGuage.Image?.Dispose();
            }
            base.Dispose(disposing);
        }

        char[] button_status = new char[4] {
            get_press_change_as_letter(StylusButtonChangeType.Released),
            get_press_change_as_letter(StylusButtonChangeType.Released),
            get_press_change_as_letter(StylusButtonChangeType.Released),
            get_press_change_as_letter(StylusButtonChangeType.Released),};

        private void PacketHandler(SevenLib.WinTab.Structs.WintabPacket wintab_pkt)
        {
            if (this.RecStat == RecStatusEnum.Recording)
            {
                RecordPacket(wintab_pkt);
            }
            var button_info = new SevenLib.WinTab.Stylus.StylusButtonChange(wintab_pkt.pkButtons);

            Update_UI_Button_status(button_info);

            // collect all the information we need to start painting
            var paint_data = new Painting.PaintData(wintab_pkt, this.tabsession.TabletInfo, paint_settings, Screen_loc_to_canvas_loc);

            HandlePainting(paint_data);
            this.old_paintdata = paint_data;
        }



        private void Update_UI_Button_status(StylusButtonChange button_info)
        {
            if (button_info.Change != StylusButtonChangeType.NoChange)
            {
                int index = button_info.ButtonId switch
                {
                    SevenLib.Stylus.StylusButtonId.Tip => 0,
                    SevenLib.Stylus.StylusButtonId.LowerButton => 1,
                    SevenLib.Stylus.StylusButtonId.UpperButton => 2,
                    SevenLib.Stylus.StylusButtonId.BarrelButton => 3,
                    _ => throw new System.ArgumentOutOfRangeException()
                };

                button_status[index] = get_press_change_as_letter(button_info.Change);

            }

            this.label_ButtonsValue.Text = new string(this.button_status);
        }

        private static char get_press_change_as_letter(StylusButtonChangeType change)
        {
            return change switch
            {
                StylusButtonChangeType.Pressed => 'D',
                StylusButtonChangeType.Released => 'U',
                _ => throw new System.ArgumentOutOfRangeException()
            };
        }

        PaintData cur_paintdata;
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
            this.cur_paintdata = paint_data;
            UpdateLivePaintStats(paint_data, paint_data.PosCanvas);

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

        public void HandleButtonChange(SevenLib.WinTab.Structs.WintabPacket pkt, StylusButtonChange change)
        {
            if (change.ButtonId == SevenLib.Stylus.StylusButtonId.Tip)
            {
                if (change.Change == StylusButtonChangeType.Pressed)
                {
                    // we need to reset the smoothing 
                    // whenever the pen tip touches the tablet
                    // if we don't do this the previous stroke will
                    // have influence on the new stroke
                    paint_settings.Dynamics.PositionSmoother.Reset();
                    paint_settings.Dynamics.PressureSmoother.Reset();
                }
            }

        }
        public SevenLib.Geometry.Point Screen_loc_to_canvas_loc(SevenLib.Geometry.Point screen_loc)
        {
            var canv_loc = this.pictureBox_Canvas.Location.ToPoint();
            var adjusted_pos = screen_loc.Subtract(canv_loc);
            var penpos_canvas = this.PointToClient(adjusted_pos.ToSDPoint()).ToPoint();
            return penpos_canvas;
        }

        private string get_pressure_string(double pressure)
        {
            int pressure_digits = 8;
            double pressure_rounded = Math.Round(pressure * 100, pressure_digits);
            string str_pressure = string.Format("{0:00.00000}%", pressure_rounded);
            return str_pressure;
        }
        private void UpdateLivePaintStats(Painting.PaintData paint_data, SevenLib.Geometry.Point penpos_canvas)
        {
            this.label_ScreenPosValue.Text = paint_data.PosScreen.ToStringXY();
            this.label_CanvasPos.Text = penpos_canvas.ToStringXY();
            this.labelPressureValueInteger.Text = paint_data.PressureRaw.ToString();
            //this.panelPressureGuage.Invalidate();
            this.pictureBoxPressureGuage.Invalidate();
            this.label_PressureValue.Text =
                string.Format("{0} → {1}",
                    get_pressure_string(paint_data.PressureNormalized),
                    get_pressure_string(paint_data.PressureEffective));

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

            this.update_config();
        }

        void update_config()
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine(string.Format("Brush Size: {0}", paint_settings.BrushWidth));
            sb.AppendLine(string.Format("Pressure Curve: {0}", paint_settings.PressureCurveAmount));
            sb.AppendLine(string.Format("Pressure Smoothing: {0}", paint_settings.PressureSmoothingAmount));
            sb.AppendLine(string.Format("Position Smoothing: {0}", paint_settings.PositionSmoothingAmount));
            sb.AppendLine(string.Format("Pressure Quantization: {0}",
                (paint_settings.PressureQuantizeLevels < 1) ? "NONE" : paint_settings.PressureQuantizeLevels.ToString()));
            sb.AppendLine(string.Format("Anit-Aliasing: {0}", paint_settings.AntiAliasing));
            sb.AppendLine(string.Format("{0},{1}", paint_settings.PostionNoiseX, paint_settings.PositionNoiseY));

            this.textBox_config.Text = sb.ToString();
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
            RunFormSettings();
        }

        private void RunFormSettings()
        {
            var form = new FormBrushSettings();
            this.paint_settings.CopyTo(form.paintsettings);

            var r = form.ShowDialog(this);
            if (r == DialogResult.OK)
            {
                form.paintsettings.CopyTo(this.paint_settings);
                this.update_config();
                this.bitmap_doc.AntiAliasing = paint_settings.AntiAliasing;
            }
        }

        private void aboutTabletToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormTablet();
            form.tablet_info = this.tabsession.TabletInfo;

            var r = form.ShowDialog(this);
        }

        private void button_Clear_Click_1(object sender, EventArgs e)
        {
            this.EraseCanvas();
        }

        private void buttonRec_Click_1(object sender, EventArgs e)
        {
            ToggleRecordingPackets();
        }



        private void button_replay_Click(object sender, EventArgs e)
        {
            ReplayPackets();
        }



        private void buttonClearRecording_Click(object sender, EventArgs e)
        {
            ClearRecording();

        }


        private void buttonSavePackets_Click(object sender, EventArgs e)
        {
            SavePackets();
        }



        private void buttonLoadPackets_Click(object sender, EventArgs e)
        {
            LoadPackets();
        }



        private void buttonCopy_Click(object sender, EventArgs e)
        {
            this.bitmap_doc.CopyToClipboard();
        }

        private void buttonSettings_Click(object sender, EventArgs e)
        {
            RunFormSettings();
        }

        private void panelPressureGuage_Paint(object sender, PaintEventArgs e)
        {


        }

        private void pictureBoxPressureGuage_Paint(object sender, PaintEventArgs e)
        {
            if (this.gfx_pressure_guage2 == null) { return; }

            int guage_width = this.pictureBoxPressureGuage.Width;
            int guage_height = this.pictureBoxPressureGuage.Height;
            int nx = (int)(guage_width * this.cur_paintdata.PressureNormalized);
            using (var pack = new System.Drawing.SolidBrush(System.Drawing.Color.Wheat))
            {
                gfx_pressure_guage2.FillRectangle(pack, new System.Drawing.Rectangle(0, 0, guage_width, guage_height));

            }
            gfx_pressure_guage2.DrawLine(np_pressure_guage, nx, 0, nx, guage_height);

            if (this.cur_paintdata.PressureEffective != this.cur_paintdata.PressureNormalized)
            {
                int ex = (int)(guage_width * this.cur_paintdata.PressureEffective);
                gfx_pressure_guage2.DrawLine(ep_pressure_guage, ex, 0, ex, guage_height);

            }

        }
    }
}
