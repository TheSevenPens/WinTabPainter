using SD = System.Drawing;

namespace WinTabPainter.Painting;

public class BitmapDocument : System.IDisposable
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

    Painting.ColorARGB DefaultPageColor = new Painting.ColorARGB(255,255,255,255);

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
        dispose_all_resources();
    }

    private void dispose_all_resources()
    {
        if (this.paint_brush != null)
        {
            this.paint_brush.Dispose();
            this.paint_brush = null;
        }

        dispose_bitmap_resources();
    }

    private void dispose_bitmap_resources()
    {
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
            this.background_layer.Graphics.FillRectangle(b, 0, 0, this.background_layer.Width, this.background_layer.Height);
        }
    }

    public void Save(string filename)
    {
        this.background_layer.Bitmap.Save(filename);
    }

    public void DrawDabCenteredAt(Painting.ColorARGB color, Geometry.Point p, int width)
    {
        int half_width = System.Math.Max(1, width / 2);
        var halfsize = new Geometry.Size(half_width, half_width);
        var fullsize = new Geometry.Size(width, width);

        var r1 = new SD.Rectangle(p.Subtract(halfsize), fullsize);
        this.background_layer.Graphics.FillEllipse(this.paint_brush, r1);
    }

    public SD.Brush GetBrushForColor(Painting.ColorARGB color)
    {

        var new_brush = new SD.SolidBrush(color);

        return new_brush;
    }

    public void Load(string filename)
    {
        this.dispose_bitmap_resources();
        var bmp = (SD.Bitmap)System.Drawing.Bitmap.FromFile(filename);
        this.background_layer = new BitmapLayer(bmp);
        this._width = background_layer.Width;
        this._height = background_layer.Height;
    }

    public bool Contains(Geometry.Point p)
    {
        if (p.X < 0) { return false; }
        if (p.Y < 0) { return false; }
        if (p.X >= this.Width) { return false; }
        if (p.Y >= this.Height) { return false; }
        return true;
    }
}
