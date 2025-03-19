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
using System.Runtime.InteropServices;

namespace WintabDN.Structs;

//Implementation note: cannot use statement such as:
//      using WTPKT = UInt32;
// because the scope of the statement is this file only.
// Thus we need to implement the 'typedef' using a class that
// implicitly defines the type.  Also remember to make it
// sequential so it won't make marshalling barf.

/// <summary>
/// Managed implementation of Wintab HWND typedef. 
/// Holds native Window handle.
/// </summary>
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct HWND
{
    [MarshalAs(UnmanagedType.I4)]
    public nint value;

    public HWND(nint value)
    { this.value = value; }

    public static implicit operator nint(HWND hwnd_I)
    { return hwnd_I.value; }

    public static implicit operator HWND(nint ptr_I)
    { return new HWND(ptr_I); }

    public static bool operator ==(HWND hwnd1, HWND hwnd2)
    { return hwnd1.value == hwnd2.value; }

    public static bool operator !=(HWND hwnd1, HWND hwnd2)
    { return hwnd1.value != hwnd2.value; }

    public override bool Equals(object obj)
    { return (HWND)obj == this; }

    public override int GetHashCode()
    { return 0; }

}
