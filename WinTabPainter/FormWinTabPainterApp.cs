﻿using System;
using System.Drawing;
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
        PenInfo pen_info;

        double pressure_curve_q = 0.0;
        int brush_size = 5;
        TabletInfo tablet_info = new TabletInfo();

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

            // bring window to first display
            // Useful when debugging to avoid having to move the form over every time

            if (System.Windows.Forms.SystemInformation.MonitorCount > 1)
            {
                var screen = System.Windows.Forms.Screen.AllScreens[0];
                this.Left = screen.Bounds.Left + (screen.Bounds.Width / 2) - (this.Width / 2);
            }

            this.trackBar_BrushSize.Value = this.brush_size;
            this.label_BrushSizeValue.Text = this.brush_size.ToString();
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
                this.pen_info = new PenInfo();
                this.pen_info.X = wintab_pkt.pkX;
                this.pen_info.Y = wintab_pkt.pkY;
                this.pen_info.Z = wintab_pkt.pkZ;
                this.pen_info.PressureRaw = wintab_pkt.pkNormalPressure;
                this.pen_info.PressureNormalized = pen_info.PressureRaw / (double)this.tablet_info.MaxPressure;
                this.pen_info.Altitude = wintab_pkt.pkOrientation.orAltitude;
                this.pen_info.Azimuth = wintab_pkt.pkOrientation.orAzimuth;

                double adjusted_pressure = ApplyCurve(pen_info.PressureNormalized, this.pressure_curve_q);

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
                    double scale = 2.5;
                    var p_screen = new Point((int)(pen_info.X / scale) - this.pictureBox_Canvas.Left, (int)(pen_info.Y / scale) - this.pictureBox_Canvas.Top);
                    var p_client = this.PointToClient(p_screen);
                    if (p_client.X < 0) { return; }
                    if (p_client.Y < 0) { return; }

                    int max_brush_size = this.brush_size;


                    var brush_size = System.Math.Max(1, adjusted_pressure * max_brush_size);
                    var rect_size = new Size((int)brush_size, (int)brush_size);

                    p_client = new Point(p_client.X - (int)(brush_size / 2), p_client.Y - (int)(brush_size / 2));
                    var rect = new Rectangle(p_client, rect_size);

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
            this.brush_size = this.trackBar_BrushSize.Value;
            this.label_BrushSizeValue.Text = this.brush_size.ToString();
        }

        private void trackBarPressureCurve_Scroll(object sender, EventArgs e)
        {
            this.pressure_curve_q = ((double)this.trackBarPressureCurve.Value) / 100.0;
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
            this.brush_size = this.brush_size + value;
            this.brush_size = Math.Max(1, this.brush_size);
            this.brush_size = Math.Min(100, this.brush_size);

            this.label_BrushSizeValue.Text = this.brush_size.ToString();
            this.trackBar_BrushSize.Value= this.brush_size;
        }
        private void MenuFileSave_Click(object sender, EventArgs e)
        {
            if (this.filename!=null)
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
            else
            {
                AppSaveAs();

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
                try
                {
                    this.bitmap_doc.Save(this.filename);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to save " + this.filename);
                }
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
    }
}