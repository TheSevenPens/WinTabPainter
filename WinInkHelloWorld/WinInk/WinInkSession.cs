namespace SevenLib.WinInk
{
    public class WinInkSession
    {

        public SevenLib.WinInk.PointerState PointerState;
        public SevenLib.Stylus.PointerData PointerData;

        public System.Action<int, int, SevenLib.Stylus.PointerData> _PointerUpCallback;
        public System.Action<int, int, SevenLib.Stylus.PointerData> _PointerDownCallback;
        public System.Action<int, int, SevenLib.Stylus.PointerData> _PointerUpdateCallback;

        public WinInkSession()
        {
            this.PointerState= new PointerState();
        }

        public void AttachToWindow(System.Windows.Window window)
        {
            var source = System.Windows.PresentationSource.FromVisual(window) as System.Windows.Interop.HwndSource;
            source?.AddHook(this._WndProc);

            // Enable mouse to act as a pointer device for testing
            SevenLib.WinInk.Interop.NativeMethods.EnableMouseInPointer(true);

            // Disable WPF Stylus features that might interfere
            System.Windows.Input.Stylus.SetIsPressAndHoldEnabled(window, false);
            System.Windows.Input.Stylus.SetIsFlicksEnabled(window, false);
            System.Windows.Input.Stylus.SetIsTapFeedbackEnabled(window, false);
            System.Windows.Input.Stylus.SetIsTouchFeedbackEnabled(window, false);
        }

        private System.IntPtr _WndProc(System.IntPtr hwnd, int msg, System.IntPtr wParam, System.IntPtr lParam, ref bool handled)
        {
            if (_HandleWndProcPointerMessage(msg, wParam))
            {

                handled = true;
                return System.IntPtr.Zero;
            }

            return System.IntPtr.Zero;
        }

        private bool _HandleWndProcPointerMessage(int msg, System.IntPtr wParam)
        {
            switch (msg)
            {
                case Interop.NativeMethods.WM_POINTERDOWN:
                case Interop.NativeMethods.WM_POINTERUPDATE:
                case Interop.NativeMethods.WM_POINTERUP:
                case Interop.NativeMethods.WM_POINTERLEAVE:
                    uint pointerId = Interop.NativeMethods.GetPointerId(wParam);

                    int pointerType = 0;
                    Interop.NativeMethods.GetPointerType(pointerId, out pointerType);

                    if (Interop.NativeMethods.GetPointerPenInfo(pointerId, out Interop.POINTER_PEN_INFO penInfo))
                    {
                        // Handling when the pointer is a pen
                        ProcessPenInfo(msg, pointerType, penInfo);
                        return true;
                    }
                    else if (Interop.NativeMethods.GetPointerInfo(pointerId, out Interop.POINTER_INFO pointerInfo))
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

        private void ProcessPenInfo(int msg, int pointerType, Interop.POINTER_PEN_INFO penInfo)
        {

            this.PointerData.Time = System.DateTime.Now;
            this.PointerData.DisplayPoint = new SevenLib.Geometry.PointD(penInfo.pointerInfo.ptPixelLocation.X, penInfo.pointerInfo.ptPixelLocation.Y);
            this.PointerData.Height = penInfo.pressure == 0 ? 256 : 0; // use pressure to simulate height 
            this.PointerData.PressureNormalized = penInfo.pressure / 1024.0f;
            this.PointerData.TiltXYDeg = new SevenLib.Trigonometry.TiltXY(penInfo.tiltX, penInfo.tiltY);
            this.PointerData.TiltAADeg = this.PointerData.TiltXYDeg.ToAA_deg();
            this.PointerData.Twist = penInfo.rotation;
            uint buttonState = MapWindowsButtonStates(penInfo.pointerInfo.pointerFlags);
            this.PointerData.ButtonState = new SevenLib.Stylus.StylusButtonState(buttonState);

            HandlePenMessage(msg, pointerType, this.PointerData);
        }


        private void ProcessPointerInfo(int msg, int pointerType, Interop.POINTER_INFO pointerInfo)
        {

            this.PointerData.Time = System.DateTime.Now;
            this.PointerData.DisplayPoint = new SevenLib.Geometry.PointD(pointerInfo.ptPixelLocation.X, pointerInfo.ptPixelLocation.Y);
            this.PointerData.Height = 0;
            this.PointerData.PressureNormalized = 1.0;
            this.PointerData.TiltXYDeg = new SevenLib.Trigonometry.TiltXY(0, 0);
            this.PointerData.TiltAADeg = new SevenLib.Trigonometry.TiltAA(0, 90);
            this.PointerData.Twist = 0;          
            this.PointerData.ButtonState = new SevenLib.Stylus.StylusButtonState(0);

            HandlePenMessage(msg, pointerType, this.PointerData);
        }

        private void HandlePenMessage(int msg, int pointerType, SevenLib.Stylus.PointerData pointerdata)
        {
            if (msg == Interop.NativeMethods.WM_POINTERDOWN)
            {
                _PointerDownCallback?.Invoke(msg, pointerType, pointerdata);
            }
            else if (msg == Interop.NativeMethods.WM_POINTERUP)
            {
                _PointerUpdateCallback?.Invoke(msg, pointerType, pointerdata);
            }
            else // UPDATE
            {
                _PointerUpdateCallback?.Invoke(msg, pointerType, pointerdata);
            }
        }

        private static uint MapWindowsButtonStates(uint powerflags)
        {
            uint buttonState = 0;
            if ((powerflags & Interop.NativeMethods.POINTER_FLAG_FIRSTBUTTON) != 0)
            {
                buttonState |= 1; // Tip
            }
            if ((powerflags & Interop.NativeMethods.POINTER_FLAG_SECONDBUTTON) != 0)
            {
                buttonState |= 2; // Button2
            }

            return buttonState;
        }
    }
}
