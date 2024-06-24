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

        
        private SD.Graphics _final_gfx;
        private SD.Bitmap _final_bmp;

        private SD.SolidBrush paint_brush;
        public SD.Bitmap RenderedDocumentBitmap { get { return this._final_bmp; } }

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
            this._final_bmp = new SD.Bitmap(width, height);
            this._final_gfx = System.Drawing.Graphics.FromImage(this._final_bmp);
            this._final_gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;


            this._width= width; 
            this._height = height;
            this.paint_brush = new SD.SolidBrush(SD.Color.Black);
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

            if (this._final_gfx != null)
            {
                this._final_gfx.Dispose();
                this._final_gfx = null;
            }

            if (this._final_bmp != null)
            {
                this._final_bmp.Dispose();
                this._final_bmp = null;   
            }
        }

        public void Erase()
        {
            using (var b = new SD.SolidBrush(this.DefaultPageColor))
            {
                this._final_gfx.FillRectangle(b, 0, 0, this._final_bmp.Width, this._final_bmp.Height);
            }
        }

        public void Save(string filename)
        {
            this._final_bmp.Save(filename);
        }

        public void DrawDabCenteredAt(ColorARGB color, Geometry.Point p, int width)
        {
            int half_width = System.Math.Max(1,width / 2);
            var halfsize = new Geometry.Size(half_width, half_width);
            var dab_rect_center = p.Subtract(halfsize);
            var rect = new SD.Rectangle(dab_rect_center, halfsize );
            // TODO: Currently ignores the brush color
            this._final_gfx.FillEllipse(this.paint_brush, rect);
        }


        public SD.Brush GetBrushForColor(ColorARGB color)
        {

            var new_brush = new SD.SolidBrush(color);

            return new_brush;
        }

        public void Load(string filename)
        {
            this.dispose_resources();
            this._final_bmp = (SD.Bitmap) System.Drawing.Bitmap.FromFile(filename);
            this._width = this._final_bmp.Width;
            this._height= this._final_bmp.Height;
            this._final_gfx = System.Drawing.Graphics.FromImage(this._final_bmp);
            this._final_gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        }
    }
}
