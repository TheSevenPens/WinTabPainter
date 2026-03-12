using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using SevenLib.WinTab.Stylus;
using System.IO.Ports;
using System.Text;
using System.Text.Json;
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
    private const int PlotPressureLimit = 100;
    private const char ScaleReadingMgSuffix = 'M';
    private const char ScaleReadingGramSuffix = 'g';

    private AppState? appstate;
    private string? currentLoadedFilePath;

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
    }

    private void InitializePlot()
    {
        var model = new PlotModel
        {
            Title = "Pressure response",
            TitleFontSize = PlotFontSize,
            Background = OxyColors.White,
            PlotAreaBackground = OxyColor.FromRgb(250, 250, 250)
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

    private void plotView1_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
    {
        e.Handled = true;
    }

    public void UpdateCharTitle()
    {
        // Safely handle null controls during initialization
        if (textBox_brand is null || textBox_inventoryid is null || textBox_date is null || plotView1 is null)
        {
            return;
        }

        var brandText = textBox_brand.Text?.Trim() ?? "BRAND";
        var idText = textBox_inventoryid.Text?.Trim() ?? "ID";
        var dateText = textBox_date.Text?.Trim() ?? "YYYY-MM-DD";
        if (plotView1.Model is PlotModel model)
        {
            model.Title = $"{brandText}/{idText}/{dateText}";
            plotView1.InvalidatePlot(false);
        }
    }

    private void OnChartTitleFieldChanged(object sender, TextChangedEventArgs e)
    {
        UpdateCharTitle();
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
        UpdateCharTitle();
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
                MessageBox.Show("Reading cancelled", "Information"));
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
        sb.AppendLine($@"    ""penfamily"": ""{textBox_penfamily.Text.Trim().ToUpper()}"" , ");
        sb.AppendLine($@"    ""inventoryid"": ""{textBox_inventoryid.Text.Trim().ToUpper()}"" , ");
        sb.AppendLine($@"    ""date"": ""{textBox_date.Text.Trim().ToUpper()}"" , ");
        sb.AppendLine($@"    ""user"": ""{textBox_User.Text.Trim().ToUpper()}"" , ");
        sb.AppendLine($@"    ""tablet"": ""{textBox_Tablet.Text.Trim().ToUpper()}"" , ");
        sb.AppendLine($@"    ""driver"": ""{textBox_driver.Text.Trim().ToUpper()}"" , ");
        sb.AppendLine($@"    ""os"": ""{textBox_OS.Text.Trim().ToUpper()}"" , ");
        sb.AppendLine($@"    ""tags"": ""{textBox_tags.Text.Trim()}"" , ");
        sb.AppendLine($@"    ""notes"": ""{textBox_notes.Text.Trim()}"" , ");
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
        dataGrid_records.ItemsSource = appstate.RecordCollection.items;
        dataGrid_records.Items.Refresh();

        if (plotView1.Model is not PlotModel model)
            return;

        // Clear existing series
        model.Series.Clear();

        var dataX = appstate.RecordCollection.items.Select(i => i.PhysicalPressure).ToList();
        var dataY = appstate.RecordCollection.items.Select(i => i.LogicalPressure * 100).ToList();

        // Create line series to connect points
        var lineSeries = new LineSeries
        {
            Color = OxyColors.Black,
            StrokeThickness = 2,
            MarkerType = MarkerType.Circle,
            MarkerSize = 4,
            MarkerStroke = OxyColors.Black,
            MarkerFill = OxyColors.Black
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

    private void button_save_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(currentLoadedFilePath))
        {
            MessageBox.Show("No file loaded. Use Export JSON to save to a new file.", "Save Error");
            return;
        }

        try
        {
            string json = CreateJSONContent();
            System.IO.File.WriteAllText(currentLoadedFilePath, json);
            MessageBox.Show($"File saved: {currentLoadedFilePath}", "Save Successful");
        }
        catch (System.IO.IOException ex)
        {
            MessageBox.Show($"Error saving file - IO Error: {ex.Message}", "Save Error");
        }
        catch (UnauthorizedAccessException ex)
        {
            MessageBox.Show($"Error saving file - Access Denied: {ex.Message}", "Save Error");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving file: {ex.Message}", "Save Error");
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

        // Check for Ctrl+S (Save)
        if (System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.LeftCtrl) || 
            System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.RightCtrl))
        {
            if (e.Key == System.Windows.Input.Key.S)
            {
                button_save_Click(null!, null!);
                e.Handled = true;
                return;
            }
        }

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

    private void Window_DragOver(object sender, System.Windows.DragEventArgs e)
    {
        if (e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop))
        {
            string[] files = (string[])e.Data.GetData(System.Windows.DataFormats.FileDrop);
            if (files.Any(f => f.EndsWith(".json", StringComparison.OrdinalIgnoreCase)))
            {
                e.Effects = System.Windows.DragDropEffects.Copy;
                e.Handled = true;
            }
            else
            {
                e.Effects = System.Windows.DragDropEffects.None;
                e.Handled = true;
            }
        }
    }

    private void Window_Drop(object sender, System.Windows.DragEventArgs e)
    {
        if (e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop))
        {
            string[] files = (string[])e.Data.GetData(System.Windows.DataFormats.FileDrop);
            string? jsonFile = files.FirstOrDefault(f => f.EndsWith(".json", StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(jsonFile))
            {
                try
                {
                    LoadJSONFile(jsonFile);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading JSON file: {ex.Message}", "Load Error");
                }
            }
        }
    }

    private void LoadJSONFile(string filePath)
    {
        string json = System.IO.File.ReadAllText(filePath);
        var data = System.Text.Json.JsonSerializer.Deserialize<PressureTestData>(json);

        if (data?.records != null && data.records.Count > 0)
        {
            appstate!.RecordCollection!.Clear();

            foreach (var record in data.records)
            {
                if (record.Count >= 2)
                {
                    appstate.RecordCollection.Add(record[0], record[1] / 100.0);
                }
            }

            // Update metadata from JSON
            if (!string.IsNullOrEmpty(data.brand))
                textBox_brand.Text = data.brand;
            if (!string.IsNullOrEmpty(data.pen))
                textBox_Pen.Text = data.pen;
            if (!string.IsNullOrEmpty(data.penfamily))
                textBox_penfamily.Text = data.penfamily;
            if (!string.IsNullOrEmpty(data.inventoryid))
                textBox_inventoryid.Text = data.inventoryid;
            if (!string.IsNullOrEmpty(data.date))
                textBox_date.Text = data.date;
            if (!string.IsNullOrEmpty(data.user))
                textBox_User.Text = data.user;
            if (!string.IsNullOrEmpty(data.tablet))
                textBox_Tablet.Text = data.tablet;
            if (!string.IsNullOrEmpty(data.driver))
                textBox_driver.Text = data.driver;
            if (!string.IsNullOrEmpty(data.os))
                textBox_OS.Text = data.os;
            if (!string.IsNullOrEmpty(data.tags))
                textBox_tags.Text = data.tags;
            if (!string.IsNullOrEmpty(data.notes))
                textBox_notes.Text = data.notes;

            // Store the file path for saving later
            currentLoadedFilePath = filePath;
            button_save.IsEnabled = true;

            UpdateCharTitle();
            updatedata();
            MessageBox.Show($"Loaded {appstate.RecordCollection.Count} records from {System.IO.Path.GetFileName(filePath)}", "Load Success");
        }
        else
        {
            MessageBox.Show("No valid records found in JSON file", "Load Error");
        }
    }
}

internal class PressureTestData
{
    public string? brand { get; set; }
    public string? pen { get; set; }
    public string? penfamily { get; set; }
    public string? inventoryid { get; set; }
    public string? date { get; set; }
    public string? user { get; set; }
    public string? tablet { get; set; }
    public string? driver { get; set; }
    public string? os { get; set; }
    public string? tags { get; set; }
    public string? notes { get; set; }
    public List<List<double>>? records { get; set; }
}
