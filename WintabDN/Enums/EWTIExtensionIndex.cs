// See copright.md for copyright information.


//TODO - generics should be used where possible -

namespace WintabDN.Enums;

/// <summary>
/// Index values used for WTI extensions.
/// For more information, see Wintab 1.4.
/// </summary>
public enum EWTIExtensionIndex
{
    /// <summary>
    /// Get a unique, null-terminated string describing the extension.
    /// </summary>
    EXT_NAME = 1,

    /// <summary>
    /// Get a unique identifier for the extension.
    /// </summary>
    EXT_TAG = 2,

    /// <summary>
    /// Get a mask that can be bitwise OR'ed with WTPKT-type variables to select the extension.
    /// </summary>
    EXT_MASK = 3,

    /// <summary>
    /// Get an array of two UINTs specifying the extension's size within a packet (in bytes). The first is for absolute mode; the second is for relative mode.
    /// </summary>
    EXT_SIZE = 4,

    /// <summary>
    /// Get an array of axis descriptions, as needed for the extension.
    /// </summary>
    EXT_AXES = 5,

    /// <summary>
    /// get the current global default data, as needed for the extension. 
    /// </summary>
    EXT_DEFAULT = 6,

    /// <summary>
    /// Get the current default digitizing context-specific data, as needed for the extension.
    /// </summary>
    EXT_DEFCONTEXT = 7,

    /// <summary>
    /// Get the current default system context-specific data, as needed for the extension.
    /// </summary>
    EXT_DEFSYSCTX = 8,

    /// <summary>
    /// Get a byte array of the current default cursor-specific data, as need for the extension. 
    /// </summary>
    EXT_CURSORS = 9,

    /// <summary>
    /// Allow 100 cursors
    /// </summary>
    EXT_DEVICES = 110,

    /// <summary>
    /// Allow 100 devices
    /// </summary>
    EXT_MAX = 210   // Allow 100 devices
}
// end namespace WintabDN
