using System;
using System.Runtime.InteropServices;

namespace SevenLib.WinInk
{
    [StructLayout(LayoutKind.Sequential)]
    public struct POINTER_INFO
    {
        public int pointerType;
        public uint pointerId;
        public uint frameId;
        public uint pointerFlags;
        public IntPtr sourceDevice;
        public IntPtr hwndTarget;
        public SevenLib.WinInk.NativePoint ptPixelLocation;
        public SevenLib.WinInk.NativePoint ptHimetricLocation;
        public SevenLib.WinInk.NativePoint ptPixelLocationRaw;
        public SevenLib.WinInk.NativePoint ptHimetricLocationRaw;
        public uint dwTime;
        public uint historyCount;
        public int InputData;
        public uint dwKeyStates;
        public ulong PerformanceCount;
        public int ButtonChangeType;
    }

}
