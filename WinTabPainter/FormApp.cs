﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WintabDN;



// References:
// https://github.com/DennisWacom/WintabControl/tree/master/WintabControl
// https://github.com/DennisWacom/InkPlatform/tree/master/WintabDN
// https://developer-docs.wacom.com/docs/icbt/windows/wintab/wintab-basics/
// https://www.nuget.org/packages/WacomSolutionPartner.WintabDotNet
// https://github.com/Wacom-Developer/wacom-device-kit-windows/tree/master/Wintab%20TiltTest

namespace DemoWinTabPaint1
{
    public partial class FormApp : Form
    {

        private CWintabContext wintab_context = null;
        private CWintabData wintab_data = null;

        private Graphics bitmap_gfx;
        private Bitmap bitmap_canvas;

        PenInfo pen_info;

        double pressure_curve_q = 0.0;
        int brush_size = 5;
        TabletInfo tablet_info = new TabletInfo();

        public FormApp()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // All actual drawing will be done to this bitmap
            this.bitmap_canvas = new Bitmap(1000, 1000);

            // Create a graphics object for the canvas bitmap and enable smoothing
            // by default for better looking stroke edges

            this.bitmap_gfx = System.Drawing.Graphics.FromImage(this.bitmap_canvas);
            this.bitmap_gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Double-buffering to reduce flicket
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            // Link the bitmap canvas to the picturebox
            // changes to the bitmap will be rendered
            // whenever the picturebox is redrawn

            this.pictureBox_Canvas.Image = this.bitmap_canvas;
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


            if (this.bitmap_gfx != null)
            {
                this.bitmap_gfx.Dispose();
            }

            if (this.bitmap_canvas != null)
            {
                this.bitmap_canvas.Dispose();
            }

        }

        private CWintabContext OpenTabletContext()
        {
            var context = CWintabInfo.GetDefaultSystemContext(ECTXOptionValues.CXO_MESSAGES);

            if (context == null)
            {
                System.Windows.Forms.MessageBox.Show("Failed to get digitizing context");
            }

            context.Name = "Digitizer Context";
            context.Options |= (uint)ECTXOptionValues.CXO_SYSTEM;

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
            this.wintab_data = new CWintabData(context);
            this.wintab_data.SetWTPacketEventHandler(WinTabPacketHandler);

            return context;
        }

        private void WinTabPacketHandler(Object sender, MessageReceivedEventArgs args)
        {

            uint pktId = (uint)args.Message.WParam;
            var wintab_pkt = this.wintab_data.GetDataPacket((uint)args.Message.LParam, pktId);

            if (wintab_pkt.pkContext == wintab_context.HCtx)
            {
                this.pen_info = new PenInfo();
                this.pen_info.X = wintab_pkt.pkX;
                this.pen_info.Y = wintab_pkt.pkY;
                this.pen_info.Z = wintab_pkt.pkZ;
                this.pen_info.Pressure = wintab_pkt.pkNormalPressure;
                this.pen_info.PressureNormalized = pen_info.Pressure / (double)this.tablet_info.MaxPressure;
                this.pen_info.Altitude = wintab_pkt.pkOrientation.orAltitude;
                this.pen_info.Azimuth = wintab_pkt.pkOrientation.orAzimuth;

                this.label_PosXValue.Text = this.pen_info.X.ToString();
                this.label_PosYValue.Text = this.pen_info.Y.ToString();
                this.label_PosZValue.Text = this.pen_info.Z.ToString();
                this.label_PressureRawValue.Text = this.pen_info.Pressure.ToString();
                this.label_PressureValue.Text = this.pen_info.PressureNormalized.ToString();

                this.label_AltitudeValue.Text = this.pen_info.Altitude.ToString();
                this.label_AzimuthValue.Text = this.pen_info.Azimuth.ToString();

                if (wintab_pkt.pkNormalPressure > 0)
                {

                    using (Brush brush = new SolidBrush(Color.Black))
                    {
                        double scale = 2.5;
                        var p_screen = new Point((int)(pen_info.X / scale) - this.pictureBox_Canvas.Left, (int)(pen_info.Y / scale) - this.pictureBox_Canvas.Top);
                        var p_client = this.PointToClient(p_screen);
                        if (p_client.X < 0) { return; }
                        if (p_client.Y < 0) { return; }

                        int max_brush_size = this.brush_size;

                        double adjusted_pressure = ApplyCurve(pen_info.PressureNormalized, this.pressure_curve_q);

                        var brush_size = System.Math.Max(1, adjusted_pressure * max_brush_size);
                        var rect_size = new Size((int)brush_size, (int)brush_size);

                        p_client = new Point(p_client.X - (int)(brush_size / 2), p_client.Y - (int)(brush_size / 2));
                        var rect = new Rectangle(p_client, rect_size);

                        this.bitmap_gfx.FillEllipse(brush, rect);

                    }

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
            using (var b = new SolidBrush(System.Drawing.Color.White))
            {
                this.bitmap_gfx.FillRectangle(b, 0, 0, this.bitmap_canvas.Width, this.bitmap_canvas.Height);
                this.pictureBox_Canvas.Invalidate();
            }
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

    }
}
