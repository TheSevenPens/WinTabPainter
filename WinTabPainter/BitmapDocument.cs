using System;
using System.Drawing;



// References:
// https://github.com/DennisWacom/WintabControl/tree/master/WintabControl
// https://github.com/DennisWacom/InkPlatform/tree/master/WintabDN
// https://developer-docs.wacom.com/docs/icbt/windows/wintab/wintab-basics/
// https://www.nuget.org/packages/WacomSolutionPartner.WintabDotNet
// https://github.com/Wacom-Developer/wacom-device-kit-windows/tree/master/Wintab%20TiltTest

namespace WinTabPainter
{
    public class BitmapDocument : IDisposable
    {

        private Graphics _gfx;
        private Bitmap _bmp;

        private Graphics Graphics { get { return this._gfx; } }
        public Bitmap Bitmap { get { return this._bmp; } }

        int _width;
        int _height;
        public int Width { get { return this._width; } }
        public int Height {  get { return this._height; } }


        public BitmapDocument(int width, int height)
        {
            this._bmp = new Bitmap(width, height);
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
            using (var b = new SolidBrush(System.Drawing.Color.White))
            {
                this._gfx.FillRectangle(b, 0, 0, this._bmp.Width, this._bmp.Height);
            }
        }

        public void Save(string filename)
        {
            this._bmp.Save(filename);
        }

        public void FillEllipse(Color color, Rectangle rect)
        {
            using (Brush brush = new SolidBrush(color))
            {
                this._gfx.FillEllipse(brush, rect);

            }
        }

        public void DrawDabCenteredAt(Color color, Point p, Size s)
        {
            var dab_rect_center = p.Subtract(s.Divide(2.0));
            var rect = new Rectangle(dab_rect_center, s);
            using (Brush brush = new SolidBrush(color))
            {
                this._gfx.FillEllipse(brush, rect);

            }
        }

        public void Load(string filename)
        {
            this.dispose_resources();
            this._bmp = (Bitmap) System.Drawing.Bitmap.FromFile(filename);
            this._width = this._bmp.Width;
            this._height= this._bmp.Height;
            this._gfx = System.Drawing.Graphics.FromImage(this._bmp);
            this._gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        }
    }
}
