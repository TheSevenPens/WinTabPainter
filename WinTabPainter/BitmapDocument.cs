using System;
using System.Drawing;
using SD = System.Drawing;

namespace WinTabPainter
{
    public class BitmapDocument : IDisposable
    {
        // BitmapDocument class intended to encapsulate
        // the logic behind maintaining a bitmap we are painting on
        //
        // Currently uses System.Drawing to implement drawing primitives
        // but this is considered an implementation detail. Ideally
        // it should use something that isn't tied up into Windows GDI
        // and something that could be used in other UI frameworks
        // besides winforms.
        // 
        // Public members reflect the current implementation in its usage of
        // members that use System.Drawing.Bitmap class and Sytem.Drawing.Point
        // and System.Drawing.Rectangle types. Ideally most of these should hide that
        // implementation and use the app's own Geometry primitives
        //
        // There is only a single "layer" in the current implemenation

        
        private SD.Graphics _gfx;
        private SD.Bitmap _bmp;

        private SD.SolidBrush paint_brush;
        private SD.Graphics Graphics { get { return this._gfx; } }
        public SD.Bitmap Bitmap { get { return this._bmp; } }

        int _width;
        int _height;
        public int Width { get { return this._width; } }
        public int Height {  get { return this._height; } }

        ColorARGB DefaultPageColor = new ColorARGB(255,255,255,255);

        public Geometry.Size Size
        {
            get { return new Geometry.Size(this._width,this._height); }
        }
        public BitmapDocument(int width, int height)
        {
            this._bmp = new SD.Bitmap(width, height);
            this._width= width; 
            this._height = height;
            this._gfx = System.Drawing.Graphics.FromImage(this._bmp);
            this._gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            this.paint_brush = new SD.SolidBrush(System.Drawing.Color.Black);
        }
        public void Dispose()
        {
            dispose_resources();
        }

        private void dispose_resources()
        {
            if (this.paint_brush != null)
            {
                this.paint_brush.Dispose();
                this.paint_brush = null;
            }

            if (this._gfx != null)
            {
                this._gfx.Dispose();
                this._gfx = null;
            }

            if (this._bmp != null)
            {
                this._bmp.Dispose();
                this._bmp = null;   
            }
        }

        public void Erase()
        {
            using (var b = new SD.SolidBrush(this.DefaultPageColor))
            {
                this._gfx.FillRectangle(b, 0, 0, this._bmp.Width, this._bmp.Height);
            }
        }

        public void Save(string filename)
        {
            this._bmp.Save(filename);
        }

        public void DrawDabCenteredAt(ColorARGB color, Geometry.Point p, int width)
        {
            int half_width = System.Math.Max(1,width / 2);
            var halfsize = new Geometry.Size(half_width, half_width);
            var dab_rect_center = p.Subtract(halfsize);
            var rect = new SD.Rectangle(dab_rect_center, halfsize );
            // TODO: Currently ignores the brush color
            this._gfx.FillEllipse(this.paint_brush, rect);
        }


        public SD.Brush GetBrushForColor(ColorARGB color)
        {

            var new_brush = new SD.SolidBrush(color);

            return new_brush;
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
