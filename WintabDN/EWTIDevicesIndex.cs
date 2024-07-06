///////////////////////////////////////////////////////////////////////////////
// CWintabFuncs.cs - Wintab32 function wrappers for WintabDN
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

namespace WintabDN;

/// <summary>
/// Index values for WTI_DEVICES
/// </summary>
public enum EWTIDevicesIndex
{
     DVC_NAME		    = 1,
     DVC_HARDWARE	    = 2,
     DVC_NCSRTYPES	    = 3,
     DVC_FIRSTCSR	    = 4,
     DVC_PKTRATE		= 5,
     DVC_PKTDATA		= 6,
     DVC_PKTMODE		= 7,
     DVC_CSRDATA		= 8,
     DVC_XMARGIN		= 9,
     DVC_YMARGIN		= 10,
     DVC_ZMARGIN		= 11,
     DVC_X			    = 12,
     DVC_Y			    = 13,
     DVC_Z			    = 14,
     DVC_NPRESSURE	    = 15,
     DVC_TPRESSURE	    = 16,
     DVC_ORIENTATION	= 17,
     DVC_ROTATION	    = 18,
     DVC_PNPID		    = 19
}
