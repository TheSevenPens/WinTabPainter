// See copright.md for copyright information.

using System.Runtime.InteropServices;

namespace WinTabDN.Structs;

/// <summary>
/// Pen Rotation
/// </summary>
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct WTRotation
{
    /// <summary>
    /// Specifies the pitch of the cursor.
    /// </summary>
    public int rotPitch;

    /// <summary>
    /// Specifies the roll of the cursor. 
    /// </summary>
    public int rotRoll;

    /// <summary>
    /// Specifies the yaw of the cursor.
    /// </summary>
    public int rotYaw;
}
