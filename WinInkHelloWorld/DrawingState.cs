
namespace WinInkHelloWorld
{
    public class DrawingState
    {
        private SevenLib.Geometry.PointD _lastCanvasPoint;
        private bool _isDrawing;
        private readonly object _lockObj = new object();

        public SevenLib.Geometry.PointD LastCanvasPoint
        {
            get 
            { 
                lock (_lockObj) 
                { 
                    return _lastCanvasPoint; 
                } 
            }
            set 
            { 
                lock (_lockObj) 
                { 
                    _lastCanvasPoint = value; 
                } 
            }
        }

        public bool IsDrawing
        {
            get 
            { 
                lock (_lockObj) 
                { 
                    return _isDrawing; 
                } 
            }
            set 
            { 
                lock (_lockObj) 
                { 
                    _isDrawing = value; 
                } 
            }
        }
    }
}
