using System;
using SD=System.Drawing;

namespace WinTabPainter
{
    public class BitmapDocument : IDisposable
    {

        private SD.Graphics _gfx;
        private SD.Bitmap _bmp;

        private SD.Graphics Graphics { get { return this._gfx; } }
        public SD.Bitmap Bitmap { get { return this._bmp; } }

        int _width;
        int _height;
        public int Width { get { return this._width; } }
        public int Height {  get { return this._height; } }


        public BitmapDocument(int width, int height)
        {
            this._bmp = new SD.Bitmap(width, height);
            this._width= width; 
            this._height = height;
            this._gfx = System.Drawing.Graphics.FromImage(this._bmp);
            this._gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        }
        public void Dispose()
        {
            dispose_resources();
        }

        private void dispose_resources()
        {
            if (this._gfx != null)
            {
                this._gfx.Dispose();
            }

            if (this._bmp != null)
            {
                this._bmp.Dispose();
            }
        }

        public void Erase()
        {
            using (var b = new SD.SolidBrush(System.Drawing.Color.White))
            {
                this._gfx.FillRectangle(b, 0, 0, this._bmp.Width, this._bmp.Height);
            }
        }

        public void Save(string filename)
        {
            this._bmp.Save(filename);
        }

        public void FillEllipse(SD.Color color, SD.Rectangle rect)
        {
            using (SD.Brush brush = new SD.SolidBrush(color))
            {
                this._gfx.FillEllipse(brush, rect);

            }
        }

        public void DrawDabCenteredAt(SD.Color color, SD.Point p, SD.Size s)
        {
            var dab_rect_center = p.Subtract(s.Divide(2.0).ToSDSizeWithRounding());
            var rect = new SD.Rectangle(dab_rect_center, s);
            using (SD.Brush brush = new SD.SolidBrush(color))
            {
                this._gfx.FillEllipse(brush, rect);

            }
        }

        public void Load(string filename)
        {
            this.dispose_resources();
            this._bmp = (SD.Bitmap) System.Drawing.Bitmap.FromFile(filename);
            this._width = this._bmp.Width;
            this._height= this._bmp.Height;
            this._gfx = System.Drawing.Graphics.FromImage(this._bmp);
            this._gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        }
    }
}
