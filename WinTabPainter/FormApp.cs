using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WintabDN;

namespace DemoWinTabPaint1
{

    public partial class FormApp : Form
    {

        private CWintabContext wintab_context = null;
        private CWintabData wintab_data = null;

        TabletInfo tablet_info = new TabletInfo();

        public FormApp()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
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
                this.textBox_PositionX.Text = wintab_pkt.pkX.ToString();
                this.textBox_PositionY.Text = wintab_pkt.pkY.ToString();
                this.textBox_PositionZ.Text = wintab_pkt.pkZ.ToString();



                double normalized_pressure = wintab_pkt.pkNormalPressure / (double) this.tablet_info.MaxPressure;
                this.textBox_PressureNormal.Text = normalized_pressure.ToString();

                this.textBox_OrientationAltitude.Text = wintab_pkt.pkOrientation.orAltitude.ToString();
                this.textBox_OrientationAzimuth.Text = wintab_pkt.pkOrientation.orAzimuth.ToString();

                this.textBox_Buttons.Text = wintab_pkt.pkCursor.ToString();
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
    }
}
