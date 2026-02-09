// See copright.md for copyright information.


namespace WinTab.Enums;

/// <summary>
/// Wintab event messsages sent to an application.
/// See Wintab Spec 1.4 for a description of these messages.
/// </summary>
public enum EWintabEventMessage
{
    WT_PACKET       = 0x7FF0,
    WT_CTXOPEN      = 0x7FF1,
    WT_CTXCLOSE     = 0x7FF2,
    WT_CTXUPDATE    = 0x7FF3,
    WT_CTXOVERLAP   = 0x7FF4,
    WT_PROXIMITY    = 0x7FF5,
    WT_INFOCHANGE   = 0x7FF6,
    WT_CSRCHANGE    = 0x7FF7,
    WT_PACKETEXT    = 0x7FF8
}
