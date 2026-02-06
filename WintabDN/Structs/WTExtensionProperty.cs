// See copright.md for copyright information.

using System.Runtime.InteropServices;

//TODO - generics should be used where possible -

namespace WinTabDN.Structs;

/// <summary>
/// Structure for reading/writing non-image Wintab extension data. (Wintab 1.4)
/// </summary>
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct WTExtensionProperty
{
    public WTExtensionPropertyBase extBase;

    /// <summary>
    /// Non-image data being written/read through the extensions API.
    /// A small buffer is sufficient.
    /// </summary>
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = WTExtensionsGlobal.WTExtensionPropertyMaxDataBytes)]
    public byte[] data;
}
// end namespace WintabDN
