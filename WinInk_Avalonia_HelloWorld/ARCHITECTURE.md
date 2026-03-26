# WinInk_Avalonia_HelloWorld Architecture

## Overview

This project demonstrates Windows Ink (WinInk) pen/stylus input in an Avalonia UI application. It receives low-latency pointer messages via Win32 window subclassing and renders pressure-sensitive strokes onto a SkiaSharp-backed bitmap canvas.

## Project Structure

```
WinInk_Avalonia_HelloWorld/
├── Program.cs                  # Entry point, Avalonia AppBuilder configuration
├── App.axaml / App.axaml.cs    # Application root, FluentTheme, window lifecycle
├── MainWindow.axaml            # UI layout: status bar + scrollable canvas
├── MainWindow.axaml.cs         # WinInk integration, drawing logic, coordinate mapping
├── DrawingState.cs             # Thread-safe shared state between message pump and UI
├── WinInk_Avalonia_HelloWorld.csproj
├── app.manifest                # Windows 10 compatibility manifest
└── ARCHITECTURE.md
```

## Dependencies

| Dependency | Purpose |
|---|---|
| **Avalonia 11.3.12** | Cross-platform UI framework (Desktop, Fluent theme, Inter font, Skia renderer) |
| **SkiaSharp 3.119.2** | Direct bitmap drawing for the canvas |
| **SevenLib.WinInk** | WinInk session management, Win32 interop, pointer data extraction |
| **SevenLib** | Shared types: `PointD`, `TiltXY`, `TiltAA`, `StylusButtonState`, `PointerData` |

## How WinInk Integration Works

### Execution Flow

```
Pen touches screen
  -> Windows generates WM_POINTERDOWN / WM_POINTERUPDATE / WM_POINTERUP
  -> Win32 subclass proc in WinInkSession intercepts the message
  -> Extracts pointer ID, calls GetPointerPenInfo() or GetPointerInfo()
  -> Fires callback with raw POINTER_PEN_INFO / POINTER_INFO struct
  -> MainWindow converts to normalized PointerData
  -> Dispatches to UI thread via Dispatcher.UIThread.Post()
  -> Updates DrawingState + draws line segment on SkiaSharp surface
  -> Invalidates the Image control to display the updated bitmap
```

### Window Handle Acquisition

The WinInk API requires a native `HWND` to subclass the window's message pump. In Avalonia, the handle is available after the window opens:

```csharp
protected override void OnOpened(EventArgs e)
{
    base.OnOpened(e);
    var handle = TryGetPlatformHandle();
    if (handle != null)
        _winink_session.AttachToWindow(handle.Handle);
}
```

`WinInkSession.AttachToWindow()` calls `SetWindowSubclass` (comctl32.dll) to intercept pointer messages before Avalonia processes them, and enables `EnableMouseInPointer(true)` so mouse input also generates pointer messages for testing without a stylus.

### Rendering Pipeline

Drawing uses an Avalonia `WriteableBitmap` with direct SkiaSharp access:

1. Lock the bitmap's pixel buffer via `_bitmap.Lock()`
2. Create an `SKSurface` over the raw pixel pointer (`fb.Address`, `fb.RowBytes`)
3. Draw with SkiaSharp (lines, fills, etc.)
4. Dispose the lock (unlocks the buffer)
5. Call `WritingCanvas.InvalidateVisual()` to trigger a repaint

This bypasses Avalonia's drawing API entirely, giving full SkiaSharp control over pixel output.

### Coordinate Mapping

WinInk delivers positions in **screen pixel coordinates**. These must be converted to canvas-relative coordinates:

```csharp
var screenPixelPoint = new PixelPoint((int)screenpoint.X, (int)screenpoint.Y);
var clientPoint = WritingCanvas.PointToClient(screenPixelPoint);
```

### Threading Model

Pointer messages arrive on the Win32 message pump thread. All UI updates and bitmap writes are marshaled to Avalonia's UI thread:

```csharp
Dispatcher.UIThread.Post(() => { /* UI work here */ });
```

`DrawingState` uses locks to safely share `IsDrawing` and `LastCanvasPoint` between threads.

## Key Differences from WPF (WinInkHelloWorld)

| Concern | WPF (WinInkHelloWorld) | Avalonia (this project) |
|---|---|---|
| **HWND access** | `WindowInteropHelper(this).Handle` in `OnSourceInitialized` | `TryGetPlatformHandle().Handle` in `OnOpened` |
| **Stylus conflicts** | Must disable WPF Stylus subsystem (`DisableStylusAndTouchSupport`, etc.) | Not needed; Avalonia has no competing Stylus system |
| **Bitmap type** | WPF `WriteableBitmap` + `BackBuffer` / `Lock` / `Unlock` / `AddDirtyRect` | Avalonia `WriteableBitmap` + `Lock()` returning `ILockedFramebuffer` |
| **Rendering** | Via `SevenLib.Media.CanvasRenderer` (WPF-coupled) | Direct SkiaSharp on locked framebuffer (no WPF dependency) |
| **Image binding** | `WritingCanvas.Source = _renderer.ImageSource` (WPF `ImageSource`) | `WritingCanvas.Source = _bitmap` (Avalonia `IImage`) |
| **Refresh** | Automatic via `AddDirtyRect` + WPF binding | Explicit `WritingCanvas.InvalidateVisual()` |
| **UI thread dispatch** | `Dispatcher.Invoke(() => ...)` | `Dispatcher.UIThread.Post(() => ...)` |
| **Screen-to-client** | `element.PointFromScreen(Point)` | `element.PointToClient(PixelPoint)` |
| **Target framework** | `net10.0-windows` with `<UseWPF>true</UseWPF>` | `net10.0-windows` with Avalonia NuGet packages |
| **XAML format** | `.xaml` (WPF namespace) | `.axaml` (Avalonia namespace) |

## Win32 API Surface

All Win32 interop lives in `SevenLib.WinInk`. The key APIs used:

- **comctl32.dll**: `SetWindowSubclass`, `DefSubclassProc` - window message interception
- **user32.dll**: `EnableMouseInPointer`, `GetPointerPenInfo`, `GetPointerInfo`, `GetPointerType`
- **Messages**: `WM_POINTERDOWN` (0x0246), `WM_POINTERUPDATE` (0x0245), `WM_POINTERUP` (0x0247), `WM_POINTERLEAVE` (0x024A)
- **Structs**: `POINTER_INFO` (base pointer data), `POINTER_PEN_INFO` (pressure 0-1024, tilt, rotation)

## Pressure and Pen Data

| Property | Source | Range |
|---|---|---|
| Position | `POINTER_INFO.ptPixelLocation` | Screen pixels |
| Pressure | `POINTER_PEN_INFO.pressure` | 0-1024, normalized to 0.0-1.0 |
| Tilt X/Y | `POINTER_PEN_INFO.tiltX/tiltY` | -90 to +90 degrees |
| Rotation | `POINTER_PEN_INFO.rotation` | 0-359 degrees |
| Buttons | `POINTER_INFO.pointerFlags` | Bitmask (tip, barrel, eraser) |

Line thickness is `pressure * 5` pixels, drawn with round caps and antialiasing.
