namespace SevenUtils.Geometry;

public readonly record struct Size(int Width, int Height)
{

    public SizeD Divide(double scale) => new SizeD(this.Width / scale, this.Height / scale);


}
