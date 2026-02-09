// See copright.md for copyright information.


namespace WinTab.Enums;

/// <summary>
/// Wintab packet status values.
/// </summary>
public enum EWintabPacketStatusValue
{
    /// <summary>
    /// Specifies that the cursor is out of the context.
    /// </summary>
    TPS_PROXIMITY   = 0x0001,

    /// <summary>
    /// Specifies that the event queue for the context has overflowed.
    /// </summary>
    TPS_QUEUE_ERR   = 0x0002,

    /// <summary>
    /// Specifies that the cursor is in the margin of the context.
    /// </summary>
    TPS_MARGIN      = 0x0004,

    /// <summary>
    /// Specifies that the cursor is out of the context, but that the 
    /// context has grabbed input while waiting for a button release event.
    /// </summary>
    TPS_GRAB        = 0x0008,

    /// <summary>
    /// Specifies that the cursor is in its inverted state.
    /// </summary>
    TPS_INVERT      = 0x0010
}
