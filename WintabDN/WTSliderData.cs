﻿///////////////////////////////////////////////////////////////////////////////
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

namespace WintabDN
{
    /// <summary>
    /// Extension data for one touch ring or one touch strip.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct WTSliderData
    {
        /// <summary>
        /// Tablet index where control is found.
        /// </summary>
        public byte nTablet;

        /// <summary>
        /// Zero-based control index.
        /// </summary>
        public byte nControl;

        /// <summary>
        /// Zero-based current active mode of control.
        /// This is the mode selected by control's toggle button.
        /// </summary>
        public byte nMode;

        /// <summary>
        /// Reserved - not used
        /// </summary>
        public byte nReserved;

        /// <summary>
        /// An integer representing the position of the user's finger on the control.
        /// When there is no finger on the control, this value is negative.
        /// </summary>
        public WTPKT nPosition;
    }
}
