using System;
using System.Windows;

namespace SevenLib.WinInk
{
    public class WinInkSession
    {
        public SevenLib.Geometry.PointD LastCanvasPoint { get; set; }
        public SevenLib.Stylus.PointerData PointerData;
        
        private FrameworkElement _canvas;
        public Action _onPointerStatsUpdated;
        public Action _PointerUpCallback;
        public Action _PointerDownCallback;
        public Action _PointerUpdateCallback;

        public WinInkSession(FrameworkElement canvas)
        {
            _canvas = canvas;
        }

        public IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (HandlePointerMessage(msg, wParam))
            {
                _onPointerStatsUpdated?.Invoke();
                handled = true;
                return IntPtr.Zero;
            }

            return IntPtr.Zero;
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
                    }
                    break;
            }

            return false;
        }

        private void ProcessPenInfo(int msg, POINTER_PEN_INFO penInfo)
        {
            var canvasPos = _canvas.PointFromScreen(new Point(penInfo.pointerInfo.ptPixelLocation.X, penInfo.pointerInfo.ptPixelLocation.Y));

            this.PointerData.Time = System.DateTime.Now;
            this.PointerData.DisplayPoint = new SevenLib.Geometry.PointD(penInfo.pointerInfo.ptPixelLocation.X, penInfo.pointerInfo.ptPixelLocation.Y);
            this.PointerData.CanvasPoint = new SevenLib.Geometry.PointD(canvasPos.X, canvasPos.Y);
            this.PointerData.Height = penInfo.pressure == 0 ? 256 : 0; // use pressure to simulate height 
            this.PointerData.PressureNormalized = penInfo.pressure / 1024.0f;
            this.PointerData.TiltXYDeg = new SevenLib.Trigonometry.TiltXY(penInfo.tiltX, penInfo.tiltY);
            this.PointerData.TiltAADeg = this.PointerData.TiltXYDeg.ToAA_deg();
            this.PointerData.Twist = penInfo.rotation;
            uint buttonState = MapWindowsButtonStates(penInfo);
            this.PointerData.ButtonState = new SevenLib.Stylus.StylusButtonState(buttonState);

            HandlePenMessage(msg);
        }

        private void ProcessPointerInfo(int msg, int pointerType, POINTER_INFO pointerInfo)
        {
            var clientPos = _canvas.PointFromScreen(new Point(pointerInfo.ptPixelLocation.X, pointerInfo.ptPixelLocation.Y));

            this.PointerData.Time = System.DateTime.Now;
            this.PointerData.DisplayPoint = new SevenLib.Geometry.PointD(pointerInfo.ptPixelLocation.X, pointerInfo.ptPixelLocation.Y);
            this.PointerData.CanvasPoint = new SevenLib.Geometry.PointD(clientPos.X, clientPos.Y);
            this.PointerData.Height = 0;
            this.PointerData.PressureNormalized = 1.0;
            this.PointerData.TiltXYDeg = new SevenLib.Trigonometry.TiltXY(0, 0);
            this.PointerData.TiltAADeg = new SevenLib.Trigonometry.TiltAA(0, 90);
            this.PointerData.Twist = 0;
            
            // Note: this line in original had a bug - penInfo is undefined in this context
            // Using default button state instead
            this.PointerData.ButtonState = new SevenLib.Stylus.StylusButtonState(0);

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

        private void HandlePointerDown()
        {
            _PointerDownCallback?.Invoke();
            this.LastCanvasPoint = this.PointerData.CanvasPoint;
        }

        private void HandlePointerUpdate()
        {
            _PointerUpdateCallback?.Invoke();
        }

        private void HandlePointerUp()
        {
            _PointerUpCallback?.Invoke();
        }

    }
}
