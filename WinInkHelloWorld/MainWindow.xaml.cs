using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace WinInkHelloWorld
{
    public partial class MainWindow : Window
    {
        private SevenLib.Media.CanvasRenderer _renderer;
        private DrawingState _drawingState = new DrawingState();
        private const int CanvasWidth = 600;
        private const int CanvasHeight = 600;

        public MainWindow()
        { // turbo
            // Disable WPF's internal stylus support to prevent it from swallowing WM_POINTER messages
            AppContext.SetSwitch("Switch.System.Windows.Input.Stylus.DisableStylusAndTouchSupport", true);
            
            InitializeComponent();
            InitializeCanvas();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            var source = PresentationSource.FromVisual(this) as HwndSource;
            source?.AddHook(WndProc);

            // Enable mouse to act as a pointer device for testing
            NativeMethods.EnableMouseInPointer(true);
            
            // Disable WPF Stylus features that might interfere
            Stylus.SetIsPressAndHoldEnabled(this, false);
            Stylus.SetIsFlicksEnabled(this, false);
            Stylus.SetIsTapFeedbackEnabled(this, false);
            Stylus.SetIsTouchFeedbackEnabled(this, false);
        }

        private void InitializeCanvas()
        {
            _renderer = new SevenLib.Media.CanvasRenderer(CanvasWidth, CanvasHeight);
            WritingCanvas.Source = _renderer.ImageSource;
        }



        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
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


                        var clientPos = WritingCanvas.PointFromScreen(new Point(penInfo.pointerInfo.ptPixelLocation.X, penInfo.pointerInfo.ptPixelLocation.Y));

                        this._drawingState.PointerData.Time = System.DateTime.Now;
                        this._drawingState.PointerData.DisplayPoint = new SevenLib.Geometry.PointD(penInfo.pointerInfo.ptPixelLocation.X, penInfo.pointerInfo.ptPixelLocation.Y);
                        this._drawingState.PointerData.CanvasPoint = new SevenLib.Geometry.PointD(clientPos.X, clientPos.Y);
                        this._drawingState.PointerData.Height = penInfo.pressure == 0 ? 256 : 0; // use pressure to simulate height 
                        this._drawingState.PointerData.PressureNormalized = penInfo.pressure / 1024.0f;
                        this._drawingState.PointerData.TiltXYDeg = new SevenLib.Trigonometry.TiltXY(penInfo.tiltX, penInfo.tiltY);
                        this._drawingState.PointerData.TiltAADeg = this._drawingState.PointerData.TiltXYDeg.ToAA_deg();
                        this._drawingState.PointerData.Twist = penInfo.rotation;
                        uint buttonState = MapWindowsButtonStates(penInfo);
                        this._drawingState.PointerData.ButtonState = new SevenLib.Stylus.StylusButtonState(buttonState);


                        HandlePenMessage(msg, penInfo);
                        handled = true;
                    }
                    else if (NativeMethods.GetPointerInfo(pointerId, out POINTER_INFO pointerInfo))
                    {
                        // Generic pointer handling

                        var clientPos = WritingCanvas.PointFromScreen(new Point(pointerInfo.ptPixelLocation.X, pointerInfo.ptPixelLocation.Y));

                        this._drawingState.PointerData.Time = System.DateTime.Now;
                        this._drawingState.PointerData.DisplayPoint = new SevenLib.Geometry.PointD(pointerInfo.ptPixelLocation.X, pointerInfo.ptPixelLocation.Y);
                        this._drawingState.PointerData.CanvasPoint = new SevenLib.Geometry.PointD(clientPos.X, clientPos.Y);
                        this._drawingState.PointerData.Height = 0; //
                        this._drawingState.PointerData.PressureNormalized = 1.0;;
                        this._drawingState.PointerData.TiltXYDeg = new SevenLib.Trigonometry.TiltXY(0, 0);
                        this._drawingState.PointerData.TiltAADeg = new SevenLib.Trigonometry.TiltAA(0, 90);
                        this._drawingState.PointerData.Twist = 0;
                        uint buttonState = MapWindowsButtonStates(penInfo);
                        this._drawingState.PointerData.ButtonState = new SevenLib.Stylus.StylusButtonState(buttonState);

                        HandlePointerMessage(msg, pointerInfo, pointerType);
                         handled = true;
                    }
                    else
                    {
                        // Debug log for failure
                        UpdatePointerStats();
                    }
                    break;
            }

            return IntPtr.Zero;
        }

        private static uint MapWindowsButtonStates(POINTER_PEN_INFO penInfo)
        {
            uint buttonState = 0;
            if ((penInfo.pointerInfo.pointerFlags & NativeMethods.POINTER_FLAG_FIRSTBUTTON) != 0) buttonState |= 1; // Tip
            if ((penInfo.pointerInfo.pointerFlags & NativeMethods.POINTER_FLAG_SECONDBUTTON) != 0) buttonState |= 8; // Barrel
            return buttonState;
        }

        private void HandlePenMessage(int msg, POINTER_PEN_INFO penInfo)
        {


            UpdatePointerStats();

            if (msg == NativeMethods.WM_POINTERDOWN)
            {
                _drawingState.IsDrawing = true;
                _drawingState.LastCanvasPoint = _drawingState.PointerData.CanvasPoint;
            }
            else if (msg == NativeMethods.WM_POINTERUP)
            {
                _drawingState.IsDrawing = false;
            }
            else // UPDATE
            {
                bool inContact = (penInfo.pointerInfo.pointerFlags & NativeMethods.POINTER_FLAG_INCONTACT) != 0;
                
                if (_drawingState.IsDrawing && inContact)
                {
                    _renderer.DrawLineX(_drawingState.LastCanvasPoint, _drawingState.PointerData.CanvasPoint, (float)(_drawingState.PointerData.PressureNormalized * 5));
                    _drawingState.LastCanvasPoint = _drawingState.PointerData.CanvasPoint;
                }
            }
        }
        private void HandlePointerMessage(int msg, POINTER_INFO pointerInfo, int ptrType)
        {

            //string deviceName = pointer_type_to_name(ptrType);
            UpdatePointerStats();

            if (msg == NativeMethods.WM_POINTERDOWN)
            {
                HandlePointerDown(this._drawingState.PointerData.CanvasPoint);
            }
            else if (msg == NativeMethods.WM_POINTERUP)
            {
                HandlePointerUp();
            }
            else
            {
                HandlePointerUpdate(pointerInfo, this._drawingState.PointerData.CanvasPoint, this._drawingState.PointerData.PressureNormalized);
            }
        }

        private void HandlePointerUpdate(POINTER_INFO pointerInfo, SevenLib.Geometry.PointD canvasPos, double pressure)
        {
            bool inContact = (pointerInfo.pointerFlags & NativeMethods.POINTER_FLAG_INCONTACT) != 0;

            if (_drawingState.IsDrawing && inContact)
            {
                _renderer.DrawLineX(_drawingState.LastCanvasPoint, canvasPos, (float)(pressure *5));
                _drawingState.LastCanvasPoint = canvasPos;
            }
        }

        private void HandlePointerUp()
        {
            _drawingState.IsDrawing = false;
        }

        private void HandlePointerDown(SevenLib.Geometry.PointD canvasPos)
        {
            _drawingState.IsDrawing = true;
            _drawingState.LastCanvasPoint = canvasPos;
        }



        private static string pointer_type_to_name(int ptrType)
        {
            if (ptrType == (int)POINTER_INPUT_TYPE.PT_MOUSE)
            {
                return "Mouse";
            }
            else if (ptrType == (int)POINTER_INPUT_TYPE.PT_TOUCH)
            {
                return ("Touch");
            }
            else
            {
                return ($"Generic({ptrType})");
            }
        }

        private void UpdatePointerStats()
        {
            string deviceType = "UNK";
            var pos = this._drawingState.PointerData.CanvasPoint;
            var pressure = this._drawingState.PointerData.PressureNormalized;
            var tiltX = this._drawingState.PointerData.TiltXYDeg.X;
            var tiltY = this._drawingState.PointerData.TiltXYDeg.Y;
            var az = this._drawingState.PointerData.TiltAADeg.Azimuth;
            var alt = this._drawingState.PointerData.TiltAADeg.Altitude;
            var btns = this._drawingState.PointerData.ButtonState.ToString();
            StatusText.Text = $"Device: {deviceType} | Pos: {pos.X:F0},{pos.Y:F0} | Press: {pressure:F2} | Tilt: {tiltX},{tiltY} | Az/Alt: {az:F0},{alt:F0} | Btn: {btns}";
        }

    }

}
