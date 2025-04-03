using System.Diagnostics;
using System.IO.Ports;

namespace WinTabPressureTester
{
    public partial class FormPressureTester : Form
    {
        WinTabUtils.TabletSession wintabsession;
        WinTabUtils.Numerics.MovingAverage ma;


        private System.IO.Ports.SerialPort serialPort;
        private CancellationTokenSource cts;
        private bool isReading = false;

        int num_measuremnts;
        Stopwatch stopwatch;
        double physi_force;
        double log_force;

        int px = 0;

        public FormPressureTester()
        {
            InitializeComponent();
            this.wintabsession = new WinTabUtils.TabletSession();
            this.ma = new WinTabUtils.Numerics.MovingAverage(600);

            serialPort = new SerialPort("COM6");
            cts = new CancellationTokenSource();
        }

        Graphics gfx_picbox1;
        Pen np_pen_black = new Pen(Color.Black, 11);
        Pen np_pen_red = new Pen(Color.Red, 11);
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
            this.np_pen_black?.Dispose();
            this.pictureBox1.Image?.Dispose();
            this.pictureBox1?.Dispose();
        }

        private void PacketHandler(WintabDN.Structs.WintabPacket wintab_pkt)
        {
            // var button_info = new WinTabUtils.PenButtonPressChange(wintab_pkt.pkButtons);

            uint raw_pressure = wintab_pkt.pkNormalPressure;
            double normalized_raw_pressure = (raw_pressure / (1.0 * this.wintabsession.TabletInfo.MaxPressure));

            string str_pressure = string.Format("{0:00.000}%", normalized_raw_pressure * 100.0);
            this.log_force = normalized_raw_pressure;

            (double TiltX, double TiltY) = WinTabUtils.Trigonometry.Angles.AzimuthAndAltudeToTiltDeg(wintab_pkt.pkOrientation.orAzimuth / 10.0, wintab_pkt.pkOrientation.orAltitude / 10.0);

            this.label_pressure_raw.Text = wintab_pkt.pkNormalPressure.ToString();
            this.label_normalized_pressure.Text = str_pressure.ToString();

            this.label_or_altitude.Text = (wintab_pkt.pkOrientation.orAltitude / 10.0).ToString() + "°";
            this.label_or_azimuth.Text = (wintab_pkt.pkOrientation.orAzimuth / 10.0).ToString() + "°";
            this.label_tiltx.Text = string.Format("{0:00.000}°", TiltX);
            this.label_tilty.Text = string.Format("{0:00.000}°", TiltY);



            this.ma.AddSample(normalized_raw_pressure);
            this.label_normalizedpressure_ma.Text = string.Format("{0:00.00}%", ma.GetAverage() * 100.0);

            int py = this.pictureBox1.Height - 10 - (int)(4 * 100 * normalized_raw_pressure);
            this.gfx_picbox1.DrawLine(this.np_pen_black,
                new WinTabUtils.Geometry.Point(px, py),
                new WinTabUtils.Geometry.Point(px, py + 1));

            this.px = this.px + 1;
            if (this.px > this.pictureBox1.Width)
            {
                this.px = 0;
            }

            this.pictureBox1.Invalidate();

        }

        private void ButtonChangeHandler(WintabDN.Structs.WintabPacket wintab_pkt, WinTabUtils.PenButtonPressChange buttonchange)
        {
            if (buttonchange.Change == WinTabUtils.PenButtonPressChangeType.Released)
            {
                this.ma.Clear();
            }
        }

        private async void button_start_Click(object sender, EventArgs e)
        {
            if (!isReading)
            {
                try
                {
                    num_measuremnts = 0;
                    this.stopwatch = Stopwatch.StartNew();

                    if (!serialPort.IsOpen)
                    {
                        serialPort.Open();
                    }

                    isReading = true;
                    await ReadSerialPortAsync(cts.Token);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
                finally
                {
                    if (serialPort.IsOpen)
                    {
                        serialPort.Close();
                    }
                    isReading = false;
                }
            }
        }

        public string TrimLastCharIf(string s, char c)
        {
            if (s == null) { return s; }
            if (s.Length == 0) { return s; }

            char lastchar = s[^1];
            if (c == lastchar)
            {
                return s.Substring(0, s.Length - 1);
            }
            else
            {
                return s;
            }

        }
        private async Task ReadSerialPortAsync(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    if (serialPort.BytesToRead > 0)
                    {
                        string line = await Task.Run(() => serialPort.ReadLine());
                        this.num_measuremnts++;

                        // Update UI (e.g., append to a TextBox)
                        if (line == null) { continue; }
                        line = line.Trim();

                        if (line.Length == 0) { continue; }

                        var tokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                        if (tokens.Length == 0) { continue; }

                        string str_nm = this.num_measuremnts.ToString();
                        string str_mps = (this.num_measuremnts / this.stopwatch.Elapsed.TotalSeconds).ToString();
                        string str_force = tokens[^1];
                        str_force = this.TrimLastCharIf(str_force, 'M');
                        str_force = this.TrimLastCharIf(str_force, 'g');

                         physi_force = double.Parse(str_force);

                        str_force += " gf";

                        if (InvokeRequired)
                        {
                            //Invoke(new Action(() => this.textBox_Log.AppendText(line + Environment.NewLine)));
                            //Invoke(new Action(() => this.label_mps.Text = str_mps));
                            //Invoke(new Action(() => this.label_num_measuremnts.Text = str_force));
                        }
                        else
                        {
                            //this.textBox_Log.AppendText(line + Environment.NewLine);
                            //this.label_mps.Text = str_mps;
                            //this.label_num_measuremnts.Text = str_nm;
                            this.label_force.Text = str_force;
                        }

                    }
                    // Small delay to prevent CPU overuse
                    await Task.Delay(10, cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
                // Cancellation was requested
                this.textBox_log.AppendText("Reading cancelled" + Environment.NewLine);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Serial port error: {ex.Message}");
            }
        }

        private void button_stop_Click(object sender, EventArgs e)
        {
            // Cancel the reading operation
            cts.Cancel();
            // Create new CancellationTokenSource for next operation
            cts = new CancellationTokenSource();
        }

        private void FormPressureTester_FormClosing(object sender, FormClosingEventArgs e)
        {

            cts.Cancel();
            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.Close();
                serialPort.Dispose();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string phys = this.physi_force.ToString();
            string log = this.log_force.ToString();
            this.textBox_log.AppendText("[ " + phys + " , " + log  + " ], \r\n");
        }

        private void FormPressureTester_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void FormPressureTester_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
    }
}
