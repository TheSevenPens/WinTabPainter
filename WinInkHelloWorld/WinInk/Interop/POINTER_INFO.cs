using System;
using System.Runtime.InteropServices;

namespace SevenLib.WinInk.Interop
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
        public SevenLib.WinInk.Interop.NativePoint ptPixelLocation;
        public SevenLib.WinInk.Interop.NativePoint ptHimetricLocation;
        public SevenLib.WinInk.Interop.NativePoint ptPixelLocationRaw;
        public SevenLib.WinInk.Interop.NativePoint ptHimetricLocationRaw;
        public uint dwTime;
        public uint historyCount;
        public int InputData;
        public uint dwKeyStates;
        public ulong PerformanceCount;
        public int ButtonChangeType;
    }

}
