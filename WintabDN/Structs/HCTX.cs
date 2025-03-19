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
using System.Runtime.InteropServices;

namespace WintabDN.Structs;

/// <summary>
/// Managed implementation of Wintab HCTX typedef.
/// Holds a Wintab context identifier.
/// </summary>
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct HCTX
{
    [MarshalAs(UnmanagedType.U4)]
    uint value;

    public HCTX(uint value)
    { this.value = value; }

    public static implicit operator uint(HCTX hctx_I)
    { return hctx_I.value; }

    public static implicit operator HCTX(uint value)
    { return new HCTX(value); }

    public static bool operator ==(HCTX hctx, uint value)
    { return hctx.value == value; }

    public static bool operator !=(HCTX hctx, uint value)
    { return hctx.value != value; }

    public override bool Equals(object obj)
    { return (HCTX)obj == this; }

    public override int GetHashCode()
    { return 0; }

    public override string ToString()
    { return value.ToString(); }
}
