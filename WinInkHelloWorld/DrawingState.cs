using System;
using System.Windows;

namespace WinInkHelloWorld
{
    public class DrawingState
    {
        public bool IsDrawing { get; set; }
        public SevenLib.Geometry.PointD LastPoint { get; set; }

        public PointerData PointerData;
    }


    public struct PointerData
    {
        // Meta
        public DateTime Time { get; set; }

        // Position

        public SevenLib.Geometry.PointD DisplayPoint { get; set; }
        public SevenLib.Geometry.PointD CanvasPoint { get; set; }
        public double Height { get; set; }
        public double PressureNormalized { get; set; }

        // Orientation (Tilt & Twist)
        public SevenLib.Trigonometry.TiltXY TiltXYDeg { get; set; }
        public SevenLib.Trigonometry.TiltAA TiltAADeg { get; set; }
        public double Twist { get; set; } // Barrel Rotation

        // Buttons
        //public SevenLib.WinTab.Utils.StylusButtonState ButtonState { get; set; }
    }
}
