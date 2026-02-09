// See copright.md for copyright information.

using System;
using System.Runtime.InteropServices;

namespace WinTab.Structs;

//Implementation note: cannot use statement such as:
//      using WTPKT = UInt32;
// because the scope of the statement is this file only.
// Thus we need to implement the 'typedef' using a class that
// implicitly defines the type.  Also remember to make it
// sequential so it won't make marshalling barf.

/// <summary>
/// Managed implementation of Wintab HWND typedef. 
/// Holds native Window handle.
/// </summary>
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct HWND
{
    [MarshalAs(UnmanagedType.I4)]
    public nint value;

    public HWND(nint value)
    { this.value = value; }

    public static implicit operator nint(HWND hwnd_I)
    { return hwnd_I.value; }

    public static implicit operator HWND(nint ptr_I)
    { return new HWND(ptr_I); }

    public static bool operator ==(HWND hwnd1, HWND hwnd2)
    { return hwnd1.value == hwnd2.value; }

    public static bool operator !=(HWND hwnd1, HWND hwnd2)
    { return hwnd1.value != hwnd2.value; }

    public override bool Equals(object obj)
    { return (HWND)obj == this; }

    public override int GetHashCode()
    { return 0; }

}
