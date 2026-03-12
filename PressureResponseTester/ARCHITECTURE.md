# PressureResponseTester - WPF Architecture

## Project Structure

```
PressureResponseTester/
├── App.xaml                           # Application definition
├── App.xaml.cs                        # Application code-behind
├── MainWindow.xaml                    # Main window UI (XAML markup)
├── MainWindow.xaml.cs                 # Main window logic (code-behind)
├── Program.cs                         # Entry point configuration
│
├── AppState.cs                        # Application state container
├── ScaleSession.cs                    # Scale session management
├── ScaleRecord.cs                     # Scale reading data record
├── ScaleParsedLine.cs                 # Parsed scale line result
├── PressureRecord.cs                  # Pressure measurement record
├── PressureRecordCollection.cs        # Collection of measurements
│
├── PressureResponseTester.csproj      # Project configuration
├── WPF_MIGRATION_SUMMARY.md           # Migration documentation
└── CSharp14_Improvements.md           # C# 14 features applied
```

---

## Architecture Layers

### **1. Presentation Layer** (WPF)
**Files**: `MainWindow.xaml`, `MainWindow.xaml.cs`

**Responsibilities**:
- Define UI layout via XAML
- Handle user interactions (button clicks, input changes)
- Display data from AppState
- Update charts and sensor readings
- Manage window lifecycle

**Key Components**:
- **Sensor Panel**: Real-time pressure/orientation readings
- **Chart Panel**: ScottPlot visualization
- **Recording Panel**: Data collection and export
- **Metadata Panel**: Test parameters and annotations

---

### **2. Business Logic Layer**
**Files**: `MainWindow.xaml.cs` event handlers

**Responsibilities**:
- WinTab session management
- Serial port communication
- Data parsing and validation
- UI state management
- Event delegation

**Key Methods**:
- `StartWinTabSession()` - Initialize tablet input
- `ReadSerialPortAsync()` - Poll scale readings
- `PacketHandler()` - Process WinTab packets
- `ParseScaleLine()` - Parse serial data
- `updatedata()` - Refresh chart visualization

---

### **3. Data Layer**
**Files**: `AppState.cs`, `ScaleSession.cs`, `ScaleRecord.cs`, `PressureRecord.cs`

**Responsibilities**:
- Store application state
- Manage sensor sessions
- Hold measurement data
- Provide collection operations

**Data Structures**:
```
AppState
├── WinTabSession (IDisposable)
├── ScaleSession (record)
│   └── LogicalPressureMovingAverage
├── SerialPort (IDisposable)
├── CancellationTokenSource
├── RecordCollection
│   └── List<PressureRecord>
└── QueueLogical (IndexedQueue)
```

---

### **4. Integration Layer**
**External Dependencies**:
- `SevenLib.WinTab` - Tablet input API
- `SevenLib.Numerics` - Statistical computations
- `ScottPlot.WPF` - Charting library
- `System.IO.Ports` - Serial communication

---

## Data Flow

### **Sensor Reading Flow**
```
WinTab Device
    ↓
WinTabSession.OnWinTabPacketReceived
    ↓
PacketHandler(WintabPacket)
    ↓
AppState.LogicalPressure = normalized_value
    ↓
UI Labels Updated via Dispatcher
    ↓
User sees pressure, tilt, orientation data
```

### **Scale Reading Flow**
```
Serial Port (COM Port)
    ↓
ReadSerialPortAsync()
    ↓
ParseScaleLine(serialData)
    ↓
ScaleRecord { ReadingAsDouble, ... }
    ↓
AppState.PhysicalPressure = value
    ↓
UI Updated
```

### **Data Recording Flow**
```
User clicks "Record"
    ↓
button_record_Click()
    ↓
RecordCollection.Add(physical, logical)
    ↓
updatedata()
    ↓
Chart refreshes with new point
    ↓
Count label updated
```

---

## Threading Model

### **UI Thread (Main)**
- XAML initialization
- User interactions
- UI updates via WPF controls

### **WinTab Thread**
- Tablet packet reception
- Handled via `OnWinTabPacketReceived` callback
- UI updates marshalled to UI thread via `Dispatcher.Invoke()`

### **Serial Port Thread**
- `ReadSerialPortAsync()` runs on ThreadPool
- `Task.Run()` encapsulates blocking serial reads
- Updates marshalled to UI thread via `Dispatcher.Invoke()`

### **Cancellation Token**
- `CancellationTokenSource` controls serial reading
- Cooperative cancellation pattern
- Graceful shutdown on window close

---

## Key Classes

### **AppState**
```csharp
public class AppState
{
    public WinTabSession? WinTabSession { get; set; }
    public ScaleSession? ScaleSession { get; set; }
    public SerialPort? SerialPort { get; set; }
    public PressureRecordCollection? RecordCollection { get; set; }
    // ... additional properties
}
```
**Purpose**: Central state container accessible throughout the application

---

### **ScaleParsedLine** (sealed record)
```csharp
public sealed record ScaleParsedLine(
    string Input, 
    bool Parsed, 
    ScaleRecord? ScaleRecord, 
    string Error
);
```
**Purpose**: Immutable result of parsing a scale reading string

---

### **ScaleRecord** (record class)
```csharp
public record class ScaleRecord
{
    public string Line { get; set; }
    public string ReadingAsString { get; set; }
    public double ReadingAsDouble { get; set; }
}
```
**Purpose**: Measurement reading from digital scale

---

### **PressureRecord** (class)
```csharp
public class PressureRecord
{
    public double PhysicalPressure { get; }
    public double LogicalPressure { get; }
}
```
**Purpose**: Paired reading (physical vs. logical pressure)

---

## Resource Management

### **IDisposable Pattern**
```csharp
protected override void Dispose(bool disposing)
{
    if (disposing)
    {
        appstate?.WinTabSession?.Dispose();
        appstate?.ScaleCts?.Dispose();
        CloseAndDisposeSerialPort();
    }
    base.Dispose(disposing);
}
```

### **Cleanup on Exit**
```csharp
private void MainWindow_Closing(...)
{
    StopWinTabSession();           // Close tablet session
    StopScaleSession();            // Cancel serial reading
    CloseAndDisposeSerialPort();   // Close COM port
    appstate?.WinTabSession?.Dispose();
    appstate?.ScaleCts?.Dispose();
}
```

---

## Error Handling

### **COM Port Exceptions**
```csharp
catch (UnauthorizedAccessException ex) { /* Access denied */ }
catch (System.IO.IOException ex) { /* I/O error */ }
catch (Exception ex) { /* Generic fallback */ }
```

### **Serial Data Parsing**
```csharp
try
{
    sr.ReadingAsDouble = double.Parse(str_force);
}
catch (FormatException)
{
    return new ScaleParsedLine(line, false, null, $"Failed to parse");
}
```

---

## Performance Considerations

1. **Moving Average Filter**: Smooths pressure readings over 200 samples
2. **Queue Management**: Maintains sliding window of 400 logical pressure values
3. **Async Serial Reads**: Prevents UI blocking during I/O
4. **Dispatcher Throttling**: Serial read delay of 10ms prevents UI flooding
5. **Plot Refresh**: Manual refresh only when data updates occur

---

## Future Enhancements

### **Short Term**
- [ ] Add data export to CSV format
- [ ] Implement graph zoom/pan controls
- [ ] Add timestamp to each recording
- [ ] Create test profile templates

### **Medium Term**
- [ ] Implement MVVM pattern
- [ ] Add configuration persistence
- [ ] Create reusable control library
- [ ] Add data analysis algorithms

### **Long Term**
- [ ] Machine learning calibration
- [ ] Multi-pressure point testing
- [ ] Report generation
- [ ] Cloud sync capabilities

---

## Conclusion

The WPF-based architecture provides a clean, maintainable, and extensible foundation for pressure testing and calibration. The separation of concerns and modern C# 14 features enable future enhancements while maintaining code quality.
