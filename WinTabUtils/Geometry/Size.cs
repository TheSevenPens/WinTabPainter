
using SD = System.Drawing;

namespace WinTabUtils.Geometry;

public readonly record struct Size( int Width, int Height )
{

    public SizeD Divide(double scale) => new SizeD(this.Width / scale, this.Height / scale);

    public SD.Size ToSDSize()
    {
        var s = new SD.Size(this.Width, this.Height);
        return s;
    }

}
