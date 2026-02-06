// See copright.md for copyright information.

using System.Runtime.InteropServices;

namespace WinTabDN.Structs;

/// <summary>
/// Pen Orientation
/// </summary>
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct WTOrientation
{
    /// <summary>
    /// Specifies the clockwise rotation of the cursor about the 
    /// z axis through a full circular range.
    /// </summary>
    public int orAzimuth;

    /// <summary>
    /// Specifies the angle with the x-y plane through a signed, semicircular range.  
    /// Positive values specify an angle upward toward the positive z axis; negative 
    /// values specify an angle downward toward the negative z axis. 
    /// </summary>
    public int orAltitude;

    /// <summary>
    /// Specifies the clockwise rotation of the cursor about its own major axis.
    /// </summary>
    public int orTwist;
}
