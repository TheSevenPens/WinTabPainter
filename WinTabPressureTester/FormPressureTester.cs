using ScottPlot;
using System.Diagnostics;
using System.IO.Ports;
using System.Text;

namespace WinTabPressureTester
{
    public partial class FormPressureTester : Form
    {
        WinTabUtils.TabletSession wintabsession;

        WinTabUtils.Numerics.MovingAverage logical_pressure_moving_average;


        private System.IO.Ports.SerialPort serialPort;
        private CancellationTokenSource cts;
        private bool isReading = false;

        Stopwatch stopwatch;
        double physi_force;
        double log_force;

        PressureRecordCollection record_collection;


        int q_logical_bufsize = 400;
        WinTabUtils.Numerics.IndexedQueue<double> q_logical;

        public FormPressureTester()
        {
            InitializeComponent();
            this.wintabsession = new WinTabUtils.TabletSession();
            this.logical_pressure_moving_average = new WinTabUtils.Numerics.MovingAverage(200);

            formsPlot1.Plot.Axes.SetLimits(0, 1000, 0, 110);

            this.q_logical = new WinTabUtils.Numerics.IndexedQueue<double>(this.q_logical_bufsize);

            serialPort = new SerialPort("COM6");
            cts = new CancellationTokenSource();

            this.record_collection = new PressureRecordCollection();
            this.button_start.Select();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            this.wintabsession.PacketHandler = this.PacketHandler;
            this.wintabsession.ButtonChangedHandler = this.ButtonChangeHandler;
            this.wintabsession.Open(WinTabUtils.TabletContextType.System);

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.wintabsession?.Close();
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

            this.label_or_altitude.Text = (wintab_pkt.pkOrientation.orAltitude / 10.0).ToString() + "�";
            this.label_or_azimuth.Text = (wintab_pkt.pkOrientation.orAzimuth / 10.0).ToString() + "�";
            this.label_tiltx.Text = string.Format("{0:00.000}�", TiltX);
            this.label_tilty.Text = string.Format("{0:00.000}�", TiltY);



            this.logical_pressure_moving_average.AddSample(normalized_raw_pressure);
            double cur_logical_pressure_ma = logical_pressure_moving_average.GetAverage();
            this.label_normalizedpressure_ma.Text = string.Format("{0:00.00}%", cur_logical_pressure_ma * 100.0);

            if (this.q_logical.Count >= this.q_logical_bufsize)
            {
                if (this.q_logical.Count > 0)
                {
                    this.q_logical.Dequeue();
                }
            }
            this.q_logical.Enqueue(cur_logical_pressure_ma);

        }

        private void ButtonChangeHandler(WintabDN.Structs.WintabPacket wintab_pkt, WinTabUtils.PenButtonPressChange buttonchange)
        {
            if (buttonchange.Change == WinTabUtils.PenButtonPressChangeType.Released)
            {
                this.logical_pressure_moving_average.Clear();
            }
        }

        private async void button_start_Click(object sender, EventArgs e)
        {

            if (!isReading)
            {
                try
                {
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

                        // Update UI (e.g., append to a TextBox)
                        if (line == null) { continue; }
                        line = line.Trim();

                        if (line.Length == 0) { continue; }

                        var tokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                        if (tokens.Length == 0) { continue; }

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

        private void button_record_Click(object sender, EventArgs e)
        {
            this.record_collection.Add(physi_force, log_force);

            this.updatedata();



        }

        private void FormPressureTester_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void FormPressureTester_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string t = this.textBox_log.Text;

            var sb = new StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine("    \"brand\": \"XXXXX\" , ");
            sb.AppendLine("    \"pen\": \"XXXXX\" , ");
            sb.AppendLine("    \"penfamily\": \"\" , ");
            sb.AppendLine("    \"inventoryid\": \"XX000000\" , ");
            sb.AppendLine("    \"date\": \"2025-04-04\" , ");
            sb.AppendLine("    \"user\": \"sevenpens\" , ");
            sb.AppendLine("    \"tablet\": \"PXX-000\" , ");
            sb.AppendLine("    \"driver\": \"XXX\" , ");
            sb.AppendLine("    \"os\": \"WINDOWS\" , ");
            sb.AppendLine("    \"notes\": \"\" , ");
            sb.AppendLine("    \"records\": [  ");
            sb.AppendLine(t);
            sb.AppendLine("    ]");
            sb.AppendLine("}");
            System.Windows.Forms.Clipboard.SetText(sb.ToString());
        }

        private void button_clearlog_Click(object sender, EventArgs e)
        {
            this.record_collection.Clear();
            this.updatedata();
        }

        private void textBox_log_Enter(object sender, EventArgs e)
        {

        }

        private void textBox_log_TextChanged(object sender, EventArgs e)
        {

        }

        private void button_clearlast_Click(object sender, EventArgs e)
        {
            if (this.record_collection.Count < 1)
            {
                return;
            }

            this.record_collection.ClearLast();

            this.updatedata();

        }

        private void button_load_sample_data_Click(object sender, EventArgs e)
        {
            this.record_collection.Add(10, 0.01);
            this.record_collection.Add(100, 0.40);
            this.record_collection.Add(150, 0.50);
            this.record_collection.Add(400, 0.85);
            this.record_collection.Add(500, 1.00);
            this.updatedata();
        }

        public void updatedata()
        {
            this.label_recordcount.Text = this.record_collection.Count.ToString();
            this.textBox_log.Text = this.record_collection.GetText();

            textBox_log.SelectionStart = textBox_log.TextLength;
            textBox_log.ScrollToCaret();


            double[] dataX = this.record_collection.items.Select(i => i.PhysicalPressure).ToArray();
            double[] dataY = this.record_collection.items.Select(i => i.LogicalPressure * 100).ToArray();

            formsPlot1.Plot.Clear();
            formsPlot1.Plot.Add.Scatter(dataX, dataY);
            formsPlot1.Refresh();
        }
    }
}
