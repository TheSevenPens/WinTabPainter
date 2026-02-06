// See copright.md for copyright information.


//TODO - generics should be used where possible -

namespace WinTabDN.Enums;

/// <summary>
/// Tablet property values used with WTExtGet and WTExtSet
/// </summary>
public enum EWTExtensionTabletProperty
{
    /// <summary>
    /// number of physical controls on tablet
    /// </summary>
    TABLET_PROPERTY_CONTROLCOUNT = 0,

    /// <summary>
    /// number of functions of control
    /// </summary>
    TABLET_PROPERTY_FUNCCOUNT = 1,

    /// <summary>
    /// control/mode is available for override
    /// </summary>
    TABLET_PROPERTY_AVAILABLE = 2,

    /// <summary>
    /// minimum value of control function
    /// </summary>
    TABLET_PROPERTY_MIN = 3,

    /// <summary>
    /// maximum value of control function
    /// </summary>
    TABLET_PROPERTY_MAX = 4,

    /// <summary>
    /// Indicates control should be overriden
    /// </summary>
    TABLET_PROPERTY_OVERRIDE = 5,

    /// <summary>
    ///  UTF8 encoded displayable name when control is overridden
    /// </summary>
    TABLET_PROPERTY_OVERRIDE_NAME = 6,

    /// <summary>
    /// PNG icon image to shown when control is overriden (supported only tablets with display OLEDs; eg: Intuos4)
    /// </summary>
    TABLET_PROPERTY_OVERRIDE_ICON = 7,

    /// <summary>
    /// Pixel width of icon display
    /// </summary>
    TABLET_PROPERTY_ICON_WIDTH = 8,

    /// <summary>
    /// Pixel height of icon display
    /// </summary>
    TABLET_PROPERTY_ICON_HEIGHT = 9,

    /// <summary>
    /// Pixel format of icon display (see TABLET_ICON_FMT_*)
    /// </summary>
    TABLET_PROPERTY_ICON_FORMAT = 10,

    /// <summary>
    /// Physical location of control (see TABLET_LOC_*)
    /// </summary>
    TABLET_PROPERTY_LOCATION = 11
}
// end namespace WintabDN
