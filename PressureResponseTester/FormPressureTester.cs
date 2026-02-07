using ScottPlot;
using System.Diagnostics;
using System.IO.Ports;
using System.Text;
using SevenUtils;

namespace WinTabPressureTester
{
    public partial class FormPressureTester : Form
    {
        AppState appstate = new AppState();



        public void UpdateCharTitle()
        {
            formsPlot1.Plot.Title($"Pressure response {this.textBox_brand.Text} {this.textBox_Pen.Text} ({this.textBox_date.Text}) ");
            formsPlot1.Refresh();

        }
        public FormPressureTester()
        {
            InitializeComponent();
            this.appstate.wintab_session = new WinTabDN.Utils.TabletSession();
            this.appstate.scale_session = new ScaleSession();


            this.textBox_date.Text = DateTime.Today.ToString("yyyy-MM-dd");
            this.textBox_User.Text = System.Environment.UserName.ToUpper().Trim();

            if (this.comboBoxcomport.Items!=null)
            {
                var portnames = SerialPort.GetPortNames();
                this.comboBoxcomport.Items.AddRange(portnames);
            }

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


            this.appstate.queue_logical = new SevenUtils.Numerics.IndexedQueue<double>(this.appstate.logical_pressure_queue_size);

            string comportname = GetSelectedComPortName();
            if (!string.IsNullOrEmpty(comportname))
            {
               this.appstate.serial_port = new SerialPort(comportname);
            }
            else
            {
               this.appstate.serial_port = null;
            }
            this.appstate.scale_cts = new CancellationTokenSource();

            this.appstate.record_collection = new PressureRecordCollection();
            this.button_start.Select();
        }

        private string GetSelectedComPortName()
        {
            var portnames= SerialPort.GetPortNames();
            if (portnames == null || portnames.Length == 0)
            {
                return null;
            }
            
            if (this.comboBoxcomport.Items.Count > 0)
            {
                var lastitem = this.comboBoxcomport.Items[this.comboBoxcomport.Items.Count - 1];
                this.comboBoxcomport.Text = lastitem.ToString();
                return this.comboBoxcomport.Text.ToUpper();
            }
            
            return null;
        }

        Graphics gfx_picbox1;
        Pen np_pressure_guage = new Pen(System.Drawing.Color.Black, 11);

        private void Form1_Load(object sender, EventArgs e)
        {
            StartWinTabSession();
        }



        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            StopScaleSession();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }

                // WinTab Session
                this.appstate.wintab_session?.Dispose();
                
                // Scale Session Resources
                this.appstate.scale_cts?.Cancel();
                this.appstate.scale_cts?.Dispose();
                
                if (this.appstate.serial_port != null)
                {
                   if (this.appstate.serial_port.IsOpen) this.appstate.serial_port.Close();
                   this.appstate.serial_port.Dispose();
                }

                // GDI+ Resources
                this.np_pressure_guage?.Dispose();
                this.pictureBox1.Image?.Dispose();
                this.pictureBox1?.Dispose();
                this.gfx_picbox1?.Dispose();
            }
            base.Dispose(disposing);
        }



        private static bool get_press_change_as_letter(WinTabDN.Utils.PenButtonPressChangeType change)
        {
            return change switch
            {
                WinTabDN.Utils.PenButtonPressChangeType.Pressed => true,
                WinTabDN.Utils.PenButtonPressChangeType.Released => false,
                _ => throw new System.ArgumentOutOfRangeException()
            };
        }
        private void PacketHandler(WinTabDN.Structs.WintabPacket wintab_pkt)
        {

            var button_info = new WinTabDN.Utils.PenButtonPressChange(wintab_pkt.pkButtons);
            if (button_info.Change != WinTabDN.Utils.PenButtonPressChangeType.NoChange)
            {
                if (button_info.ButtonId == WinTabDN.Utils.PenButtonIdentifier.Tip)
                {
                    this.checkBox_tipdown.Checked = get_press_change_as_letter(button_info.Change);
                }
                else if (button_info.ButtonId == WinTabDN.Utils.PenButtonIdentifier.LowerButton)
                {
                    this.checkBox_lowerbuttondown.Checked = get_press_change_as_letter(button_info.Change);
                }
                else if (button_info.ButtonId == WinTabDN.Utils.PenButtonIdentifier.UpperButton)
                {
                    this.checkBox_upperbuttondown.Checked = get_press_change_as_letter(button_info.Change);
                }
                else
                {
                    // do Nothing
                }
            }


            uint raw_pressure = wintab_pkt.pkNormalPressure;
            double normalized_raw_pressure = (raw_pressure / (1.0 * this.appstate.wintab_session.TabletInfo.MaxPressure));

            string str_pressure = string.Format("{0:00.000}%", normalized_raw_pressure * 100.0);
            this.appstate.log_pressure = normalized_raw_pressure;

            (double TiltX, double TiltY) = SevenUtils.Trigonometry.Angles.AzimuthAndAltudeToTiltDeg(wintab_pkt.pkOrientation.orAzimuth / 10.0, wintab_pkt.pkOrientation.orAltitude / 10.0);

            this.label_pressure_raw.Text = wintab_pkt.pkNormalPressure.ToString();
            this.label_normalized_pressure.Text = str_pressure.ToString();

            this.label_or_altitude.Text = (wintab_pkt.pkOrientation.orAltitude / 10.0).ToString() + "°";
            this.label_or_azimuth.Text = (wintab_pkt.pkOrientation.orAzimuth / 10.0).ToString() + "°";
            this.label_tiltx.Text = string.Format("{0:00.000}°", TiltX);
            this.label_tilty.Text = string.Format("{0:00.000}°", TiltY);



            this.appstate.scale_session.logical_pressure_moving_average.AddSample(normalized_raw_pressure);
            double cur_logical_pressure_ma = this.appstate.scale_session.logical_pressure_moving_average.GetAverage();
            this.label_normalizedpressure_ma.Text = string.Format("{0:00.00}%", cur_logical_pressure_ma * 100.0);

            if (this.appstate.queue_logical.Count >= this.appstate.logical_pressure_queue_size)
            {
                if (this.appstate.queue_logical.Count > 0)
                {
                    this.appstate.queue_logical.Dequeue();
                }
            }
            this.appstate.queue_logical.Enqueue(cur_logical_pressure_ma);

        }

        private void ButtonChangeHandler(WinTabDN.Structs.WintabPacket wintab_pkt, WinTabDN.Utils.PenButtonPressChange buttonchange)
        {
            if (buttonchange.Change == WinTabDN.Utils.PenButtonPressChangeType.Released)
            {
                this.appstate.scale_session.logical_pressure_moving_average.Clear();
            }
        }

        private async void button_start_Click(object sender, EventArgs e)
        {

            if (!appstate.scale_isReading)
            {
                await StartScaleSession();
            }
        }

        private void StartWinTabSession()
        {
            this.appstate.wintab_session.PacketHandler = this.PacketHandler;
            this.appstate.wintab_session.ButtonChangedHandler = this.ButtonChangeHandler;
            this.appstate.wintab_session.Open(WinTabDN.Utils.TabletContextType.System);
        }

        private void StopWinTabSession()
        {
            this.appstate.wintab_session?.Close();
        }

        private async Task StartScaleSession()
        {
            try
            {
                if (appstate.serial_port != null && !appstate.serial_port.IsOpen)
                {
                    appstate.serial_port.Open();
                }

                if (appstate.serial_port == null)
                {
                     // Simulate or just wait if no serial port
                     // For now just return or let it run simulated?
                     // appstate.serial_port is used in ReadSerialPortAsync, need to check there too
                }

                appstate.scale_isReading = true;
                await ReadSerialPortAsync(appstate.scale_cts.Token);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ERROR Failed to open COM Port\r\n" +
                    $"{ex.GetType().FullName}\r\n" +
                $"{ex.Message}");
            }
            finally
            {
                await ReadSerialPortAsync(appstate.scale_cts.Token);
                if (appstate.serial_port != null && appstate.serial_port.IsOpen)
                {
                    appstate.serial_port.Close();
                }
                appstate.scale_isReading = false;
            }
        }

        private void StopScaleSession()
        {
            // Cancel the reading operation
            appstate.scale_cts.Cancel();
            // Create new CancellationTokenSource for next operation
            appstate.scale_cts = new CancellationTokenSource();
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
                    if (appstate.serial_port != null && appstate.serial_port.IsOpen && appstate.serial_port.BytesToRead > 0)
                    {
                        string line = await Task.Run(() => appstate.serial_port.ReadLine());
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
                                this.appstate.physi_pressure = sr.ReadingAsDouble;
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
            StopScaleSession();
        }



        private void FormPressureTester_FormClosing(object sender, FormClosingEventArgs e)
        {

            appstate.scale_cts.Cancel();
            if (appstate.serial_port != null && appstate.serial_port.IsOpen)
            {
                appstate.serial_port.Close();
                appstate.serial_port.Dispose();
            }

        }

        private void button_record_Click(object sender, EventArgs e)
        {
            this.appstate.record_collection.Add(this.appstate.physi_pressure, this.appstate.scale_session.logical_pressure_moving_average.GetAverage());

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
            sb.AppendLine(this.appstate.record_collection.GetRecordsJSON());
            sb.AppendLine("    ]");
            sb.AppendLine("}");

            string text_content = sb.ToString();
            return text_content;
        }

        private void button_clearlog_Click(object sender, EventArgs e)
        {
            this.appstate.record_collection.Clear();
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
            if (this.appstate.record_collection.Count < 1)
            {
                return;
            }

            this.appstate.record_collection.ClearLast();

            this.updatedata();

        }

        private void button_load_sample_data_Click(object sender, EventArgs e)
        {
            this.appstate.record_collection.Add(10, 0.01);
            this.appstate.record_collection.Add(100, 0.40);
            this.appstate.record_collection.Add(150, 0.50);
            this.appstate.record_collection.Add(400, 0.85);
            this.appstate.record_collection.Add(500, 1.00);
            this.updatedata();
        }

        public void updatedata()
        {
            this.label_recordcount.Text = this.appstate.record_collection.Count.ToString();
            this.textBox_log.Text = this.appstate.record_collection.GetRecordsText();

            textBox_log.SelectionStart = textBox_log.TextLength;
            textBox_log.ScrollToCaret();


            double[] dataX = this.appstate.record_collection.items.Select(i => i.PhysicalPressure).ToArray();
            double[] dataY = this.appstate.record_collection.items.Select(i => i.LogicalPressure * 100).ToArray();

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
