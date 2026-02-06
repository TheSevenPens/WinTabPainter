// See copright.md for copyright information.

using System.Runtime.InteropServices;

namespace WinTabDN.Structs;

/// <summary>
/// Managed implementation of Wintab FIX32 typedef.
/// Used for a fixed-point arithmetic value.
/// </summary>
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct FIX32
{
    [MarshalAs(UnmanagedType.U4)]
    uint value;

    public FIX32(uint value)
    { this.value = value; }

    public static implicit operator uint(FIX32 fix32_I)
    { return fix32_I.value; }

    public static implicit operator FIX32(uint value)
    { return new FIX32(value); }

    public override string ToString()
    { return value.ToString(); }
}
