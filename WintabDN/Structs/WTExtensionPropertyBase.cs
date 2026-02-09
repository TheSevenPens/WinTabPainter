// See copright.md for copyright information.

using System.Runtime.InteropServices;

//TODO - generics should be used where possible -

namespace WinTab.Structs;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct WTExtensionPropertyBase
{
    /// <summary>
    /// Structure version (reserved: always 0 for now)
    /// </summary>
    public byte version;

    /// <summary>
    /// 0-based index for tablet
    /// </summary>
    public byte tabletIndex;

    /// <summary>
    /// 0-based index for control 
    /// </summary>
    public byte controlIndex;

    /// <summary>
    /// 0-based index for control's sub-function
    /// </summary>
    public byte functionIndex;

    /// <summary>
    /// ID of property being set (see EWTExtensionTabletProperty)
    /// </summary>
    public ushort propertyID;

    /// <summary>
    /// Alignment padding (reserved)
    /// </summary>
    public ushort reserved;

    /// <summary>
    /// Number of bytes in data[] buffer
    /// </summary>
    public uint dataSize;
}
// end namespace WintabDN
