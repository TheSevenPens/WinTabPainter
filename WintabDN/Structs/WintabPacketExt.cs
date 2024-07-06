///////////////////////////////////////////////////////////////////////////////
// CWintabData.cs - Wintab data management for WintabDN
//
// Copyright (c) 2010, Wacom Technology Corporation
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
using System.Runtime.InteropServices;

namespace WintabDN.Structs;

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
