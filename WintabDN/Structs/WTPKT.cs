// See copright.md for copyright information.

using System.Runtime.InteropServices;

namespace WinTabDN.Structs;

/// <summary>
/// Managed implementation of Wintab WTPKT typedef.
/// Holds Wintab packet identifier.
/// </summary>
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct WTPKT
{
    [MarshalAs(UnmanagedType.U4)]
    uint value;

    public WTPKT(uint value)
    { this.value = value; }

    public static implicit operator uint(WTPKT pkt_I)
    { return pkt_I.value; }

    public static implicit operator WTPKT(uint value)
    { return new WTPKT(value); }

    public override string ToString()
    { return value.ToString(); }
}
