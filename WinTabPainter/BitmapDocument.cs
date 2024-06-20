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

        public readonly int Width;
        public readonly int Height;


        public BitmapDocument(int width, int height)
        {
            this._bmp = new Bitmap(width, height);
            this.Width = width; 
            this.Height = height;
            this._gfx = System.Drawing.Graphics.FromImage(this._bmp);
            this._gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        }
        public void Dispose()
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
    }
}
