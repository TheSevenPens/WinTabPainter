using SD = System.Drawing;

namespace WinTabPainter
{
    public struct ColorARGB
    {
        public readonly ushort Alpha;
        public readonly ushort Red;
        public readonly ushort Green;
        public readonly ushort Blue;
        public ColorARGB(ushort alpha, ushort red, ushort green, ushort blue)
        {
            this.Alpha = alpha;
            this.Red = red;
            this.Green = green;
            this.Blue = blue;
        }

        public SD.Color ToSDColor()
        {
            return SD.Color.FromArgb(this.Alpha, this.Red, this.Green, this.Blue);
        }
    }
}
