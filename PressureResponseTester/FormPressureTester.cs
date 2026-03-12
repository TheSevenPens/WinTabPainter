using ScottPlot;
using SevenLib.WinTab.Stylus;
using System.IO.Ports;
using System.Text;

namespace WinTabPressureTester
{
    public partial class FormPressureTester : Form
    {
        private const int SerialPortReadDelay = 10;
        private const int PlotFontSize = 27;
        private const int PlotAxisLimit = 1000;
        private const int PlotPressureLimit = 110;

        AppState appstate = new AppState();



        public void UpdateCharTitle()
        {
            formsPlot1.Plot.Title($"Pressure response {this.textBox_brand.Text} {this.textBox_Pen.Text} ({this.textBox_date.Text}) ");
            formsPlot1.Refresh();

        }
        public FormPressureTester()
        {
            InitializeComponent();
            this.appstate.WinTabSession = new SevenLib.WinTab.WinTabSession();
            this.appstate.ScaleSession = new ScaleSession();


            this.textBox_date.Text = DateTime.Today.ToString("yyyy-MM-dd");
            this.textBox_User.Text = System.Environment.UserName.ToUpper().Trim();

            if (this.comboBoxcomport.Items != null)
            {
                var portnames = SerialPort.GetPortNames();
                this.comboBoxcomport.Items.AddRange(portnames);
            }

            this.UpdateCharTitle();

            formsPlot1.Plot.XLabel("Physical pressure (gf)");
            formsPlot1.Plot.YLabel("Logical pressure (%)");

            formsPlot1.Plot.Axes.Title.Label.FontSize = PlotFontSize;

            formsPlot1.Plot.Axes.Left.Label.FontSize = PlotFontSize;
            formsPlot1.Plot.Axes.Bottom.Label.FontSize = PlotFontSize;

            formsPlot1.Plot.Axes.SetLimits(0, PlotAxisLimit, 0, PlotPressureLimit);
            formsPlot1.UserInputProcessor.IsEnabled = false;

            formsPlot1.Plot.Grid.MajorLineColor = Colors.Black.WithOpacity(.1);
            formsPlot1.Plot.Grid.MajorLineWidth = 2;

            formsPlot1.Plot.Axes.Left.TickLabelStyle.FontSize = PlotFontSize;
            formsPlot1.Plot.Axes.Bottom.TickLabelStyle.FontSize = PlotFontSize;


            this.appstate.QueueLogical = new SevenLib.Numerics.IndexedQueue<double>(this.appstate.LogicalPressureQueueSize);

            string comportname = GetSelectedComPortName();
            if (!string.IsNullOrEmpty(comportname))
            {
               this.appstate.SerialPort = new SerialPort(comportname);
            }
            else
            {
               this.appstate.SerialPort = null;
            }
            this.appstate.ScaleCts = new CancellationTokenSource();

            this.appstate.RecordCollection = new PressureRecordCollection();
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
                this.appstate.WinTabSession?.Dispose();

                // Scale Session Resources
                this.appstate.ScaleCts?.Cancel();
                this.appstate.ScaleCts?.Dispose();

                CloseAndDisposeSerialPort();

                // GDI+ Resources
                this.np_pressure_guage?.Dispose();
                this.pictureBox1.Image?.Dispose();
                this.pictureBox1?.Dispose();
                this.gfx_picbox1?.Dispose();
            }
            base.Dispose(disposing);
        }

        private void CloseAndDisposeSerialPort()
        {
            if (appstate.SerialPort is not null)
            {
                if (appstate.SerialPort.IsOpen)
                {
                    appstate.SerialPort.Close();
                }
                appstate.SerialPort.Dispose();
            }
        }



        private static bool get_press_change_as_letter(StylusButtonChangeType change)
        {
            return change switch
            {
                StylusButtonChangeType.Pressed => true,
                StylusButtonChangeType.Released => false,
                _ => throw new System.ArgumentOutOfRangeException()
            };
        }
        private void PacketHandler(SevenLib.WinTab.Structs.WintabPacket wintab_pkt)
        {

            var button_info = new SevenLib.WinTab.Stylus.StylusButtonChange(wintab_pkt.pkButtons);
            if (button_info.Change != StylusButtonChangeType.NoChange)
            {
                if (button_info.ButtonId == SevenLib.Stylus.StylusButtonId.Tip)
                {
                    this.checkBox_tipdown.Checked = get_press_change_as_letter(button_info.Change);
                }
                else if (button_info.ButtonId == SevenLib.Stylus.StylusButtonId.LowerButton)
                {
                    this.checkBox_lowerbuttondown.Checked = get_press_change_as_letter(button_info.Change);
                }
                else if (button_info.ButtonId == SevenLib.Stylus.StylusButtonId.UpperButton)
                {
                    this.checkBox_upperbuttondown.Checked = get_press_change_as_letter(button_info.Change);
                }
            }


            uint raw_pressure = wintab_pkt.pkNormalPressure;
            double normalized_raw_pressure = (raw_pressure / (1.0 * this.appstate.WinTabSession.TabletInfo.MaxPressure));

            this.appstate.LogicalPressure = normalized_raw_pressure;

            SevenLib.Trigonometry.TiltAA tiltAA = new(wintab_pkt.pkOrientation.orAzimuth / 10.0, wintab_pkt.pkOrientation.orAltitude / 10.0);
            var tiltxy_deg = tiltAA.ToXY_Deg();

            this.label_pressure_raw.Text = wintab_pkt.pkNormalPressure.ToString();
            this.label_normalized_pressure.Text = $"{normalized_raw_pressure * 100.0:00.000}%";

            this.label_or_altitude.Text = $"{wintab_pkt.pkOrientation.orAltitude / 10.0:F0}°";
            this.label_or_azimuth.Text = $"{wintab_pkt.pkOrientation.orAzimuth / 10.0:F0}°";
            this.label_tiltx.Text = $"{tiltxy_deg.X:00.000}°";
            this.label_tilty.Text = $"{tiltxy_deg.Y:00.000}°";

            this.appstate.ScaleSession.LogicalPressureMovingAverage.AddSample(normalized_raw_pressure);
            double cur_logical_pressure_ma = this.appstate.ScaleSession.LogicalPressureMovingAverage.GetAverage();
            this.label_normalizedpressure_ma.Text = $"{cur_logical_pressure_ma * 100.0:00.00}%";

            if (this.appstate.QueueLogical.Count >= this.appstate.LogicalPressureQueueSize)
            {
                if (this.appstate.QueueLogical.Count > 0)
                {
                    this.appstate.QueueLogical.Dequeue();
                }
            }
            this.appstate.QueueLogical.Enqueue(cur_logical_pressure_ma);

        }

        private void ButtonChangeHandler(SevenLib.WinTab.Structs.WintabPacket wintab_pkt, StylusButtonChange buttonchange)
        {
            if (buttonchange.Change == StylusButtonChangeType.Released)
            {
                this.appstate.ScaleSession.LogicalPressureMovingAverage.Clear();
            }
        }

        private async void button_start_Click(object sender, EventArgs e)
        {

            if (!appstate.ScaleIsReading)
            {
                await StartScaleSession();
            }
        }

        private void StartWinTabSession()
        {
            this.appstate.WinTabSession.OnWinTabPacketReceived = this.PacketHandler;
            this.appstate.WinTabSession.OnButtonStateChanged = this.ButtonChangeHandler;
            this.appstate.WinTabSession.Open(SevenLib.WinTab.Enums.TabletContextType.System);
        }

        private void StopWinTabSession()
        {
            this.appstate.WinTabSession?.Close();
        }

        private async Task StartScaleSession()
        {
            try
            {
                if (appstate.SerialPort is not null && !appstate.SerialPort.IsOpen)
                {
                    appstate.SerialPort.Open();
                }

                appstate.ScaleIsReading = true;
                await ReadSerialPortAsync(appstate.ScaleCts.Token);
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show($"Failed to open COM Port - Access Denied\r\n{ex.Message}");
            }
            catch (System.IO.IOException ex)
            {
                MessageBox.Show($"Failed to open COM Port - IO Error\r\n{ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open COM Port\r\n{ex.GetType().FullName}\r\n{ex.Message}");
            }
            finally
            {
                CloseAndDisposeSerialPort();
                appstate.ScaleIsReading = false;
            }
        }

        private void StopScaleSession()
        {
            appstate.ScaleCts.Cancel();
            appstate.ScaleCts = new CancellationTokenSource();
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
            if (line is null)
            {
                return new ScaleParsedLine { Input = line, Parsed = false, ScaleRecord = null, Error = "Line was null" };
            }

            line = line.Trim();

            if (line.Length == 0)
            {
                return new ScaleParsedLine { Input = line, Parsed = false, ScaleRecord = null, Error = "Line was empty" };
            }

            var tokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (tokens.Length == 0)
            {
                return new ScaleParsedLine { Input = line, Parsed = false, ScaleRecord = null, Error = "No tokens in line" };
            }

            string str_force = tokens[^1];
            str_force = TrimLastCharIf(str_force, 'M');
            str_force = TrimLastCharIf(str_force, 'g');

            var sr = new ScaleRecord { Line = line, ReadingAsString = str_force };

            try
            {
                sr.ReadingAsDouble = double.Parse(str_force);
            }
            catch (FormatException)
            {
                return new ScaleParsedLine { Input = line, Parsed = false, ScaleRecord = null, Error = $"Failed to parse force \"{str_force}\"" };
            }

            return new ScaleParsedLine { Input = line, Parsed = true, ScaleRecord = sr, Error = string.Empty };
        }

        private async Task ReadSerialPortAsync(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    if (appstate.SerialPort is not null && appstate.SerialPort.IsOpen && appstate.SerialPort.BytesToRead > 0)
                    {
                        string line = await Task.Run(() => appstate.SerialPort.ReadLine(), cancellationToken);
                        var sr_parse = ParseScaleLine(line);

                        if (sr_parse.Parsed && sr_parse.ScaleRecord is not null)
                        {
                            this.appstate.PhysicalPressure = sr_parse.ScaleRecord.ReadingAsDouble;
                            Update_Scale_UI_Elements(sr_parse.ScaleRecord.ReadingAsString);
                        }
                    }

                    await Task.Delay(SerialPortReadDelay, cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
                this.textBox_log.AppendText($"Reading cancelled{Environment.NewLine}");
            }
            catch (System.IO.IOException ex)
            {
                MessageBox.Show($"Serial port IO error: {ex.Message}");
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
                Invoke(new Action(() => this.label_force.Text = $"{str_force} gf"));
            }
            else
            {
                this.label_force.Text = $"{str_force} gf";
            }
        }

        private void button_stop_Click(object sender, EventArgs e)
        {
            StopScaleSession();
        }



        private void FormPressureTester_FormClosing(object sender, FormClosingEventArgs e)
        {
            appstate.ScaleCts.Cancel();
            CloseAndDisposeSerialPort();
        }

        private void button_record_Click(object sender, EventArgs e)
        {
            this.appstate.RecordCollection.Add(this.appstate.PhysicalPressure, this.appstate.ScaleSession.LogicalPressureMovingAverage.GetAverage());
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
            sb.AppendLine($@"    ""penfamily"": """" , ");
            sb.AppendLine($@"    ""inventoryid"": ""{textBox_inventoryid.Text.Trim().ToUpper()}"" , ");
            sb.AppendLine($@"    ""date"": ""{this.textBox_date.Text.Trim().ToUpper()}"" , ");
            sb.AppendLine($@"    ""user"": ""{this.textBox_User.Text.Trim().ToUpper()}"" , ");
            sb.AppendLine($@"    ""tablet"": ""{this.textBox_Tablet.Text.Trim().ToUpper()}"" , ");
            sb.AppendLine($@"    ""driver"": ""{this.textBox_driver.Text.Trim().ToUpper()}"" , ");
            sb.AppendLine($@"    ""os"": ""{this.textBox_OS.Text.Trim().ToUpper()}"" , ");
            sb.AppendLine($@"    ""notes"": """" , ");
            sb.AppendLine("    \"records\": [  ");
            sb.AppendLine(this.appstate.RecordCollection.GetRecordsJSON());
            sb.AppendLine("    ]");
            sb.AppendLine("}");
            return sb.ToString();
        }

        private void button_clearlog_Click(object sender, EventArgs e)
        {
            this.appstate.RecordCollection.Clear();
            this.updatedata();
        }

        private void button_clearlast_Click(object sender, EventArgs e)
        {
            if (this.appstate.RecordCollection.Count < 1)
            {
                return;
            }

            this.appstate.RecordCollection.ClearLast();
            this.updatedata();
        }

        private void button_load_sample_data_Click(object sender, EventArgs e)
        {
            this.appstate.RecordCollection.Add(10, 0.01);
            this.appstate.RecordCollection.Add(100, 0.40);
            this.appstate.RecordCollection.Add(150, 0.50);
            this.appstate.RecordCollection.Add(400, 0.85);
            this.appstate.RecordCollection.Add(500, 1.00);
            this.updatedata();
        }

        public void updatedata()
        {
            this.label_recordcount.Text = this.appstate.RecordCollection.Count.ToString();
            this.textBox_log.Text = this.appstate.RecordCollection.GetRecordsText();

            textBox_log.SelectionStart = textBox_log.TextLength;
            textBox_log.ScrollToCaret();

            double[] dataX = this.appstate.RecordCollection.items.Select(i => i.PhysicalPressure).ToArray();
            double[] dataY = this.appstate.RecordCollection.items.Select(i => i.LogicalPressure * 100).ToArray();

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
                MessageBox.Show($"File saved {filePath}");
            }
            catch (System.IO.IOException ex)
            {
                MessageBox.Show($"Error saving file - IO Error: {ex.Message}");
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show($"Error saving file - Access Denied: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving file: {ex.Message}");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.UpdateCharTitle();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // not implemented yet
        }
    }
}
