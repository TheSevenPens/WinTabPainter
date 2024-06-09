using System;
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

        private Graphics canvas_gfx;
        private Graphics bitmap_gfx;
        private Bitmap bitmap;


        PenInfo pen_info;

        TabletInfo tablet_info = new TabletInfo();

        public FormApp()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.canvas_gfx = this.panel_Canvas.CreateGraphics();
            this.bitmap = new Bitmap(1000, 1000);
            this.bitmap_gfx = System.Drawing.Graphics.FromImage(this.bitmap);
            this.wintab_context = OpenQueryDigitizerContext();

            // bring window to first display

            if (System.Windows.Forms.SystemInformation.MonitorCount > 1)
            {
                var screen = System.Windows.Forms.Screen.AllScreens[0];
                this.Left = screen.Bounds.Left + (screen.Bounds.Width / 2) - (this.Width / 2);
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.CloseCurrentContext();

            if (this.canvas_gfx != null)
            {
                this.canvas_gfx.Dispose();
            }


            if (this.bitmap_gfx != null)
            {
                this.bitmap_gfx.Dispose();
            }

            if (this.bitmap != null)
            {
                this.bitmap.Dispose();
            }

        }

        private CWintabContext OpenQueryDigitizerContext()
        {
            //var context = CWintabInfo.GetDefaultDigitizingContext(ECTXOptionValues.CXO_MESSAGES);
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
                this.textBox_Device.Text = this.tablet_info.Device;
            }
            else
            {
                this.textBox_Device.Text = "UNKNOWN";
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

                this.textBox_PositionX.Text = this.pen_info.X.ToString();
                this.textBox_PositionY.Text = this.pen_info.Y.ToString();
                this.textBox_PositionZ.Text = this.pen_info.Z.ToString();
                this.textBox_PressureNormal.Text = this.pen_info.PressureNormalized.ToString();

                this.textBox_OrientationAltitude.Text = this.pen_info.Altitude.ToString();
                this.textBox_OrientationAzimuth.Text = this.pen_info.Azimuth.ToString();

                if (wintab_pkt.pkNormalPressure > 0)
                {
                    this.panel_Canvas.Invalidate();
                }
            }
        }

        private void CloseCurrentContext()
        {
            if (this.wintab_context != null)
            {
                this.wintab_context.Close();
                this.wintab_context = null;
                this.wintab_data.ClearWTPacketEventHandler();
                this.wintab_data = null;
            }
        }

        private void panel_Canvas_Paint(object sender, PaintEventArgs e)
        {
            if (this.panel_Canvas.IsDisposed) return;
            
            using (Pen pen = new Pen(Color.Blue, 4))
            using (Brush brush = new SolidBrush(Color.Yellow))
            {
                double scale = 2.5;
                var p_screen = new Point((int)(pen_info.X/ scale) - this.panel_Canvas.Left, (int) (pen_info.Y/ scale) - this.panel_Canvas.Top);
                var p_client = this.PointToClient(p_screen);
                if (p_client.X <0) { return; }
                if (p_client.Y <0) { return; }

                int max_brush_size = 40;
                var brush_size = System.Math.Max(1, pen_info.PressureNormalized * max_brush_size);
                var rect_size = new Size((int)brush_size, (int)brush_size);
                var rect = new Rectangle(p_client, rect_size);

                this.bitmap_gfx.DrawEllipse(pen, rect);
                this.canvas_gfx.DrawImage(this.bitmap, new Point(0, 0));
                //this.canvas_gfx.DrawEllipse(pen, rect);
            }

        }
    }
}
