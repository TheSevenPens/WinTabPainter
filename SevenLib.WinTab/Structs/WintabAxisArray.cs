// See copright.md for copyright information.

using System.Runtime.InteropServices;

namespace SevenLib.WinTab.Structs;

/// <summary>
/// Array of WintabAxis objects.
/// </summary>
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct WintabAxisArray
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
    public WintabAxis[] array;
}

