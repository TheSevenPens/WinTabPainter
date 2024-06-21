using System;
using SD=System.Drawing;
using System.Windows.Forms;

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



        public FormWinTabPainterApp()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // All actual drawing will be done to this bitmap
            this.bitmap_doc = new BitmapDocument(1000, 1000);

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
            this.paintsettings.Smoother = new EMASmoother(0);
            this.trackBar_Smoothing.Value = 0;
            this.set_smoothing(0);
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

            if (tablet_info.Device!= null)
            {
                this.label_DeviceValue.Text = this.tablet_info.Device;
            }
            else
            {
                this.label_DeviceValue.Text = "UNKNOWN";
            }


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

            uint pktId = (uint)args.Message.WParam;
            var wintab_pkt = this.wintab_data.GetDataPacket((uint)args.Message.LParam, pktId);

            if (wintab_pkt.pkContext == wintab_context.HCtx)
            {
                // collect all the information we need to start painting
                var paint_data = new PaintData(wintab_pkt, tablet_info, this.paintsettings);

                // Update the UI based on paint data
                UpdateUIForPainting(paint_data);

                if (wintab_pkt.pkNormalPressure > 0)
                {
                    double scale = 2.5; // Seems tied to the screen scaling I am using on Windows on my specific machine. Need to remove this constant

                    // convert the pen position from the screen to the app's client area
                    var canvas_topleft = new SD.Point(this.pictureBox_Canvas.Left, this.pictureBox_Canvas.Top);
                    var penpos_screen = paint_data.PenPosition.Divide(scale);
                    var penpos_canvas = this.PointToClient(penpos_screen.Subtract(canvas_topleft));

                    // the pen is not in the client area, abandon doing anything
                    if (penpos_canvas.X < 0) { return; }
                    if (penpos_canvas.Y < 0) { return; }

                    var penpos_client_smoothed = this.paintsettings.Smoother.Smooth(penpos_canvas.ToPointD());

                    var dab_size = new SD.Size(paint_data.BrushWidthAdjusted, paint_data.BrushWidthAdjusted);
                    this.bitmap_doc.DrawDabCenteredAt(SD.Color.Black, penpos_client_smoothed.ToPoint(), dab_size);

                    this.pictureBox_Canvas.Invalidate();
                }
            }
        }

        private void UpdateUIForPainting(PaintData paint_data)
        {
            this.label_PosXValue.Text = paint_data.PenX.ToString();
            this.label_PosYValue.Text = paint_data.PenY.ToString();
            this.label_PosZValue.Text = paint_data.PenZ.ToString();
            this.label_PressureRawValue.Text = paint_data.PressureRaw.ToString();
            this.label_PressureValue.Text = Math.Round(paint_data.PressureNormalized, 5).ToString();
            this.label_PressureAdjusted.Text = Math.Round(paint_data.PressureAdjusted, 5).ToString();
            this.label_AltitudeValue.Text = paint_data.Altitude.ToString();
            this.label_AzimuthValue.Text = paint_data.Azimuth.ToString();
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
            this.paintsettings.PressureCurveControl = ((double)this.trackBarPressureCurve.Value) / 100.0;
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
            var ofd = new SaveFileDialog();
            ofd.FileName = this.filename ?? "Untitled.png";
            ofd.DefaultExt = "png";
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            var v = ofd.ShowDialog();

            if (v == DialogResult.OK)
            {
                string mydocs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                this.filename = ofd.FileName;
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
            //ofd.FileName = this.filename ?? "Untitled.png";
            ofd.DefaultExt = "png";
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            var v = ofd.ShowDialog();
            if (v == DialogResult.OK) 
            {
                this.bitmap_doc.Load(ofd.FileName);
                this.filename = ofd.FileName;
            }
            this.pictureBox_Canvas.Image = this.bitmap_doc.Bitmap;
            this.pictureBox_Canvas.Invalidate();
        }


        private void trackBar_Smoothing_Scroll(object sender, EventArgs e)
        {
            this.set_smoothing(this.trackBar_Smoothing.Value / this.trackBar_Smoothing.Maximum);
        }

        public void set_smoothing(double value)
        {
            this.paintsettings.PositionSmoothingAmount = value;
            this.paintsettings.PositionSmoothingAmount = System.Math.Min(this.paintsettings.PositionSmoothingAmount, this.paintsettings.SMOOTHING_MAX);
            this.paintsettings.PositionSmoothingAmount = System.Math.Max(this.paintsettings.PositionSmoothingAmount, this.paintsettings.SMOOTHING_MIN);
            this.paintsettings.Smoother.Alpha = this.paintsettings.PositionSmoothingAmount;
        }
    }
}
