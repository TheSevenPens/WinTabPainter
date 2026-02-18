using System;
using System.Text;
using System.Windows.Forms;

namespace WinTabPainter
{
    public partial class FormTablet : Form
    {
        public FormTablet()
        {
            InitializeComponent();
        }

        public SevenLib.WinTab.WinTabDeviceInfo tablet_info;

        private void FormTablet_Load(object sender, EventArgs e)
        {

            StringBuilder sb = new StringBuilder();
            sb.AppendFormatLine("Tablet name: {0}", this.tablet_info.Name);

            sb.AppendFormatLine("X Axis Min: {0}", this.tablet_info.XAxis.axMin);
            sb.AppendFormatLine("X Axis Max: {0}", this.tablet_info.XAxis.axMax);
            sb.AppendFormatLine("X Axis Resolution: {0}", this.tablet_info.XAxis.axResolution);
            sb.AppendFormatLine("X Axis Units: {0}", this.tablet_info.XAxis.axUnits);

            sb.AppendFormatLine("Y Axis Min: {0}", this.tablet_info.YAxis.axMin);
            sb.AppendFormatLine("Y Axis Max: {0}", this.tablet_info.YAxis.axMax);
            sb.AppendFormatLine("Y Axis Resolution: {0}", this.tablet_info.YAxis.axResolution);
            sb.AppendFormatLine("Y Axis Units: {0}", this.tablet_info.YAxis.axUnits);

            sb.AppendFormatLine("Max Pressure: {0}", this.tablet_info.MaxPressure);
            sb.AppendFormatLine("Supports tilt: {0}", this.tablet_info.TiltSupport);

            this.textBox1.Text = sb.ToString();

        }

        private void button_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
