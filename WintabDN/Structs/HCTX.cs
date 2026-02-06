// See copright.md for copyright information.

using System.Runtime.InteropServices;

namespace WintabDN.Structs;

/// <summary>
/// Managed implementation of Wintab HCTX typedef.
/// Holds a Wintab context identifier.
/// </summary>
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct HCTX
{
    [MarshalAs(UnmanagedType.U4)]
    uint value;

    public HCTX(uint value)
    { this.value = value; }

    public static implicit operator uint(HCTX hctx_I)
    { return hctx_I.value; }

    public static implicit operator HCTX(uint value)
    { return new HCTX(value); }

    public static bool operator ==(HCTX hctx, uint value)
    { return hctx.value == value; }

    public static bool operator !=(HCTX hctx, uint value)
    { return hctx.value != value; }

    public override bool Equals(object obj)
    { return (HCTX)obj == this; }

    public override int GetHashCode()
    { return 0; }

    public override string ToString()
    { return value.ToString(); }
}
