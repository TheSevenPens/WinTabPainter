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



namespace WintabDN
{
    public class UnmanagedBuffer<T> : IDisposable where T : new()
    {
        public IntPtr BufferPointer;
        private T _value;
        private bool _disposed;

        public UnmanagedBuffer()
        {
            this._value = new T();
            this.BufferPointer = CMemUtils.AllocUnmanagedBuf(this._value);
            this._disposed = false;
        }

        public T GetValue(int size)
        {
            this._value = CMemUtils.MarshalUnmanagedBuf<T>(BufferPointer, size);
            return this._value;
        }

        public void Dispose()
        {
            if (this._disposed) return;
            CMemUtils.FreeUnmanagedBuf(this.BufferPointer);
        }
    }

}
