namespace WinTabPressureTester
{
    public partial class FormPressureTester : Form
    {
        WinTabUtils.TabletSession wintabsession;
        WinTabUtils.Numerics.MovingAverage ma;
        public FormPressureTester()
        {
            InitializeComponent();
            this.wintabsession = new WinTabUtils.TabletSession();
            this.ma = new WinTabUtils.Numerics.MovingAverage(600);
        }

        Graphics gfx_picbox1;
        Pen np_pressure_guage = new Pen(Color.Black, 11);

        private void Form1_Load(object sender, EventArgs e)
        {
            this.wintabsession.PacketHandler = this.PacketHandler;
            this.wintabsession.ButtonChangedHandler = this.ButtonChangeHandler;
            this.wintabsession.Open(WinTabUtils.TabletContextType.System);
            this.pictureBox1.Image = new System.Drawing.Bitmap(this.pictureBox1.Width, this.pictureBox1.Height);
            this.gfx_picbox1 = System.Drawing.Graphics.FromImage(this.pictureBox1.Image);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.wintabsession?.Close();
            this.np_pressure_guage?.Dispose();
            this.pictureBox1.Image?.Dispose();
            this.pictureBox1?.Dispose();
        }

        private void PacketHandler(WintabDN.Structs.WintabPacket wintab_pkt)
        {
            // var button_info = new WinTabUtils.PenButtonPressChange(wintab_pkt.pkButtons);

            uint raw_pressure = wintab_pkt.pkNormalPressure;
            double normalized_raw_pressure = (raw_pressure / (1.0 * this.wintabsession.TabletInfo.MaxPressure));

            string str_pressure = string.Format("{0:00.000}%", normalized_raw_pressure * 100.0);

            (double TiltX, double TiltY) = WinTabUtils.Trigonometry.Angles.AzimuthAndAltudeToTiltDeg(wintab_pkt.pkOrientation.orAzimuth / 10.0, wintab_pkt.pkOrientation.orAltitude / 10.0);

            this.label_pressure_raw.Text = wintab_pkt.pkNormalPressure.ToString();
            this.label_normalized_pressure.Text = str_pressure.ToString();

            this.label_or_altitude.Text = (wintab_pkt.pkOrientation.orAltitude / 10.0).ToString() + "�";
            this.label_or_azimuth.Text = (wintab_pkt.pkOrientation.orAzimuth / 10.0).ToString() + "�";
            this.label_tiltx.Text = string.Format("{0:00.000}�", TiltX);
            this.label_tilty.Text = string.Format("{0:00.000}�", TiltY);



            this.ma.AddSample(normalized_raw_pressure);
            this.label_normalizedpressure_ma.Text = string.Format("{0:00.00}%", ma.GetAverage() * 100.0);

        }

        private void ButtonChangeHandler(WintabDN.Structs.WintabPacket wintab_pkt, WinTabUtils.PenButtonPressChange buttonchange)
        {
            if (buttonchange.Change == WinTabUtils.PenButtonPressChangeType.Released)
            {
                this.ma.Clear();
            }
        }
    }
}
