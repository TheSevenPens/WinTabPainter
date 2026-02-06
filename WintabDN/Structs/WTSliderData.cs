// See copright.md for copyright information.

using System.Runtime.InteropServices;

namespace WinTabDN.Structs;

/// <summary>
/// Extension data for one touch ring or one touch strip.
/// </summary>
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct WTSliderData
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
    /// Zero-based current active mode of control.
    /// This is the mode selected by control's toggle button.
    /// </summary>
    public byte nMode;

    /// <summary>
    /// Reserved - not used
    /// </summary>
    public byte nReserved;

    /// <summary>
    /// An integer representing the position of the user's finger on the control.
    /// When there is no finger on the control, this value is negative.
    /// </summary>
    public WTPKT nPosition;
}
