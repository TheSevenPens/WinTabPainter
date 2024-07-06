
using SD = System.Drawing;

namespace WinTabPainter.Geometry;

public readonly struct Size
{
    public readonly int Width;
    public readonly int Height;

    public Size(int w, int h)
    {
        this.Width = w;
        this.Height = h;
    }

    public SizeD Divide(double scale) => new SizeD(this.Width / scale, this.Height / scale);


    public static implicit operator SD.Size(Size s) => s.ToSDSize();

    public SD.Size ToSDSize()
    {
        var s = new SD.Size(this.Width, this.Height);
        return s;
    }

    public static bool operator ==(Size lhs, Size rhs) => lhs.Equals(rhs);

    public static bool operator !=(Size lhs, Size rhs) => !(lhs == rhs);
}
