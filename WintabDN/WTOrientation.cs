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
using System;
using System.Runtime.InteropServices;

namespace WintabDN
{
    /// <summary>
    /// Pen Orientation
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct WTOrientation
    {
        /// <summary>
        /// Specifies the clockwise rotation of the cursor about the 
        /// z axis through a full circular range.
        /// </summary>
        public Int32 orAzimuth;

        /// <summary>
        /// Specifies the angle with the x-y plane through a signed, semicircular range.  
        /// Positive values specify an angle upward toward the positive z axis; negative 
        /// values specify an angle downward toward the negative z axis. 
        /// </summary>
        public Int32 orAltitude;

        /// <summary>
        /// Specifies the clockwise rotation of the cursor about its own major axis.
        /// </summary>
        public Int32 orTwist;
    }
}
