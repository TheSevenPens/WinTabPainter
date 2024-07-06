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
/// Wintab event messsages sent to an application.
/// See Wintab Spec 1.4 for a description of these messages.
/// </summary>
public enum EWintabEventMessage
{
    WT_PACKET       = 0x7FF0,
    WT_CTXOPEN      = 0x7FF1,
    WT_CTXCLOSE     = 0x7FF2,
    WT_CTXUPDATE    = 0x7FF3,
    WT_CTXOVERLAP   = 0x7FF4,
    WT_PROXIMITY    = 0x7FF5,
    WT_INFOCHANGE   = 0x7FF6,
    WT_CSRCHANGE    = 0x7FF7,
    WT_PACKETEXT    = 0x7FF8
}
