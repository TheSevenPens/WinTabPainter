using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using SevenLib.WinTab.Stylus;
using System.IO.Ports;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace WinTabPressureTester;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private const int SerialPortReadDelay = 10;
    private const int PlotFontSize = 14;
    private const int PlotAxisLimit = 1000;
    private const int PlotPressureLimit = 110;
    private const char ScaleReadingMgSuffix = 'M';
    private const char ScaleReadingGramSuffix = 'g';

    private AppState? appstate;

    public MainWindow()
    {
        InitializeComponent();

        // Initialize AppState with required members
        var winTabSession = new SevenLib.WinTab.WinTabSession();
        var scaleSession = new ScaleSession();
        var recordCollection = new PressureRecordCollection();
        var scaleCts = new CancellationTokenSource();

        appstate = new AppState
        {
            WinTabSession = winTabSession,
            ScaleSession = scaleSession,
            RecordCollection = recordCollection,
            ScaleCts = scaleCts
        };

        this.Loaded += MainWindow_Loaded;
        this.Closing += MainWindow_Closing;

        InitializeUI();
    }

    private void InitializeUI()
    {
        // Set initial values
        textBox_date.Text = DateTime.Today.ToString("yyyy-MM-dd");
        textBox_User.Text = Environment.UserName.ToUpper().Trim();

        // Populate COM ports
        var portnames = SerialPort.GetPortNames();
        if (portnames.Length > 0)
        {
            foreach (var port in portnames)
            {
                comboBoxcomport.Items.Add(port);
            }
            comboBoxcomport.SelectedIndex = portnames.Length - 1;
        }

        // Initialize chart
        InitializePlot();

        // Initialize queue
        appstate!.QueueLogical = new SevenLib.Numerics.IndexedQueue<double>(appstate.LogicalPressureQueueSize);

        // Set up COM port if available
        string? comportname = GetSelectedComPortName();
        if (!string.IsNullOrEmpty(comportname))
        {
            appstate.SerialPort = new SerialPort(comportname);
        }

        UpdateCharTitle();
    }

    private void InitializePlot()
    {
        var model = new PlotModel
        {
            Title = "Pressure response",
            TitleFontSize = PlotFontSize,
            Background = OxyColors.White
        };

        // Add X-axis
        var xAxis = new LinearAxis
        {
            Position = AxisPosition.Bottom,
            Title = "Physical pressure (gf)",
            Minimum = 0,
            Maximum = PlotAxisLimit,
            TitleFontSize = PlotFontSize * 0.9,
            FontSize = PlotFontSize * 0.75
        };
        model.Axes.Add(xAxis);

        // Add Y-axis
        var yAxis = new LinearAxis
        {
            Position = AxisPosition.Left,
            Title = "Logical pressure (%)",
            Minimum = 0,
            Maximum = PlotPressureLimit,
            TitleFontSize = PlotFontSize * 0.9,
            FontSize = PlotFontSize * 0.75
        };
        model.Axes.Add(yAxis);

        plotView1.Model = model;
    }

    private void plotView1_MouseDown(object sender, System.Windows.Input.MouseEventArgs e)
    {
        e.Handled = true;
    }

    public void UpdateCharTitle()
    {
        var brandText = textBox_brand.Text;
        var penText = textBox_Pen.Text;
        var dateText = textBox_date.Text;
        if (plotView1.Model is PlotModel model)
        {
            model.Title = $"Pressure response {brandText} {penText} ({dateText})";
            plotView1.InvalidatePlot(false);
        }
    }

    private string? GetSelectedComPortName()
    {
        var portnames = SerialPort.GetPortNames();
        if (portnames is null or [])
        {
            return null;
        }

        return comboBoxcomport.SelectedItem?.ToString()?.ToUpper();
    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        StartWinTabSession();
    }

    private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        StopWinTabSession();
        StopScaleSession();
        CloseAndDisposeSerialPort();

        // Cleanup
        appstate?.WinTabSession?.Dispose();
        appstate?.ScaleCts?.Dispose();
    }

    private void CloseAndDisposeSerialPort()
    {
        if (appstate?.SerialPort is not null)
        {
            if (appstate.SerialPort.IsOpen)
            {
                appstate.SerialPort.Close();
            }
            appstate.SerialPort.Dispose();
        }
    }

    private static bool GetButtonPressed(StylusButtonChangeType change) =>
        change switch
        {
            StylusButtonChangeType.Pressed => true,
            StylusButtonChangeType.Released => false,
            _ => throw new ArgumentOutOfRangeException(nameof(change))
        };

    private void PacketHandler(SevenLib.WinTab.Structs.WintabPacket wintab_pkt)
    {
        Dispatcher.Invoke(() =>
        {
            var button_info = new SevenLib.WinTab.Stylus.StylusButtonChange(wintab_pkt.pkButtons);
            if (button_info.Change != StylusButtonChangeType.NoChange)
            {
                if (button_info.ButtonId == SevenLib.Stylus.StylusButtonId.Tip)
                {
                    checkBox_tipdown.IsChecked = GetButtonPressed(button_info.Change);
                }
                else if (button_info.ButtonId == SevenLib.Stylus.StylusButtonId.LowerButton)
                {
                    checkBox_lowerbuttondown.IsChecked = GetButtonPressed(button_info.Change);
                }
                else if (button_info.ButtonId == SevenLib.Stylus.StylusButtonId.UpperButton)
                {
                    checkBox_upperbuttondown.IsChecked = GetButtonPressed(button_info.Change);
                }
            }

            uint raw_pressure = wintab_pkt.pkNormalPressure;
            double normalized_raw_pressure = raw_pressure / (1.0 * appstate!.WinTabSession!.TabletInfo.MaxPressure);

            appstate.LogicalPressure = normalized_raw_pressure;

            var tiltAA = new SevenLib.Trigonometry.TiltAA(
                wintab_pkt.pkOrientation.orAzimuth / 10.0,
                wintab_pkt.pkOrientation.orAltitude / 10.0);
            var tiltxy_deg = tiltAA.ToXY_Deg();

            label_pressure_raw.Content = wintab_pkt.pkNormalPressure.ToString();
            label_normalized_pressure.Content = $"{normalized_raw_pressure * 100.0:00.000}%";

            label_or_altitude.Content = $"{wintab_pkt.pkOrientation.orAltitude / 10.0:F0}°";
            label_or_azimuth.Content = $"{wintab_pkt.pkOrientation.orAzimuth / 10.0:F0}°";
            label_tiltx.Content = $"{tiltxy_deg.X:00.000}°";
            label_tilty.Content = $"{tiltxy_deg.Y:00.000}°";

            appstate.ScaleSession!.LogicalPressureMovingAverage.AddSample(normalized_raw_pressure);
            double cur_logical_pressure_ma = appstate.ScaleSession.LogicalPressureMovingAverage.GetAverage();
            label_normalizedpressure_ma.Content = $"{cur_logical_pressure_ma * 100.0:00.00}%";

            if (appstate.QueueLogical!.Count >= appstate.LogicalPressureQueueSize)
            {
                if (appstate.QueueLogical.Count > 0)
                {
                    appstate.QueueLogical.Dequeue();
                }
            }
            appstate.QueueLogical.Enqueue(cur_logical_pressure_ma);
        });
    }

    private void ButtonChangeHandler(SevenLib.WinTab.Structs.WintabPacket wintab_pkt, StylusButtonChange buttonchange)
    {
        if (buttonchange.Change == StylusButtonChangeType.Released)
        {
            appstate!.ScaleSession!.LogicalPressureMovingAverage.Clear();
        }
    }

    private void StartWinTabSession()
    {
        try
        {
            appstate!.WinTabSession!.OnWinTabPacketReceived = PacketHandler;
            appstate.WinTabSession.OnButtonStateChanged = ButtonChangeHandler;
            appstate.WinTabSession.Open(SevenLib.WinTab.Enums.TabletContextType.System);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to initialize WinTab: {ex.Message}", "Initialization Error");
        }
    }

    private void StopWinTabSession()
    {
        appstate?.WinTabSession?.Close();
    }

    private async void button_start_Click(object sender, RoutedEventArgs e)
    {
        if (!appstate!.ScaleIsReading)
        {
            await StartScaleSession();
        }
    }

    private async Task StartScaleSession()
    {
        try
        {
            if (appstate!.SerialPort is not null && !appstate.SerialPort.IsOpen)
            {
                appstate.SerialPort.Open();
            }

            appstate.ScaleIsReading = true;
            await ReadSerialPortAsync(appstate.ScaleCts!.Token);
        }
        catch (UnauthorizedAccessException ex)
        {
            MessageBox.Show($"Failed to open COM Port - Access Denied\r\n{ex.Message}", "COM Port Error");
        }
        catch (System.IO.IOException ex)
        {
            MessageBox.Show($"Failed to open COM Port - IO Error\r\n{ex.Message}", "COM Port Error");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to open COM Port\r\n{ex.GetType().FullName}\r\n{ex.Message}", "COM Port Error");
        }
        finally
        {
            CloseAndDisposeSerialPort();
            appstate!.ScaleIsReading = false;
        }
    }

    private void StopScaleSession()
    {
        if (appstate?.ScaleCts is not null)
        {
            appstate.ScaleCts.Cancel();
            appstate.ScaleCts = new CancellationTokenSource();
        }
    }

    private void button_stop_Click(object sender, RoutedEventArgs e)
    {
        StopScaleSession();
    }

    public static string TrimLastCharIf(string s, char c) 
        => (s is null or "") ? s : (s[^1] == c ? s[..^1] : s);

    public static ScaleParsedLine ParseScaleLine(string? line)
    {
        if (line is null)
        {
            return new ScaleParsedLine(line, false, null, "Line was null");
        }

        line = line.Trim();

        if (line.Length == 0)
        {
            return new ScaleParsedLine(line, false, null, "Line was empty");
        }

        var tokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (tokens.Length == 0)
        {
            return new ScaleParsedLine(line, false, null, "No tokens in line");
        }

        string str_force = tokens[^1];
        str_force = TrimLastCharIf(str_force, ScaleReadingMgSuffix);
        str_force = TrimLastCharIf(str_force, ScaleReadingGramSuffix);

        var sr = new ScaleRecord { Line = line, ReadingAsString = str_force };

        try
        {
            sr.ReadingAsDouble = double.Parse(str_force);
        }
        catch (FormatException)
        {
            return new ScaleParsedLine(line, false, null, $"Failed to parse force \"{str_force}\"");
        }

        return new ScaleParsedLine(line, true, sr, string.Empty);
    }

    private async Task ReadSerialPortAsync(CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (appstate!.SerialPort is not null && appstate.SerialPort.IsOpen && appstate.SerialPort.BytesToRead > 0)
                {
                    string line = await Task.Run(() => appstate.SerialPort.ReadLine(), cancellationToken);
                    var sr_parse = ParseScaleLine(line);

                    if (sr_parse.Parsed && sr_parse.ScaleRecord is not null)
                    {
                        appstate.PhysicalPressure = sr_parse.ScaleRecord.ReadingAsDouble;
                        Dispatcher.Invoke(() => Update_Scale_UI_Elements(sr_parse.ScaleRecord.ReadingAsString));
                    }
                }

                await Task.Delay(SerialPortReadDelay, cancellationToken);
            }
        }
        catch (OperationCanceledException)
        {
            Dispatcher.Invoke(() => 
                textBox_log.AppendText($"Reading cancelled{Environment.NewLine}"));
        }
        catch (System.IO.IOException ex)
        {
            MessageBox.Show($"Serial port IO error: {ex.Message}", "Serial Port Error");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Serial port error: {ex.Message}", "Serial Port Error");
        }
    }

    private void Update_Scale_UI_Elements(string str_force)
    {
        label_force.Content = $"{str_force} gf";
    }

    private void button_record_Click(object sender, RoutedEventArgs e)
    {
        appstate!.RecordCollection!.Add(
            appstate.PhysicalPressure,
            appstate.ScaleSession!.LogicalPressureMovingAverage.GetAverage());
        updatedata();
    }

    private string CreateJSONContent()
    {
        var sb = new StringBuilder();
        sb.AppendLine("{");
        sb.AppendLine($@"    ""brand"": ""{textBox_brand.Text.Trim().ToUpper()}"" , ");
        sb.AppendLine($@"    ""pen"": ""{textBox_Pen.Text.Trim().ToUpper()}"" , ");
        sb.AppendLine($@"    ""penfamily"": """" , ");
        sb.AppendLine($@"    ""inventoryid"": ""{textBox_inventoryid.Text.Trim().ToUpper()}"" , ");
        sb.AppendLine($@"    ""date"": ""{textBox_date.Text.Trim().ToUpper()}"" , ");
        sb.AppendLine($@"    ""user"": ""{textBox_User.Text.Trim().ToUpper()}"" , ");
        sb.AppendLine($@"    ""tablet"": ""{textBox_Tablet.Text.Trim().ToUpper()}"" , ");
        sb.AppendLine($@"    ""driver"": ""{textBox_driver.Text.Trim().ToUpper()}"" , ");
        sb.AppendLine($@"    ""os"": ""{textBox_OS.Text.Trim().ToUpper()}"" , ");
        sb.AppendLine($@"    ""notes"": """" , ");
        sb.AppendLine("    \"records\": [  ");
        sb.AppendLine(appstate!.RecordCollection!.GetRecordsJSON());
        sb.AppendLine("    ]");
        sb.AppendLine("}");
        return sb.ToString();
    }

    private void button_copytext_Click(object sender, RoutedEventArgs e)
    {
        string text_content = CreateJSONContent();
        System.Windows.Clipboard.SetText(text_content);
        MessageBox.Show("JSON copied to clipboard", "Success");
    }

    private void button_clearlog_Click(object sender, RoutedEventArgs e)
    {
        appstate!.RecordCollection!.Clear();
        updatedata();
    }

    private void button_clearlast_Click(object sender, RoutedEventArgs e)
    {
        if (appstate!.RecordCollection!.Count < 1)
        {
            return;
        }

        appstate.RecordCollection.ClearLast();
        updatedata();
    }

    private void button_load_sample_data_Click(object sender, RoutedEventArgs e)
    {
        appstate!.RecordCollection!.Add(10, 0.01);
        appstate.RecordCollection.Add(100, 0.40);
        appstate.RecordCollection.Add(150, 0.50);
        appstate.RecordCollection.Add(400, 0.85);
        appstate.RecordCollection.Add(500, 1.00);
        updatedata();
    }

    public void updatedata()
    {
        label_recordcount.Content = appstate!.RecordCollection!.Count.ToString();
        textBox_log.Text = appstate.RecordCollection.GetRecordsText();

        textBox_log.ScrollToEnd();

        if (plotView1.Model is not PlotModel model)
            return;

        // Clear existing series
        model.Series.Clear();

        var dataX = appstate.RecordCollection.items.Select(i => i.PhysicalPressure).ToList();
        var dataY = appstate.RecordCollection.items.Select(i => i.LogicalPressure * 100).ToList();

        // Create line series to connect points
        var lineSeries = new LineSeries
        {
            Color = OxyColors.Blue,
            StrokeThickness = 2,
            MarkerType = MarkerType.Circle,
            MarkerSize = 6,
            MarkerStroke = OxyColors.Blue,
            MarkerFill = OxyColors.Blue
        };

        for (int i = 0; i < dataX.Count; i++)
        {
            lineSeries.Points.Add(new DataPoint(dataX[i], dataY[i]));
        }

        model.Series.Add(lineSeries);
        plotView1.InvalidatePlot(false);
    }

    private void button_export_Click(object sender, RoutedEventArgs e)
    {
        string json = CreateJSONContent();
        string datestring = textBox_date.Text.Trim().ToUpper();
        string inventoryid = textBox_inventoryid.Text.Trim().ToUpper();
        string filename = $"{inventoryid}_{datestring}.json";

        string myDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string filePath = System.IO.Path.Combine(myDocumentsPath, filename);

        try
        {
            System.IO.File.WriteAllText(filePath, json);
            MessageBox.Show($"File saved: {filePath}", "Export Successful");
        }
        catch (System.IO.IOException ex)
        {
            MessageBox.Show($"Error saving file - IO Error: {ex.Message}", "Export Error");
        }
        catch (UnauthorizedAccessException ex)
        {
            MessageBox.Show($"Error saving file - Access Denied: {ex.Message}", "Export Error");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving file: {ex.Message}", "Export Error");
        }
    }

    private void button_copy_chart_Click(object sender, RoutedEventArgs e)
    {
        UpdateCharTitle();
        MessageBox.Show("Chart title updated", "Success");
    }

    private void Window_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (e.IsRepeat)
            return;

        switch (e.Key)
        {
            case System.Windows.Input.Key.R:
                button_record_Click(null!, null!);
                e.Handled = true;
                break;
            case System.Windows.Input.Key.L:
                button_load_sample_data_Click(null!, null!);
                e.Handled = true;
                break;
            case System.Windows.Input.Key.C:
                button_clearlast_Click(null!, null!);
                e.Handled = true;
                break;
            case System.Windows.Input.Key.A:
                button_clearlog_Click(null!, null!);
                e.Handled = true;
                break;
            case System.Windows.Input.Key.S:
                button_start_Click(null!, null!);
                e.Handled = true;
                break;
            case System.Windows.Input.Key.T:
                button_stop_Click(null!, null!);
                e.Handled = true;
                break;
        }
    }
}
