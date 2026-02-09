// See copright.md for copyright information.


using System.Runtime.InteropServices;

namespace WinTab.Structs;

/// <summary>
/// Extension data for one Express Key.
/// </summary>
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct WTExpKeyData
{
    /// <summary>
    /// Tablet index where control is found.
    /// </summary>
    public byte nTablet;

    /// <summary>
    /// Zero-based control index.
    /// </summary>
    public byte nControl;

    /// <summary>
    /// Zero-based index indicating side of tablet where control found (0 = left, 1 = right).
    /// </summary>
    public byte nLocation;

    /// <summary>
    /// Reserved - not used
    /// </summary>
    public byte nReserved;

    /// <summary>
    /// Indicates Express Key button press (1 = pressed, 0 = released)
    /// </summary>
    public WTPKT nState;
}
