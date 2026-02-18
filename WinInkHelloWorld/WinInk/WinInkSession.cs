namespace SevenLib.WinInk
{
    public class WinInkSession
    {

        public SevenLib.WinInk.PointerState PointerState;

        public System.Action<int, int, Interop.POINTER_PEN_INFO> _PointerPenInfoCallback;
        public System.Action<int, int, Interop.POINTER_INFO> _PointerInfoCallback;
        public System.Action<int, int, SevenLib.Stylus.PointerData> _PointerDataCallback;

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
            _PointerPenInfoCallback?.Invoke(msg, pointerType, penInfo);
            if (_PointerDataCallback != null)
            {
                var pointerdata = create_pointer_data_from_pen_info(penInfo);
                _PointerDataCallback.Invoke(msg, pointerType, pointerdata);
            }
        }

        private void ProcessPointerInfo(int msg, int pointerType, Interop.POINTER_INFO pointerInfo)
        {
            _PointerInfoCallback?.Invoke(msg, pointerType, pointerInfo);
            if (_PointerDataCallback != null) 
            {
                var pointerdata = create_pointer_data_from_pointer_info(pointerInfo);
                _PointerDataCallback.Invoke(msg, pointerType, pointerdata);
            }
        }

        public static Stylus.PointerData create_pointer_data_from_pen_info(Interop.POINTER_PEN_INFO penInfo)
        {
            SevenLib.Stylus.PointerData pointerdata = new Stylus.PointerData();
            pointerdata.Time = System.DateTime.Now;
            pointerdata.DisplayPoint = new SevenLib.Geometry.PointD(penInfo.pointerInfo.ptPixelLocation.X, penInfo.pointerInfo.ptPixelLocation.Y);
            pointerdata.Height = penInfo.pressure == 0 ? 256 : 0; // use pressure to simulate height 
            pointerdata.PressureNormalized = penInfo.pressure / 1024.0f;
            pointerdata.TiltXYDeg = new SevenLib.Trigonometry.TiltXY(penInfo.tiltX, penInfo.tiltY);
            pointerdata.TiltAADeg = pointerdata.TiltXYDeg.ToAA_deg();
            pointerdata.Twist = penInfo.rotation;
            uint buttonState = MapWindowsButtonStates(penInfo.pointerInfo.pointerFlags);
            pointerdata.ButtonState = new SevenLib.Stylus.StylusButtonState(buttonState);
            return pointerdata;
        }

        public static Stylus.PointerData create_pointer_data_from_pointer_info(Interop.POINTER_INFO pointerInfo)
        {
            SevenLib.Stylus.PointerData pointerdata = new Stylus.PointerData();
            pointerdata.Time = System.DateTime.Now;
            pointerdata.DisplayPoint = new SevenLib.Geometry.PointD(pointerInfo.ptPixelLocation.X, pointerInfo.ptPixelLocation.Y);
            pointerdata.Height = 0;
            pointerdata.PressureNormalized = 1.0;
            pointerdata.TiltXYDeg = new SevenLib.Trigonometry.TiltXY(0, 0);
            pointerdata.TiltAADeg = new SevenLib.Trigonometry.TiltAA(0, 90);
            pointerdata.Twist = 0;
            pointerdata.ButtonState = new SevenLib.Stylus.StylusButtonState(0);
            return pointerdata;
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
