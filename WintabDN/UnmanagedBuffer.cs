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
using System.Security.Cryptography.Xml;



namespace WintabDN
{

    public class UnmanagedBuffer : IDisposable
    {
        public IntPtr BufferPointer;
        private bool _disposed;
        public UnmanagedBuffer()
        {
        }

        public static UnmanagedBuffer ForObjectType<T>() where T : new()
        {
            var ub = new UnmanagedBuffer();
            var v = new T();
            ub.BufferPointer = CMemUtils.AllocUnmanagedBuf(v);
            ub._disposed = false;
            return ub;
        }
        public static UnmanagedBuffer ForStringType()
        {
            var ub = new UnmanagedBuffer();
            ub.BufferPointer = CMemUtils.AllocUnmanagedBuf(CWintabInfo.MAX_STRING_SIZE);
            ub._disposed = false;
            return ub;
        }

        public T GetValueObject<T>(int size) where T : new()
        {
            var _value = CMemUtils.MarshalUnmanagedBuf<T>(BufferPointer, size);
            return _value;
        }
        public string GetValueString(int size)
        {
            // Strip off final null character before marshalling.
            var s = CMemUtils.MarshalUnmanagedString(this.BufferPointer, size - 1);
            return s;
        }

        public void Dispose()
        {
            if (this._disposed) return;
            if (this.BufferPointer == IntPtr.Zero) return;
            CMemUtils.FreeUnmanagedBuf(this.BufferPointer);
        }
    }

}
