using System.Windows;

namespace WinInkHelloWorld
{
    public class DrawingState
    {
        public bool IsDrawing { get; set; }
        public Point LastPoint { get; set; }
    }
}
