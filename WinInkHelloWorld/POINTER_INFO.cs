using System;
using System.Runtime.InteropServices;

namespace WinInkHelloWorld
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
        public NativePoint ptPixelLocation;
        public NativePoint ptHimetricLocation;
        public NativePoint ptPixelLocationRaw;
        public NativePoint ptHimetricLocationRaw;
        public uint dwTime;
        public uint historyCount;
        public int InputData;
        public uint dwKeyStates;
        public ulong PerformanceCount;
        public int ButtonChangeType;
    }

}
