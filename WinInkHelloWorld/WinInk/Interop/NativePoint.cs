using System.Runtime.InteropServices;

namespace SevenLib.WinInk.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    public struct NativePoint 
    {
        public int X;
        public int Y;
        public NativePoint(int x, int y) { X = x; Y = y; }
    }

}
