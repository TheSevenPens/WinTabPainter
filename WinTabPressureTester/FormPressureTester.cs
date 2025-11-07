using ScottPlot;
using System.Diagnostics;
using System.IO.Ports;
using System.Text;
using WinTabUtils;

namespace WinTabPressureTester
{

    public partial class FormPressureTester : Form
    {
        // Sessions to held with our two datasources
        // the tablet and the scale
        WinTabUtils.TabletSession wintabsession;
        ScaleSession scalesession;


        // move to scalesession
        private System.IO.Ports.SerialPort serial_port;

        // move to scalesession ???
        private CancellationTokenSource cts;
        private bool isReading = false;

        // get rid of this
        Stopwatch stopwatch;

        // store in a different place? ???
        double physi_pressure;
        double log_pressure;

        PressureRecordCollection record_collection;
        int q_logical_pressure_bufsize = 400;
        WinTabUtils.Numerics.IndexedQueue<double> q_logical;


        public void UpdateCharTitle()
        {
            formsPlot1.Plot.Title($"Pressure response {this.textBox_brand.Text} {this.textBox_Pen.Text} ({this.textBox_date.Text}) ");
            formsPlot1.Refresh();

        }
        public FormPressureTester()
        {
            InitializeComponent();
            this.wintabsession = new WinTabUtils.TabletSession();
            this.scalesession = new ScaleSession();


            this.textBox_date.Text = DateTime.Today.ToString("yyyy-MM-dd");
            this.textBox_User.Text = System.Environment.UserName.ToUpper().Trim();


            this.UpdateCharTitle();

            formsPlot1.Plot.XLabel("Physical pressure (gf)");
            formsPlot1.Plot.YLabel("Logical pressure (%)");

            formsPlot1.Plot.Axes.Title.Label.FontSize = 27;

            //formsPlot1.Plot.Axes.Left.Label.FontName = "Consolas";
            formsPlot1.Plot.Axes.Left.Label.FontSize = 27;
            //formsPlot1.Plot.Axes.Bottom.Label.FontName = "Segoe";
            formsPlot1.Plot.Axes.Bottom.Label.FontSize = 27;

            formsPlot1.Plot.Axes.SetLimits(0, 1000, 0, 110);
            formsPlot1.UserInputProcessor.IsEnabled = false; // prevent the user from scrolling or panning the plot

            formsPlot1.Plot.Grid.MajorLineColor = Colors.Black.WithOpacity(.1);
            formsPlot1.Plot.Grid.MajorLineWidth = 2;

            formsPlot1.Plot.Axes.Left.TickLabelStyle.FontSize = 27;
            formsPlot1.Plot.Axes.Bottom.TickLabelStyle.FontSize = 27;


            this.q_logical = new WinTabUtils.Numerics.IndexedQueue<double>(this.q_logical_pressure_bufsize);

            string comportname = GetSelectedComPortName();
            this.serial_port = new SerialPort(comportname);
            cts = new CancellationTokenSource();

            this.record_collection = new PressureRecordCollection();
            this.button_start.Select();
        }

        private string GetSelectedComPortName()
        {
            var x= SerialPort.GetPortNames();
            var lastitem = this.comboBoxcomport.Items[this.comboBoxcomport.Items.Count - 1];
            this.comboBoxcomport.Text = "COM4"; //lastitem.ToString();

            string comport = this.comboBoxcomport.Text.ToUpper();
            return comport;
        }

        Graphics gfx_picbox1;
        Pen np_pressure_guage = new Pen(System.Drawing.Color.Black, 11);

        private void Form1_Load(object sender, EventArgs e)
        {
            this.wintabsession.PacketHandler = this.PacketHandler;
            this.wintabsession.ButtonChangedHandler = this.ButtonChangeHandler;
            this.wintabsession.Open(WinTabUtils.TabletContextType.System);

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.wintabsession?.Close();
            this.np_pressure_guage?.Dispose();
            this.pictureBox1.Image?.Dispose();
            this.pictureBox1?.Dispose();
        }

        private static bool get_press_change_as_letter(PenButtonPressChangeType change)
        {
            return change switch
            {
                WinTabUtils.PenButtonPressChangeType.Pressed => true,
                WinTabUtils.PenButtonPressChangeType.Released => false,
                _ => throw new System.ArgumentOutOfRangeException()
            };
        }
        private void PacketHandler(WintabDN.Structs.WintabPacket wintab_pkt)
        {

            var button_info = new WinTabUtils.PenButtonPressChange(wintab_pkt.pkButtons);
            if (button_info.Change != WinTabUtils.PenButtonPressChangeType.NoChange)
            {
                if (button_info.ButtonId == WinTabUtils.PenButtonIdentifier.Tip)
                {
                    this.checkBox_tipdown.Checked = get_press_change_as_letter(button_info.Change);
                }
                else if (button_info.ButtonId == WinTabUtils.PenButtonIdentifier.LowerButton)
                {
                    this.checkBox_lowerbuttondown.Checked = get_press_change_as_letter(button_info.Change);
                }
                else if (button_info.ButtonId == WinTabUtils.PenButtonIdentifier.UpperButton)
                {
                    this.checkBox_upperbuttondown.Checked = get_press_change_as_letter(button_info.Change);
                }
                else
                {
                    // do Nothing
                }
            }


            uint raw_pressure = wintab_pkt.pkNormalPressure;
            double normalized_raw_pressure = (raw_pressure / (1.0 * this.wintabsession.TabletInfo.MaxPressure));

            string str_pressure = string.Format("{0:00.000}%", normalized_raw_pressure * 100.0);
            this.log_pressure = normalized_raw_pressure;

            (double TiltX, double TiltY) = WinTabUtils.Trigonometry.Angles.AzimuthAndAltudeToTiltDeg(wintab_pkt.pkOrientation.orAzimuth / 10.0, wintab_pkt.pkOrientation.orAltitude / 10.0);

            this.label_pressure_raw.Text = wintab_pkt.pkNormalPressure.ToString();
            this.label_normalized_pressure.Text = str_pressure.ToString();

            this.label_or_altitude.Text = (wintab_pkt.pkOrientation.orAltitude / 10.0).ToString() + "°";
            this.label_or_azimuth.Text = (wintab_pkt.pkOrientation.orAzimuth / 10.0).ToString() + "°";
            this.label_tiltx.Text = string.Format("{0:00.000}°", TiltX);
            this.label_tilty.Text = string.Format("{0:00.000}°", TiltY);



            this.scalesession.logical_pressure_moving_average.AddSample(normalized_raw_pressure);
            double cur_logical_pressure_ma = this.scalesession.logical_pressure_moving_average.GetAverage();
            this.label_normalizedpressure_ma.Text = string.Format("{0:00.00}%", cur_logical_pressure_ma * 100.0);

            if (this.q_logical.Count >= this.q_logical_pressure_bufsize)
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
                this.scalesession.logical_pressure_moving_average.Clear();
            }
        }

        private async void button_start_Click(object sender, EventArgs e)
        {

            if (!isReading)
            {
                try
                {
                    this.stopwatch = Stopwatch.StartNew();

                    if (!serial_port.IsOpen)
                    {
                        serial_port.Open();
                    }

                    isReading = true;
                    await ReadSerialPortAsync(cts.Token);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"ERROR Failed to open COM Port\r\n" +
                        $"{ex.GetType().FullName}\r\n" +
                    $"{ex.Message}");
                }
                finally
                {
                    if (serial_port.IsOpen)
                    {
                        serial_port.Close();
                    }
                    isReading = false;
                }
            }
        }

        public static string TrimLastCharIf(string s, char c)
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

        public class ScaleParsedLine
        {
            public string Input;
            public bool Parsed;
            public ScaleRecord ScaleRecord;
            public string Error;

        }

        public static ScaleParsedLine ParseScaleLine(string line)
        {
            if (line == null)
            {
                var r1 = new ScaleParsedLine();
                r1.Input = line;
                r1.Parsed = false;
                r1.ScaleRecord = null;
                r1.Error = "Line was null";
                return r1;
            }

            line = line.Trim();

            if (line.Length == 0)
            {
                var r2 = new ScaleParsedLine();
                r2.Input = line;
                r2.Parsed = false;
                r2.ScaleRecord = null;
                r2.Error = "Line was empty";

                return r2;
            }

            var tokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (tokens.Length == 0)
            {
                var r3 = new ScaleParsedLine();
                r3.Input = line;
                r3.Parsed = false;
                r3.ScaleRecord = null;
                r3.Error = "No tokens in line";
                return r3;
            }

            string str_force = tokens[^1];
            str_force = TrimLastCharIf(str_force, 'M');
            str_force = TrimLastCharIf(str_force, 'g');

            //physi_pressure = double.Parse(str_force);


            var sr = new ScaleRecord();


            sr.Line = line;
            sr.ReadingAsString = str_force;


            try
            {
                sr.ReadingAsDouble = double.Parse(str_force);
            }
            catch (Exception ex)
            {
                var r4 = new ScaleParsedLine();
                r4.Input = line;
                r4.Parsed = false;
                r4.ScaleRecord = null;
                r4.Error = "Failed to parse force \"" + str_force + "\"";
                return r4;
            }


            var r = new ScaleParsedLine();
            r.Input = line;
            r.Parsed = true;
            r.ScaleRecord = sr;
            r.Error = string.Empty;
            return r;
        }

        private async Task ReadSerialPortAsync(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    if (serial_port.BytesToRead > 0)
                    {
                        string line = await Task.Run(() => serial_port.ReadLine());
                        var sr_parse = ParseScaleLine(line);

                        if (sr_parse.Parsed == false)
                        {
                            // line was not parsed

                        }
                        else
                        {
                            // line was parsed
                            var sr = sr_parse.ScaleRecord;
                            if (sr != null)
                            {
                                physi_pressure = sr.ReadingAsDouble;
                                Update_Scale_UI_Elements(sr.ReadingAsString);
                            }
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

        private void Update_Scale_UI_Elements(string str_force)
        {
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
                string str_force_with_unit = str_force + " gf";
                this.label_force.Text = str_force_with_unit;
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
            if (serial_port != null && serial_port.IsOpen)
            {
                serial_port.Close();
                serial_port.Dispose();
            }

        }

        private void button_record_Click(object sender, EventArgs e)
        {
            this.record_collection.Add(physi_pressure, this.scalesession.logical_pressure_moving_average.GetAverage());

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
            string text_content = CreateJSONContent();
            System.Windows.Forms.Clipboard.SetText(text_content);
        }

        private string CreateJSONContent()
        {

            var sb = new StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine($@"    ""brand"": ""{textBox_brand.Text.Trim().ToUpper()}"" , ");
            sb.AppendLine($@"    ""pen"": ""{textBox_Pen.Text.Trim().ToUpper()}"" , ");
            sb.AppendLine($@"    ""penfamily"": ""{string.Empty.Trim().ToUpper()}"" , ");
            sb.AppendLine($@"    ""inventoryid"": ""{textBox_inventoryid.Text.Trim().ToUpper()}"" , ");
            sb.AppendLine($@"    ""date"": ""{this.textBox_date.Text.Trim().ToUpper()}"" , ");
            sb.AppendLine($@"    ""user"": ""{this.textBox_User.Text.Trim().ToUpper()}"" , ");
            sb.AppendLine($@"    ""tablet"": ""{this.textBox_Tablet.Text.Trim().ToUpper()}"" , ");
            sb.AppendLine($@"    ""driver"": ""{this.textBox_driver.Text.Trim().ToUpper()}"" , ");
            sb.AppendLine($@"    ""os"": ""{this.textBox_OS.Text.Trim().ToUpper()}"" , ");
            sb.AppendLine($@"    ""notes"": ""{string.Empty}"" , ");
            sb.AppendLine("    \"records\": [  ");
            sb.AppendLine(this.record_collection.GetRecordsJSON());
            sb.AppendLine("    ]");
            sb.AppendLine("}");

            string text_content = sb.ToString();
            return text_content;
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
            this.textBox_log.Text = this.record_collection.GetRecordsText();

            textBox_log.SelectionStart = textBox_log.TextLength;
            textBox_log.ScrollToCaret();


            double[] dataX = this.record_collection.items.Select(i => i.PhysicalPressure).ToArray();
            double[] dataY = this.record_collection.items.Select(i => i.LogicalPressure * 100).ToArray();

            formsPlot1.Plot.Clear();
            var scatter = formsPlot1.Plot.Add.Scatter(dataX, dataY);
            scatter.LineWidth = 3;
            formsPlot1.Refresh();
        }

        private void button_export_Click(object sender, EventArgs e)
        {
            string json = this.CreateJSONContent();
            string datestring = this.textBox_date.Text.Trim().ToUpper();
            string inventoryid = this.textBox_inventoryid.Text.Trim().ToUpper();
            string filename = $"{inventoryid}_{datestring}.json";



            string myDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string filePath = Path.Combine(myDocumentsPath, filename);

            try
            {
                File.WriteAllText(filePath, json);
                MessageBox.Show("File saved " + filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving file: {ex.Message}");
            }
        }

        private void textBox_brand_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox_brand_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void textBox_brand_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.UpdateCharTitle();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // not implemented yet

        }

        private void textBox_inventoryid_TextChanged(object sender, EventArgs e)
        {

        }

        private void formsPlot1_Load(object sender, EventArgs e)
        {

        }
    }
}
