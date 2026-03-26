# WinTab_Avalonia_HelloWorld Architecture

## Overview

This project demonstrates WinTab (Wintab32) tablet input in an Avalonia UI application. It receives stylus/pen data from graphics tablets via the Wintab API and renders pressure-sensitive brush strokes onto a SkiaSharp-backed bitmap canvas.

## Project Structure

```
WinTab_Avalonia_HelloWorld/
├── Program.cs                  # Entry point, Avalonia AppBuilder configuration
├── App.axaml / App.axaml.cs    # Application root, FluentTheme, window lifecycle
├── MainWindow.axaml            # UI layout: status bar with telemetry + scrollable canvas
├── MainWindow.axaml.cs         # WinTab integration, drawing logic, coordinate mapping
├── WinTab_Avalonia_HelloWorld.csproj
├── app.manifest                # Windows 10 compatibility manifest
└── ARCHITECTURE.md
```

## Dependencies

| Dependency | Purpose |
|---|---|
| **Avalonia 11.3.12** | Cross-platform UI framework (Desktop, Fluent theme, Inter font, Skia renderer) |
| **SkiaSharp 3.119.2** | Direct bitmap drawing for the canvas |
| **SevenLib.WinTab** | WinTab session management, Wintab32.dll interop, packet handling |
| **SevenLib** | Shared types: `PointD`, `TiltXY`, `TiltAA`, `StylusButtonState`, `PointerData` |

## How WinTab Integration Works

### Execution Flow

```
Tablet hardware generates stylus event
  -> Wintab32.dll sends WT_PACKET (0x7FF0) message
  -> SevenLib.WinTab's hidden MessageWindow receives it via WndProc
  -> Extracts packet ID, calls WTPacket() to retrieve WintabPacket struct
  -> Checks button state changes, fires callbacks
  -> MainWindow receives OnWinTabPacketReceived callback
  -> Converts raw WintabPacket to normalized PointerData
  -> Dispatches to UI thread via Dispatcher.UIThread.Post()
  -> Draws pressure-scaled point on SkiaSharp surface
  -> Invalidates the Image control to display updated bitmap
```

### Key Difference from WinInk

WinTab does **not** require the Avalonia window's native HWND. The `SevenLib.WinTab` library creates its own hidden `NativeWindow` (Windows Forms) on a background STA thread to receive Wintab messages. This makes WinTab integration simpler with Avalonia — no platform handle hookup needed.

### Initialization

```csharp
// No HWND needed — WinTab manages its own message window
_wintabsession = new WinTabSession();
_wintabsession.OnButtonStateChanged = HandleButtonStateChange;
_wintabsession.OnWinTabPacketReceived = (packet) =>
{
    var pointerData = _wintabsession.create_pointerdata_from_wintabpacket(packet);
    HandlePointerEvent(pointerData);
};
_wintabsession.Open(TabletContextType.System);
```

### Rendering Pipeline

Drawing uses an Avalonia `WriteableBitmap` with direct SkiaSharp access:

1. Lock the bitmap's pixel buffer via `_bitmap.Lock()`
2. Create an `SKSurface` over the raw pixel pointer (`fb.Address`, `fb.RowBytes`)
3. Draw with SkiaSharp (pressure-scaled rectangles)
4. Dispose the lock
5. Call `CanvasImage.InvalidateVisual()` to trigger a repaint

### UI Update Timer

A `DispatcherTimer` at ~60 FPS updates the status bar with real-time tablet telemetry:
- Screen coordinates (Sx, Sy)
- Canvas coordinates (Cx, Cy)
- Height/Z distance
- Normalized pressure
- Azimuth and altitude tilt angles
- Button state

Values go stale (show "-") after 1 second of no input.

### Coordinate Mapping

WinTab delivers positions in **screen pixel coordinates** (via system context with inverted Y-axis). These are converted to canvas-relative coordinates:

```csharp
var screenPixelPoint = new PixelPoint((int)screenPoint.X, (int)screenPoint.Y);
var clientPoint = CanvasImage.PointToClient(screenPixelPoint);
```

### Threading Model

WinTab packets arrive via `SynchronizationContext.Post()` from the hidden message window's thread. All UI updates and bitmap writes are marshaled to Avalonia's UI thread:

```csharp
Dispatcher.UIThread.Post(() => { /* UI work here */ });
```

## Key Differences from WPF (WinTabHelloWorld)

| Concern | WPF (WinTabHelloWorld) | Avalonia (this project) |
|---|---|---|
| **Bitmap type** | WPF `WriteableBitmap` + `BackBuffer` / `Lock` / `Unlock` / `AddDirtyRect` | Avalonia `WriteableBitmap` + `Lock()` returning `ILockedFramebuffer` |
| **Rendering** | Via `SevenLib.Media.CanvasRenderer` (WPF-coupled) | Direct SkiaSharp on locked framebuffer |
| **Image binding** | `CanvasImage.Source = _renderer.ImageSource` (WPF `ImageSource`) | `CanvasImage.Source = _bitmap` (Avalonia `IImage`) |
| **Refresh** | Automatic via `AddDirtyRect` + WPF binding | Explicit `CanvasImage.InvalidateVisual()` |
| **UI thread dispatch** | `Dispatcher.Invoke(() => ...)` | `Dispatcher.UIThread.Post(() => ...)` |
| **Screen-to-client** | `element.PointFromScreen(Point)` | `element.PointToClient(PixelPoint)` |
| **Timer** | WPF `DispatcherTimer` | Avalonia `DispatcherTimer` (same API, different namespace) |
| **Window closing** | `CancelEventArgs` | `WindowClosingEventArgs` |
| **XAML format** | `.xaml` (WPF namespace) | `.axaml` (Avalonia namespace) |
| **Target framework** | `net10.0-windows` with `<UseWPF>true</UseWPF>` | `net10.0-windows` with Avalonia NuGet packages |

## WinTab API Surface

All Wintab interop lives in `SevenLib.WinTab`. Key components:

- **Wintab32.dll**: `WTOpenA`, `WTClose`, `WTPacket`, `WTInfoA`, `WTEnable`, `WTQueueSizeGet/Set`
- **Messages**: `WT_PACKET` (0x7FF0), `WT_PACKETEXT` (0x7FF8)
- **Context types**: System (screen-mapped) or Digitizer (raw tablet coordinates)
- **Hidden window**: `MessageWindow` (NativeWindow subclass) on background STA thread

## Tablet Data

| Property | Source | Range |
|---|---|---|
| Position X/Y | `WintabPacket.pkX/pkY` | Screen pixels (system context) |
| Height Z | `WintabPacket.pkZ` | Device-dependent |
| Pressure | `WintabPacket.pkNormalPressure` | 0 to `MaxPressure`, normalized to 0.0-1.0 |
| Azimuth | `WintabPacket.pkOrientation.orAzimuth` | 0-3600 (divide by 10 for degrees) |
| Altitude | `WintabPacket.pkOrientation.orAltitude` | -900 to +900 (divide by 10 for degrees) |
| Twist | `WintabPacket.pkOrientation.orTwist` | Device-dependent |
| Buttons | `WintabPacket.pkButtons` | Bitmask (tip, lower, upper, barrel) |

Brush size is `pressure * 15` pixels, drawn as filled rectangles.

## Cleanup

On window close, `WinTabSession.Close()` is called to release the Wintab context and shut down the hidden message window. The UI timer is also stopped.
