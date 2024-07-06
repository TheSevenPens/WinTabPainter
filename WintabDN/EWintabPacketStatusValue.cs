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
namespace WintabDN;

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
