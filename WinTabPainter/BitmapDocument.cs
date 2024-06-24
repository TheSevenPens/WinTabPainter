using System;
using System.Drawing;
using SD = System.Drawing;

namespace WinTabPainter
{

    public class BitmapLayer : IDisposable
    {
        internal SD.Graphics _layer_gfx;
        internal SD.Bitmap _layer_bmp;

        public SD.Bitmap Bitmap { get { return this._layer_bmp; } }

        readonly Geometry.Size Size;
        public int Width => this.Size.Width;
        public int Height => this.Size.Height;
        public BitmapLayer(Geometry.Size size)
        {
            this.Size = size;
            this._layer_bmp = new SD.Bitmap(this.Width, this.Height);
            this._layer_gfx = System.Drawing.Graphics.FromImage(this._layer_bmp);
            this._layer_gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        }

        public BitmapLayer(SD.Bitmap bmp)
        {
            this.Size = new Geometry.Size(bmp.Width,bmp.Height);
            this._layer_bmp = new SD.Bitmap(this.Width, this.Height);
            this._layer_gfx = System.Drawing.Graphics.FromImage(this._layer_bmp);
            this._layer_gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        }


        public void Dispose()
        {
            this.dispose_respources();
        }
        public void dispose_respources()
        {
            if (this._layer_gfx != null)
            {
                this._layer_gfx.Dispose();
                this._layer_gfx = null;
            }

            if (this._layer_bmp != null)
            {
                this._layer_bmp.Dispose();
                this._layer_bmp = null;
            }

        }
    }
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

        public BitmapLayer background_layer;

        private SD.SolidBrush paint_brush;

        int _width;
        int _height;
        public int Width => this._width;
        public int Height => this._height;

        ColorARGB DefaultPageColor = new ColorARGB(255,255,255,255);

        public Geometry.Size Size
        {
            get { return new Geometry.Size(this._width,this._height); }
        }
        public BitmapDocument(int width, int height)
        {
            this.background_layer = new BitmapLayer(new Geometry.Size(width,height));
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

            if (this.background_layer != null)
            {
                this.background_layer.Dispose();
                this.background_layer = null;
            }
        }

        public void Erase()
        {
            using (var b = new SD.SolidBrush(this.DefaultPageColor))
            {
                this.background_layer._layer_gfx.FillRectangle(b, 0, 0, this.background_layer.Width, this.background_layer.Height);
            }
        }

        public void Save(string filename)
        {
            this.background_layer.Bitmap.Save(filename);
        }

        public void DrawDabCenteredAt(ColorARGB color, Geometry.Point p, int width)
        {
            int half_width = System.Math.Max(1,width / 2);
            var halfsize = new Geometry.Size(half_width, half_width);
            var dab_rect_center = p.Subtract(halfsize);
            var rect = new SD.Rectangle(dab_rect_center, halfsize );
            // TODO: Currently ignores the brush color
            this.background_layer._layer_gfx.FillEllipse(this.paint_brush, rect);
        }


        public SD.Brush GetBrushForColor(ColorARGB color)
        {

            var new_brush = new SD.SolidBrush(color);

            return new_brush;
        }

        public void Load(string filename)
        {
            this.dispose_resources();
            var bmp = (SD.Bitmap)System.Drawing.Bitmap.FromFile(filename);
            this.background_layer = new BitmapLayer(bmp);
            this._width = background_layer.Width;
            this._height = background_layer.Height;
        }
    }
}
