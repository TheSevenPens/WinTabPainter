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
using System.Runtime.InteropServices;

//TODO - generics should be used where possible -

namespace WintabDN;

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
