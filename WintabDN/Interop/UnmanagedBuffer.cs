﻿///////////////////////////////////////////////////////////////////////////////
// CWintabInfo.cs - Wintab information access for WintabDN
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

namespace WintabDN.Interop;

public class UnmanagedBuffer : IDisposable
{
    private bool disposed;

    private IntPtr buffer_pointer;
    private Type expected_type;

    public nint Pointer
    {
        get => buffer_pointer;
        private set => buffer_pointer = value;
    }

    protected UnmanagedBuffer()
    {
    }

    public static UnmanagedBuffer CreateForSize<T>(int bufsize) where T : new()
    {
        var buf = new UnmanagedBuffer();
        buf.Pointer = UnmanagedBuffer.AllocUnmanagedBufSize(bufsize);
        buf.disposed = false;
        buf.expected_type = null;
        return buf;
    }

    public static UnmanagedBuffer CreateForObject<T>() where T : new()
    {
        var buf = new UnmanagedBuffer();
        var temp_value = new T();
        buf.Pointer = UnmanagedBuffer.AllocUnmanagedBuf(temp_value);
        buf.disposed = false;
        buf.expected_type = typeof(T);
        return buf;
    }
    public static UnmanagedBuffer CreateForString()
    {
        var buf = new UnmanagedBuffer();
        buf.Pointer = WintabDN.Interop.CMemUtils.AllocUnmanagedBuf(CWintabInfo.MAX_STRING_SIZE);
        buf.disposed = false;
        buf.expected_type = typeof(string);
        return buf;
    }

    public T GetObjectFromBuffer<T>(int size) where T : new()
    {
        this.assert_type(typeof(T));
        var value = WintabDN.Interop.CMemUtils.MarshalBufferToObject<T>(Pointer, size);
        return value;
    }
    public string GetStringFromBuffer(int size)
    {
        this.assert_type(typeof(string));
        // Strip off final null character before marshalling.
        var s = WintabDN.Interop.CMemUtils.MarshalBufferToString(this.Pointer, size - 1);
        return s;
    }

    private void assert_type(Type t)
    {
        if (this.expected_type != t)
        {
            throw new System.ArgumentOutOfRangeException("mismatch in types");
        }

    }

    public void MarshallIntoBuffer(object structure)
    {
        this.assert_type(structure.GetType());
        System.Runtime.InteropServices.Marshal.StructureToPtr(structure, this.Pointer, false);
    }

    public T MarshallFromBuffer<T>()
    {
        this.assert_type(typeof(T));
        var value = (T)System.Runtime.InteropServices.Marshal.PtrToStructure(this.Pointer, typeof(T));
        return value;

    }
    public void Dispose()
    {
        if (this.disposed) return;
        if (this.Pointer == IntPtr.Zero) return;
        WintabDN.Interop.CMemUtils.FreeUnmanagedBuf(this.Pointer);
    }


    ///internals
    ///
    /// <summary>
    /// Allocates a pointer to unmanaged heap memory of sizeof(val_I).
    /// </summary>
    /// <param name="val_I">managed object that determines #bytes of unmanaged buf</param>
    /// <returns>Unmanaged buffer pointer.</returns>
    private static IntPtr AllocUnmanagedBuf(Object val_I)
    {
        IntPtr buf = IntPtr.Zero;
        int numBytes = Marshal.SizeOf(val_I);
        buf = Marshal.AllocHGlobal(numBytes);
        return buf;
    }

    /// <returns>Unmanaged buffer pointer.</returns>
    private static IntPtr AllocUnmanagedBufSize(int size_I)
    {
        IntPtr buf = IntPtr.Zero;
        buf = Marshal.AllocHGlobal(size_I);
        return buf;
    }
}