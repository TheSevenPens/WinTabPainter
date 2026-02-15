using System;
using System.Windows;

namespace WinInkHelloWorld
{
    public class PointerMessageHandler
    {
        private DrawingState _drawingState;
        private SevenLib.Media.CanvasRenderer _renderer;
        private FrameworkElement _writingCanvas;

        public PointerMessageHandler(DrawingState drawingState, SevenLib.Media.CanvasRenderer renderer, FrameworkElement writingCanvas)
        {
            _drawingState = drawingState;
            _renderer = renderer;
            _writingCanvas = writingCanvas;
        }

        public bool HandlePointerMessage(int msg, IntPtr wParam)
        {
            switch (msg)
            {
                case NativeMethods.WM_POINTERDOWN:
                case NativeMethods.WM_POINTERUPDATE:
                case NativeMethods.WM_POINTERUP:
                case NativeMethods.WM_POINTERLEAVE:
                    uint pointerId = NativeMethods.GetPointerId(wParam);

                    int pointerType = 0;
                    NativeMethods.GetPointerType(pointerId, out pointerType);

                    if (NativeMethods.GetPointerPenInfo(pointerId, out POINTER_PEN_INFO penInfo))
                    {
                        // Handling when the pointer is a pen
                        ProcessPenInfo(msg, penInfo);
                        return true;
                    }
                    else if (NativeMethods.GetPointerInfo(pointerId, out POINTER_INFO pointerInfo))
                    {
                        // Generic pointer handling
                        ProcessPointerInfo(msg, pointerType, pointerInfo);
                        return true;
                    }
                    else
                    {
                        // Debug log for failure
                        UpdatePointerStats();
                    }
                    break;
            }

            return false;
        }

        private void ProcessPenInfo(int msg, POINTER_PEN_INFO penInfo)
        {
            var clientPos = _writingCanvas.PointFromScreen(new Point(penInfo.pointerInfo.ptPixelLocation.X, penInfo.pointerInfo.ptPixelLocation.Y));

            _drawingState.PointerData.Time = System.DateTime.Now;
            _drawingState.PointerData.DisplayPoint = new SevenLib.Geometry.PointD(penInfo.pointerInfo.ptPixelLocation.X, penInfo.pointerInfo.ptPixelLocation.Y);
            _drawingState.PointerData.CanvasPoint = new SevenLib.Geometry.PointD(clientPos.X, clientPos.Y);
            _drawingState.PointerData.Height = penInfo.pressure == 0 ? 256 : 0; // use pressure to simulate height 
            _drawingState.PointerData.PressureNormalized = penInfo.pressure / 1024.0f;
            _drawingState.PointerData.TiltXYDeg = new SevenLib.Trigonometry.TiltXY(penInfo.tiltX, penInfo.tiltY);
            _drawingState.PointerData.TiltAADeg = _drawingState.PointerData.TiltXYDeg.ToAA_deg();
            _drawingState.PointerData.Twist = penInfo.rotation;
            uint buttonState = MapWindowsButtonStates(penInfo);
            _drawingState.PointerData.ButtonState = new SevenLib.Stylus.StylusButtonState(buttonState);

            HandlePenMessage(msg);
        }

        private void ProcessPointerInfo(int msg, int pointerType, POINTER_INFO pointerInfo)
        {
            var clientPos = _writingCanvas.PointFromScreen(new Point(pointerInfo.ptPixelLocation.X, pointerInfo.ptPixelLocation.Y));

            _drawingState.PointerData.Time = System.DateTime.Now;
            _drawingState.PointerData.DisplayPoint = new SevenLib.Geometry.PointD(pointerInfo.ptPixelLocation.X, pointerInfo.ptPixelLocation.Y);
            _drawingState.PointerData.CanvasPoint = new SevenLib.Geometry.PointD(clientPos.X, clientPos.Y);
            _drawingState.PointerData.Height = 0;
            _drawingState.PointerData.PressureNormalized = 1.0;
            _drawingState.PointerData.TiltXYDeg = new SevenLib.Trigonometry.TiltXY(0, 0);
            _drawingState.PointerData.TiltAADeg = new SevenLib.Trigonometry.TiltAA(0, 90);
            _drawingState.PointerData.Twist = 0;
            
            // Note: this line in original had a bug - penInfo is undefined in this context
            // Using default button state instead
            _drawingState.PointerData.ButtonState = new SevenLib.Stylus.StylusButtonState(0);

            HandlePointerMessage(msg, pointerType);
        }

        private static uint MapWindowsButtonStates(POINTER_PEN_INFO penInfo)
        {
            uint buttonState = 0;
            if ((penInfo.pointerInfo.pointerFlags & NativeMethods.POINTER_FLAG_FIRSTBUTTON) != 0)
            {
                buttonState |= 1; // Tip
            }
            if ((penInfo.pointerInfo.pointerFlags & NativeMethods.POINTER_FLAG_SECONDBUTTON) != 0)
            {
                buttonState |= 2; // Button2
            }

            return buttonState;
        }

        private void HandlePenMessage(int msg)
        {
            UpdatePointerStats();

            if (msg == NativeMethods.WM_POINTERDOWN)
            {
                HandlePointerDown();
            }
            else if (msg == NativeMethods.WM_POINTERUP)
            {
                HandlePointerUp();
            }
            else // UPDATE
            {
                HandlePointerUpdate();
            }
        }

        private void HandlePointerMessage(int msg, int ptrType)
        {
            UpdatePointerStats();

            if (msg == NativeMethods.WM_POINTERDOWN)
            {
                HandlePointerDown();
            }
            else if (msg == NativeMethods.WM_POINTERUP)
            {
                HandlePointerUp();
            }
            else
            {
                HandlePointerUpdate();
            }
        }

        private void HandlePointerUpdate()
        {
            bool pointer_in_contact = _drawingState.PointerData.PressureNormalized > 0;

            if (_drawingState.IsDrawing && pointer_in_contact)
            {
                _renderer.DrawLineX(_drawingState.LastCanvasPoint, _drawingState.PointerData.CanvasPoint, (float)(_drawingState.PointerData.PressureNormalized * 5));
                _drawingState.LastCanvasPoint = _drawingState.PointerData.CanvasPoint;
            }
        }

        private void HandlePointerUp()
        {
            _drawingState.IsDrawing = false;
        }

        private void HandlePointerDown()
        {
            _drawingState.IsDrawing = true;
            _drawingState.LastCanvasPoint = _drawingState.PointerData.CanvasPoint;
        }

        private void UpdatePointerStats()
        {
            // Note: StatusText update moved to caller since we don't have access to UI controls
            // Caller should handle updating the status text based on _drawingState.PointerData
        }
    }
}
