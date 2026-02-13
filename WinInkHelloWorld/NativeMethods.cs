using System;
using System.Runtime.InteropServices;

namespace WinInkHelloWorld
{

    public static class NativeMethods
    {
        public const int WM_POINTERDOWN = 0x0246;
        public const int WM_POINTERUPDATE = 0x0245;
        public const int WM_POINTERUP = 0x0247;
        public const int WM_POINTERLEAVE = 0x024A;
        public const int POINTER_FLAG_INCONTACT = 0x0004;

        public static uint GetPointerId(IntPtr wParam)
        {
            return (uint)(wParam.ToInt64() & 0xFFFF); // LOWORD
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnableMouseInPointer([MarshalAs(UnmanagedType.Bool)] bool fEnable);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetPointerPenInfo(uint pointerId, out POINTER_PEN_INFO penInfo);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetPointerType(uint pointerId, out int pointerType);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetPointerInfo(uint pointerId, out POINTER_INFO pointerInfo);
    }




}
