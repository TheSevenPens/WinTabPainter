using System.Runtime.InteropServices;

namespace WinInkHelloWorld.WinInkLib
{
    [StructLayout(LayoutKind.Sequential)]
    public struct NativePoint 
    {
        public int X;
        public int Y;
        public NativePoint(int x, int y) { X = x; Y = y; }
    }

}
