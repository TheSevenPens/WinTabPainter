// See copright.md for copyright information.


using System.Runtime.InteropServices;

namespace SevenLib.WinTab.Structs;

/// <summary>
/// Wintab extension data packet for one tablet control.
/// The tablet controls for which extension data is available are: Express Key, Touch Ring and Touch Strip controls.
/// Note that tablets will have either Touch Rings or Touch Strips - not both.
/// All tablets have Express Keys.
/// </summary>
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct WintabPacketExt
{
    /// <summary>
    /// Extension control properties common to all control types.
    /// </summary>
    public WTExtensionBase pkBase;

    /// <summary>
    /// Extension data for one Express Key.
    /// </summary>
    public WTExpKeyData pkExpKey;

    /// <summary>
    /// Extension data for one Touch Strip.
    /// </summary>
    public WTSliderData pkTouchStrip;

    /// <summary>
    /// Extension data for one Touch Ring.
    /// </summary>
    public WTSliderData pkTouchRing;

}
