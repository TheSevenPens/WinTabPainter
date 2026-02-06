using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SevenPaint.View
{
    public class ViewManager
    {
        private readonly ScrollViewer _scrollViewer;
        private readonly ScaleTransform _scaleTransform;
        
        // Zoom Settings
        private int _zoomLevel = 1;
        private const int MinZoom = 1;
        private const int MaxZoom = 20;

        // Panning State
        private bool _isSpaceDown = false;
        private bool _isPanning = false;
        private System.Windows.Point _lastMousePosition;

        // Events
        public event EventHandler<int>? ZoomChanged;

        public bool IsPanning => _isPanning;
        public bool IsSpaceDown => _isSpaceDown;
        public int ZoomLevel => _zoomLevel;

        public ViewManager(ScrollViewer scrollViewer, ScaleTransform scaleTransform)
        {
            _scrollViewer = scrollViewer ?? throw new ArgumentNullException(nameof(scrollViewer));
            _scaleTransform = scaleTransform ?? throw new ArgumentNullException(nameof(scaleTransform));
        }

        // --- Key Handling ---

        public void ProcessKeyDown(System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Space && !_isSpaceDown && !_isPanning)
            {
                _isSpaceDown = true;
                System.Windows.Input.Mouse.OverrideCursor = System.Windows.Input.Cursors.Hand;
            }
        }

        public void ProcessKeyUp(System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Space)
            {
                _isSpaceDown = false;
                
                if (!_isPanning)
                {
                    System.Windows.Input.Mouse.OverrideCursor = null; 
                }
                else
                {
                   _isPanning = false;
                   _scrollViewer.ReleaseMouseCapture();
                   System.Windows.Input.Mouse.OverrideCursor = null; 
                }
            }
        }

        // --- Mouse Handling ---

        public void ProcessPreviewMouseDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            if (_isSpaceDown && e.ChangedButton == System.Windows.Input.MouseButton.Left)
            {
                _isPanning = true;
                _lastMousePosition = e.GetPosition(_scrollViewer);
                _scrollViewer.CaptureMouse();
                System.Windows.Input.Mouse.OverrideCursor = System.Windows.Input.Cursors.ScrollAll; 
                e.Handled = true; 
            }
        }

        public void ProcessPreviewMouseMove(System.Windows.Input.MouseEventArgs e)
        {
            if (_isPanning)
            {
                System.Windows.Point currentPos = e.GetPosition(_scrollViewer);
                double dx = currentPos.X - _lastMousePosition.X;
                double dy = currentPos.Y - _lastMousePosition.Y;
                
                _scrollViewer.ScrollToHorizontalOffset(_scrollViewer.HorizontalOffset - dx);
                _scrollViewer.ScrollToVerticalOffset(_scrollViewer.VerticalOffset - dy);
                
                _lastMousePosition = currentPos;
            }
        }

        public void ProcessPreviewMouseUp(System.Windows.Input.MouseButtonEventArgs e)
        {
            if (_isPanning && e.ChangedButton == System.Windows.Input.MouseButton.Left)
            {
                _isPanning = false;
                _scrollViewer.ReleaseMouseCapture();
                if (_isSpaceDown)
                    System.Windows.Input.Mouse.OverrideCursor = System.Windows.Input.Cursors.Hand; 
                else
                    System.Windows.Input.Mouse.OverrideCursor = null;
            }
        }

        public void ProcessPreviewMouseWheel(System.Windows.Input.MouseWheelEventArgs e)
        {
            if (System.Windows.Input.Keyboard.Modifiers == System.Windows.Input.ModifierKeys.Control)
            {
                int delta = (e.Delta > 0) ? 1 : -1;
                System.Windows.Point mousePos = e.GetPosition(_scrollViewer);
                
                PerformZoom(_zoomLevel + delta, mousePos);

                e.Handled = true;
            }
        }

        // --- Zoom Logic ---

        public void ZoomIn()
        {
            PerformZoom(_zoomLevel + 1);
        }

        public void ZoomOut()
        {
            PerformZoom(_zoomLevel - 1);
        }

        public void ResetZoom()
        {
            PerformZoom(1);
        }

        private void PerformZoom(int newZoom, System.Windows.Point? centerOnViewport = null)
        {
            if (newZoom < MinZoom) newZoom = MinZoom;
            if (newZoom > MaxZoom) newZoom = MaxZoom;
            
            if (newZoom == _zoomLevel) return;

            double viewportW = _scrollViewer.ViewportWidth;
            double viewportH = _scrollViewer.ViewportHeight;
            
            System.Windows.Point center = centerOnViewport ?? new System.Windows.Point(viewportW / 2.0, viewportH / 2.0);

            // Find point in UN-SCALED content space
            double oldScale = _zoomLevel; // Since integer zoom, scale is just zoomLevel
            double scrollX = _scrollViewer.HorizontalOffset;
            double scrollY = _scrollViewer.VerticalOffset;

            double contentX = (scrollX + center.X) / oldScale;
            double contentY = (scrollY + center.Y) / oldScale;

            // Apply New Zoom
            _zoomLevel = newZoom;
            _scaleTransform.ScaleX = _zoomLevel;
            _scaleTransform.ScaleY = _zoomLevel;

            ZoomChanged?.Invoke(this, _zoomLevel);

            // Re-center
            // We need to wait for layout update? 
            // Usually setting ScaleTransform works immediately for layout measure, 
            // but ScrollViewer might need to update its Extent.
            // However, doing it synchronously usually works if we calculate target offset correctly.
            
            double newScrollX = (contentX * _zoomLevel) - center.X;
            double newScrollY = (contentY * _zoomLevel) - center.Y;

            _scrollViewer.ScrollToHorizontalOffset(newScrollX);
            _scrollViewer.ScrollToVerticalOffset(newScrollY);
        }
    }
}
