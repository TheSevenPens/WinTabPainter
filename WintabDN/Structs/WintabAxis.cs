// See copright.md for copyright information.

using System.Runtime.InteropServices;

namespace WinTab.Structs;

/// <summary>
/// Managed version of AXIS struct.
/// </summary>
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct WintabAxis
{
    /// <summary>
    /// Specifies the minimum value of the data item in the tablet's na-tive coordinates.
    /// </summary>
    public int axMin;

    /// <summary>
    /// Specifies the maximum value of the data item in the tablet's na-tive coordinates.
    /// </summary>
    public int axMax;

    /// <summary>
    /// Indicates the units used in calculating the resolution for the data item.
    /// </summary>
    public uint axUnits;

    /// <summary>
    /// Is a fixed-point number giving the number of data item incre-ments per physical unit.
    /// </summary>
    public FIX32 axResolution;
}

