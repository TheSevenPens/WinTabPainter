﻿///////////////////////////////////////////////////////////////////////////////
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

namespace WintabDN
{
    /// <summary>
    /// Index values for WTI contexts.
    /// </summary>
    public enum EWTIContextIndex
    {
         CTX_NAME           = 1,
         CTX_OPTIONS		= 2,
         CTX_STATUS		    = 3,
         CTX_LOCKS		    = 4,
         CTX_MSGBASE		= 5,
         CTX_DEVICE		    = 6,
         CTX_PKTRATE		= 7,
         CTX_PKTDATA		= 8,
         CTX_PKTMODE		= 9,
         CTX_MOVEMASK	    = 10,
         CTX_BTNDNMASK	    = 11,
         CTX_BTNUPMASK	    = 12,
         CTX_INORGX		    = 13,
         CTX_INORGY		    = 14,
         CTX_INORGZ		    = 15,
         CTX_INEXTX		    = 16,
         CTX_INEXTY         = 17,
         CTX_INEXTZ		    = 18,
         CTX_OUTORGX		= 19,
         CTX_OUTORGY		= 20,
         CTX_OUTORGZ		= 21,
         CTX_OUTEXTX		= 22,
         CTX_OUTEXTY		= 23,
         CTX_OUTEXTZ		= 24,
         CTX_SENSX		    = 25,
         CTX_SENSY		    = 26,
         CTX_SENSZ		    = 27,
         CTX_SYSMODE		= 28,
         CTX_SYSORGX		= 29,
         CTX_SYSORGY		= 30,
         CTX_SYSEXTX		= 31,
         CTX_SYSEXTY		= 32,
         CTX_SYSSENSX	    = 33,
         CTX_SYSSENSY	    = 34
    }

}
