using System;
using System.Drawing;



// References:
// https://github.com/DennisWacom/WintabControl/tree/master/WintabControl
// https://github.com/DennisWacom/InkPlatform/tree/master/WintabDN
// https://developer-docs.wacom.com/docs/icbt/windows/wintab/wintab-basics/
// https://www.nuget.org/packages/WacomSolutionPartner.WintabDotNet
// https://github.com/Wacom-Developer/wacom-device-kit-windows/tree/master/Wintab%20TiltTest

namespace DemoWinTabPaint1
{
    public class BitmapDocument : IDisposable
    {

        private Graphics _gfx;
        private Bitmap _bmp;

        public Graphics Graphics { get { return this._gfx; } }
        public Bitmap Bitmap { get { return this._bmp; } }

        public BitmapDocument(int width, int height)
        {
            this._bmp = new Bitmap(width, height);
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
    }
}
