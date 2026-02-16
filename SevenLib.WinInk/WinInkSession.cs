File: SevenLib.WinInk\WinInkSession.cs
````````csharp
        public WinInkSession()
        {
            this.PointerState = new PointerState();
            this.PointerData = new SevenLib.Stylus.PointerDataNew();
        }

        private void ProcessPenInfo(int msg, Interop.POINTER_PEN_INFO penInfo)
        {
            var canvasPos = _canvas.PointFromScreen(new Point(penInfo.pointerInfo.ptPixelLocation.X, penInfo.pointerInfo.ptPixelLocation.Y));

            this.PointerData.Time = System.DateTime.Now;
            this.PointerData.DisplayPoint = new SevenLib.Geometry.PointD(penInfo.pointerInfo.ptPixelLocation.X, penInfo.pointerInfo.ptPixelLocation.Y);
            this.PointerData.CanvasPoint = new SevenLib.Geometry.PointD(canvasPos.X, canvasPos.Y);
            this.PointerData.Height = penInfo.pressure == 0 ? 256 : 0;
            this.PointerData.PressureNormalized = penInfo.pressure / 1024.0f;
            this.PointerData.TiltXYDeg = new SevenLib.Trigonometry.TiltXY(penInfo.tiltX, penInfo.tiltY);
            this.PointerData.TiltAADeg = this.PointerData.TiltXYDeg.ToAA_deg();
            this.PointerData.Twist = penInfo.rotation;
            uint buttonState = MapWindowsButtonStates(penInfo);
            this.PointerData.ButtonState = new SevenLib.Stylus.StylusButtonState(buttonState);

            HandlePenMessage(msg, this.PointerData);
        }

        private void ProcessPointerInfo(int msg, int pointerType, Interop.POINTER_INFO pointerInfo)
        {
            var canvasPos = _canvas.PointFromScreen(new Point(pointerInfo.ptPixelLocation.X, pointerInfo.ptPixelLocation.Y));

            this.PointerData.Time = System.DateTime.Now;
            this.PointerData.DisplayPoint = new SevenLib.Geometry.PointD(pointerInfo.ptPixelLocation.X, pointerInfo.ptPixelLocation.Y);
            this.PointerData.CanvasPoint = new SevenLib.Geometry.PointD(canvasPos.X, canvasPos.Y);
            this.PointerData.Height = 0;
            this.PointerData.PressureNormalized = 1.0;
            this.PointerData.TiltXYDeg = new SevenLib.Trigonometry.TiltXY(0, 0);
            this.PointerData.TiltAADeg = new SevenLib.Trigonometry.TiltAA(0, 90);
            this.PointerData.Twist = 0;
            this.PointerData.ButtonState = new SevenLib.Stylus.StylusButtonState(0);

            HandlePointerMessage(msg, pointerType, this.PointerData);
        }

        private void HandlePointerDown(SevenLib.Stylus.PointerDataNew pointerdata)
        {
            _canvas.Dispatcher.Invoke(() =>
            {
                this.PointerState.LastCanvasPoint = pointerdata.CanvasPoint;
                _PointerDownCallback?.Invoke(pointerdata);
            });
        }

        private void HandlePointerUpdate(SevenLib.Stylus.PointerDataNew pointerdata)
        {
            _canvas.Dispatcher.Invoke(() =>
            {
                _PointerUpdateCallback?.Invoke(pointerdata);
            });
        }

        private void HandlePointerUp(SevenLib.Stylus.PointerDataNew pointerdata)
        {
            _canvas.Dispatcher.Invoke(() =>
            {
                _PointerUpCallback?.Invoke(pointerdata);
            });
        }   