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
/// Index values for WTInfo wCategory parameter.
/// </summary>
public enum EWTICategoryIndex
{
     WTI_INTERFACE   = 1,
     WTI_STATUS      = 2,
     WTI_DEFCONTEXT  = 3,
     WTI_DEFSYSCTX   = 4,
     WTI_DEVICES     = 100,
     WTI_CURSORS     = 200,
     WTI_EXTENSIONS  = 300,
     WTI_DDCTXS      = 400,
     WTI_DSCTXS      = 500
}
