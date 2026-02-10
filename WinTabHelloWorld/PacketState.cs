using System;
using System.Windows;

namespace WinTabHelloWorld;

public class PacketState
{
    public WinTab.Structs.WintabPacket Packet { get; set; }
    public DateTime Time { get; set; }
    public Point LocalPoint { get; set; }
}
