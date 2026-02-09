// See copright.md for copyright information.

using System.Runtime.InteropServices;

//TODO - generics should be used where possible -

namespace WinTab.Structs;

/// <summary>
/// Structure read/writing image Wintab extension data. (Wintab 1.4)
/// </summary>
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct WTExtensionImageProperty
{
    public WTExtensionPropertyBase extBase;

    /// <summary>
    /// Image data being written through the extensions API.
    /// A large buffer is needed.
    /// </summary>
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = WTExtensionsGlobal.WTExtensionPropertyImageMaxDataBytes)]
    public byte[] data;
}
// end namespace WintabDN
