using SD = System.Drawing;

namespace WinTabPainter.Painting;

public class BitmapLayer : System.IDisposable
{
    internal SD.Graphics Graphics;
    internal SD.Bitmap Bitmap;

    readonly WinTabUtils.Geometry.Size Size;
    public int Width => this.Size.Width;
    public int Height => this.Size.Height;
    public BitmapLayer(WinTabUtils.Geometry.Size size)
    {
        this.Size = size;
        this.Bitmap = new SD.Bitmap(this.Width, this.Height);
        this.Graphics = System.Drawing.Graphics.FromImage(this.Bitmap);
        this.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
    }

    public BitmapLayer(SD.Bitmap bmp)
    {
        this.Size = new WinTabUtils.Geometry.Size(bmp.Width,bmp.Height);
        this.Bitmap = bmp;
        this.Graphics = System.Drawing.Graphics.FromImage(this.Bitmap);
        this.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
    }


    public void Dispose()
    {
        this.dispose_respources();
    }
    public void dispose_respources()
    {
        if (this.Graphics != null)
        {
            this.Graphics.Dispose();
            this.Graphics = null;
        }

        if (this.Bitmap != null)
        {
            this.Bitmap.Dispose();
            this.Bitmap = null;
        }

    }
}
