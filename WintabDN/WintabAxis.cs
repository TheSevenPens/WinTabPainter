///////////////////////////////////////////////////////////////////////////////
// CWintabContext.cs - Wintab context management for WintabDN
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
using System;
using System.Runtime.InteropServices;

namespace WintabDN;

/// <summary>
/// Managed version of AXIS struct.
/// </summary>
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct WintabAxis
{
    /// <summary>
    /// Specifies the minimum value of the data item in the tablet's na-tive coordinates.
    /// </summary>
    public Int32 axMin;

    /// <summary>
    /// Specifies the maximum value of the data item in the tablet's na-tive coordinates.
    /// </summary>
    public Int32 axMax;

    /// <summary>
    /// Indicates the units used in calculating the resolution for the data item.
    /// </summary>
    public UInt32 axUnits;

    /// <summary>
    /// Is a fixed-point number giving the number of data item incre-ments per physical unit.
    /// </summary>
    public FIX32 axResolution;
}

