///////////////////////////////////////////////////////////////////////////////
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
    private IntPtr buffer_pointer;
    private bool disposed;
    private Type type;

    public nint BufferPointer
    {
        get => buffer_pointer;
        private set => buffer_pointer = value;
    }

    public UnmanagedBuffer()
    {
    }

    public static UnmanagedBuffer CreateForObject<T>() where T : new()
    {
        var ub = new UnmanagedBuffer();
        var v = new T();
        ub.BufferPointer = WintabDN.Interop.CMemUtils.AllocUnmanagedBuf(v);
        ub.disposed = false;
        ub.type = typeof(T);
        return ub;
    }
    public static UnmanagedBuffer CreateForString()
    {
        var ub = new UnmanagedBuffer();
        ub.BufferPointer = WintabDN.Interop.CMemUtils.AllocUnmanagedBuf(CWintabInfo.MAX_STRING_SIZE);
        ub.disposed = false;
        ub.type = typeof(string);
        return ub;
    }

    public T GetValueObject<T>(int size) where T : new()
    {
        this.assert_type(typeof(T));
        var value = WintabDN.Interop.CMemUtils.MarshalUnmanagedBuf<T>(BufferPointer, size);
        return value;
    }
    public string GetValueString(int size)
    {
        this.assert_type(typeof(string));
        // Strip off final null character before marshalling.
        var s = WintabDN.Interop.CMemUtils.MarshalUnmanagedString(this.BufferPointer, size - 1);
        return s;
    }

    private void assert_type(Type t)
    {
        if (this.type != t)
        {
            throw new System.ArgumentOutOfRangeException("mismatch in types");
        }

    }

    public void MarshallIntoBuffer(object structure)
    {
        Marshal.StructureToPtr(structure, this.BufferPointer, false);
    }

    public T MarshallFromBuffer<T>()
    {
        var value = (T)Marshal.PtrToStructure(this.BufferPointer, typeof(T));
        return value;

    }
    public void Dispose()
    {
        if (this.disposed) return;
        if (this.BufferPointer == IntPtr.Zero) return;
        WintabDN.Interop.CMemUtils.FreeUnmanagedBuf(this.BufferPointer);
    }
}