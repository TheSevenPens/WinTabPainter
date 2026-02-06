// See copright.md for copyright information.

using System.Runtime.InteropServices;

namespace WinTabDN.Structs;

/// <summary>
/// Common properties for control extension data transactions.
/// </summary>
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct WTExtensionBase
{
    /// <summary>
    /// Specifies the Wintab context to which these properties apply.
    /// </summary>
    public HCTX nContext;

    /// <summary>
    /// Status of setting/getting properties.
    /// </summary>
    public uint nStatus;

    /// <summary>
    /// Timestamp applied to property transaction.
    /// </summary>
    public WTPKT nTime;

    /// <summary>
    /// Reserved - not used.
    /// </summary>
    public uint nSerialNumber;
}
