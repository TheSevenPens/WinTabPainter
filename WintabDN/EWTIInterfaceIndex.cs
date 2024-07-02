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

namespace WintabDN
{
    /// <summary>
    /// Index values for WTI_INTERFACE.
    /// </summary>
    public enum EWTIInterfaceIndex
    {
         IFC_WINTABID       = 1,
         IFC_SPECVERSION    = 2,
         IFC_IMPLVERSION    = 3,
         IFC_NDEVICES       = 4,
         IFC_NCURSORS       = 5,
         IFC_NCONTEXTS      = 6,
         IFC_CTXOPTIONS     = 7,
         IFC_CTXSAVESIZE    = 8,
         IFC_NEXTENSIONS    = 9,
         IFC_NMANAGERS      = 10
    }

}
