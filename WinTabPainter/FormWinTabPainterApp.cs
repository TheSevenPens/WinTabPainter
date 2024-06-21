using System;
using System.Drawing;
using System.Runtime.CompilerServices;
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
        PenInfo pen_info;
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

            this.trackBar_BrushSize.Value = this.paintsettings.brush_size;
            this.label_BrushSizeValue.Text = this.paintsettings.brush_size.ToString();

            // Default to no smoothing
            this.paintsettings.smoother = new EMASmoother(0);
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
                double adjusted_pressure = ApplyCurve(pen_info.PressureNormalized, this.paintsettings.pressure_curve_q);

                this.pen_info = new PenInfo();
                this.pen_info.X = wintab_pkt.pkX;
                this.pen_info.Y = wintab_pkt.pkY;
                this.pen_info.Z = wintab_pkt.pkZ;
                this.pen_info.PressureRaw = wintab_pkt.pkNormalPressure;
                this.pen_info.PressureNormalized = pen_info.PressureRaw / (double)this.tablet_info.MaxPressure;
                this.pen_info.Altitude = wintab_pkt.pkOrientation.orAltitude;
                this.pen_info.Azimuth = wintab_pkt.pkOrientation.orAzimuth;
                this.label_PosXValue.Text = this.pen_info.X.ToString();
                this.label_PosYValue.Text = this.pen_info.Y.ToString();
                this.label_PosZValue.Text = this.pen_info.Z.ToString();
                this.label_PressureRawValue.Text = this.pen_info.PressureRaw.ToString();
                this.label_PressureValue.Text = Math.Round(this.pen_info.PressureNormalized,5).ToString();
                this.label_PressureAdjusted.Text = Math.Round(adjusted_pressure,5).ToString();
                this.label_AltitudeValue.Text = this.pen_info.Altitude.ToString();
                this.label_AzimuthValue.Text = this.pen_info.Azimuth.ToString();

                if (wintab_pkt.pkNormalPressure > 0)
                {
                    double scale = 2.5; // to account for my screens scaling; need to abstract this away

                    var p_screen = new Point(pen_info.X,pen_info.Y).Divide(scale).Subtract(this.pictureBox_Canvas.Left,this.pictureBox_Canvas.Top);
                    var p_client = this.PointToClient(p_screen);


                    if (p_client.X < 0) { return; }
                    if (p_client.Y < 0) { return; }

                    this.paintsettings.smoother.Smooth(p_client.ToPointD());

                    int max_brush_size = this.paintsettings.brush_size;

                    var adjusted_brush_size = System.Math.Max(1, adjusted_pressure * max_brush_size);

                    var dab_rect_size = new Size((int) adjusted_brush_size, (int)adjusted_brush_size);
                    var p_dab_center = p_client.Subtract( dab_rect_size.Divide(2.0) );

                    if (this.paintsettings.smoother.Alpha!=0.0)
                    {
                        var s = paintsettings.smoother.Smooth(new PointD(p_dab_center.X, p_dab_center.Y));
                        p_dab_center = new Point((int)s.X, (int)s.Y);
                    }
                    var rect = new Rectangle(p_dab_center, dab_rect_size);

                    this.bitmap_doc.FillEllipse(Color.Black, rect);

                    this.pictureBox_Canvas.Invalidate();
                }
            }
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
            this.paintsettings.brush_size = this.trackBar_BrushSize.Value;
            this.label_BrushSizeValue.Text = this.paintsettings.brush_size.ToString();
        }

        private void trackBarPressureCurve_Scroll(object sender, EventArgs e)
        {
            this.paintsettings.pressure_curve_q = ((double)this.trackBarPressureCurve.Value) / 100.0;
        }

        private double ApplyCurve( double pressure, double q)
        {
            if (q < -1) { q = -1; }
            else if ( q > 1 ) { q = 1; }

            double new_pressure = pressure;

            if (q>0)
            {
                new_pressure = Math.Pow(pressure, 1.0-q);
            }
            else
            {
                new_pressure = Math.Pow(pressure,1.0/(1.0+q));
            }

            return new_pressure;
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
            this.paintsettings.brush_size = this.paintsettings.brush_size + value;
            this.paintsettings.brush_size = Math.Max(1, this.paintsettings.brush_size);
            this.paintsettings.brush_size = Math.Min(100, this.paintsettings.brush_size);

            this.label_BrushSizeValue.Text = this.paintsettings.brush_size.ToString();
            this.trackBar_BrushSize.Value= this.paintsettings.brush_size;
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
            this.paintsettings.smoothing = value;
            this.paintsettings.smoothing = System.Math.Min(this.paintsettings.smoothing, this.paintsettings.smoothing_max);
            this.paintsettings.smoothing = System.Math.Max(this.paintsettings.smoothing, this.paintsettings.smoothing_min);
            this.paintsettings.smoother.Alpha = this.paintsettings.smoothing;
        }
    }
}
