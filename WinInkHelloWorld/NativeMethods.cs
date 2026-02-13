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
        public const int POINTER_FLAG_FIRSTBUTTON = 0x0010;
        public const int POINTER_FLAG_SECONDBUTTON = 0x0020;
        public const int POINTER_FLAG_THIRDBUTTON = 0x0040;
        public const int POINTER_FLAG_FOURTHBUTTON = 0x0080;
        public const int POINTER_FLAG_FIFTHBUTTON = 0x0100;
        public const int POINTER_FLAG_PRIMARY = 0x2000;
        public const int POINTER_FLAG_CONFIDENCE = 0x00000400;
        public const int POINTER_FLAG_CANCELED = 0x00000800;
        public const int POINTER_FLAG_DOWN = 0x00010000;
        public const int POINTER_FLAG_UPDATE = 0x00020000;
        public const int POINTER_FLAG_UP = 0x00040000;
        public const int POINTER_FLAG_INRANGE = 0x00080000;
        public const int POINTER_FLAG_INHOVER = 0x00100000;

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
