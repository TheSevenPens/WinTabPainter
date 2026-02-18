namespace SevenLib.WinInk
{
    public class WinInkSession
    {
        public System.Action<int, int, Interop.POINTER_PEN_INFO> _PointerPenInfoCallback;
        public System.Action<int, int, Interop.POINTER_INFO> _PointerInfoCallback;

        private System.IntPtr _windowHandle;
        private Interop.SubclassWndProc _subclassProc;

        public WinInkSession()
        {
        }

        public void AttachToWindow(System.IntPtr hwnd)
        {
            _windowHandle = hwnd;

            // Keep the delegate alive as an instance field
            _subclassProc = new Interop.SubclassWndProc(HandleWindowMessage);

            // Subclass the window using Windows API directly (no WPF dependency)
            Interop.NativeMethods.SetWindowSubclass(hwnd, _subclassProc, System.IntPtr.Zero, System.IntPtr.Zero);

            // Enable mouse to act as a pointer device for testing
            Interop.NativeMethods.EnableMouseInPointer(true);
        }

        private System.IntPtr HandleWindowMessage(System.IntPtr hwnd, uint msg, System.IntPtr wParam, System.IntPtr lParam, System.IntPtr uIdSubclass, System.IntPtr dwRefData)
        {
            if (_HandleWndProcPointerMessage((int)msg, wParam))
            {
                return System.IntPtr.Zero;
            }

            // Call the original window procedure
            return Interop.NativeMethods.DefSubclassProc(hwnd, msg, wParam, lParam);
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
                        if (_PointerPenInfoCallback != null) 
                        {
                            _PointerPenInfoCallback(msg, pointerType, penInfo);
                        }

                        return true;
                    }
                    else if (Interop.NativeMethods.GetPointerInfo(pointerId, out Interop.POINTER_INFO pointerInfo))
                    {
                        if (_PointerInfoCallback!=null)
                        {
                            _PointerInfoCallback(msg, pointerType, pointerInfo);
                        }
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
