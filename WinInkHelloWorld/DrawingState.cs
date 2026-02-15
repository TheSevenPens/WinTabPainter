namespace WinInkHelloWorld
{
    public class DrawingState
    {
        public bool IsDrawing { get; set; }
        public SevenLib.Geometry.PointD LastCanvasPoint { get; set; }

        public SevenLib.Stylus.PointerData PointerData;
    }
}
