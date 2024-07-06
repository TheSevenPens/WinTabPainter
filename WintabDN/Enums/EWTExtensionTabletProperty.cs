///////////////////////////////////////////////////////////////////////////////
// CWintabExtensions.cs - Wintab extensions access for WintabDN
//
// Copyright (c) 2013, Wacom Technology Corporation
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
///////////////////////////////////////////////////////////////////////////////
//TODO - generics should be used where possible -

namespace WintabDN.Enums;

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
