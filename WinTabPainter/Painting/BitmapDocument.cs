﻿using System.Windows.Forms;
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

    Painting.ColorARGB DefaultPageColor = new Painting.ColorARGB(0,255,255,255);

    public bool AntiAliasing
    {
        get => this.background_layer.Graphics.SmoothingMode == SD.Drawing2D.SmoothingMode.AntiAlias ;
        set => this.background_layer.Graphics.SmoothingMode = value ? System.Drawing.Drawing2D.SmoothingMode.AntiAlias : System.Drawing.Drawing2D.SmoothingMode.None;
    }
    public WinTabUtils.Geometry.Size Size
    {
        get { return new WinTabUtils.Geometry.Size(this._width,this._height); }
    }
    public BitmapDocument(int width, int height)
    {
        this.background_layer = new BitmapLayer(new WinTabUtils.Geometry.Size(width,height));
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
            var old_composting_mode = this.background_layer.Graphics.CompositingMode;
            this.background_layer.Graphics.CompositingMode = SD.Drawing2D.CompositingMode.SourceCopy;
            this.background_layer.Graphics.FillRectangle(b, 0, 0, this.background_layer.Width, this.background_layer.Height);
            this.background_layer.Graphics.CompositingMode = old_composting_mode;

        }
    }

    public void Save(string filename)
    {
        this.background_layer.Bitmap.Save(filename);
    }

    public void DrawDabCenteredAt(Painting.ColorARGB color, WinTabUtils.Geometry.Point p, double width)
    {
        double half_width = System.Math.Max(1.0, width / 2.0);
        var halfsize = new WinTabUtils.Geometry.SizeD(half_width, half_width);
        var fullsize = new WinTabUtils.Geometry.SizeD(width, width);

        var r1 = new SD.RectangleF(p.ToPointD().Subtract(halfsize), fullsize);
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

    public bool Contains(WinTabUtils.Geometry.Point p)
    {
        if (p.X < 0) { return false; }
        if (p.Y < 0) { return false; }
        if (p.X >= this.Width) { return false; }
        if (p.Y >= this.Height) { return false; }
        return true;
    }

    public void CopyToClipboard()
    {
        Clipboard.SetImage(this.background_layer.Bitmap);
    }
}
