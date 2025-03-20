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

    public static UnmanagedBuffer CreateForObjectArray<T>(int numitems) where T : new()
    {
        var buf = new UnmanagedBuffer();

        var temp_value = new T();
        var item_size = System.Runtime.InteropServices.Marshal.SizeOf(temp_value);
        int bufsize = item_size * numitems;


        buf.Pointer = UnmanagedBuffer.alloc_unmanaged_buffer_by_size(bufsize);
        buf.disposed = false;
        buf.expected_type = null;
        return buf;
    }

    public static UnmanagedBuffer CreateForObject<T>() where T : new()
    {
        var buf = new UnmanagedBuffer();
        var temp_value = new T();
        buf.Pointer = UnmanagedBuffer.alloc_unmanaged_bufer(temp_value);
        buf.disposed = false;
        buf.expected_type = typeof(T);
        return buf;
    }
    public static UnmanagedBuffer CreateForString()
    {
        var buf = new UnmanagedBuffer();
        buf.Pointer = UnmanagedBuffer.alloc_unmanaged_buffer_by_size(CWintabInfo.MAX_STRING_SIZE);
        buf.disposed = false;
        buf.expected_type = typeof(string);
        return buf;
    }

    public T GetObjectFromBuffer<T>(int size) where T : new()
    {
        this.assert_type(typeof(T));
        var value = UnmanagedBuffer.marshal_buffer_to_object<T>(Pointer, size);
        return value;
    }
    public string GetStringFromBuffer(int size)
    {
        this.assert_type(typeof(string));
        // Strip off final null character before marshalling.
        var s = UnmanagedBuffer.marshal_buffer_to_string(this.Pointer, size - 1);
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



    /// <summary>
    /// Marshal unmanaged data packets into managed WintabPacket data.
    /// </summary>
    /// <param name="num_pkts">number of packets to marshal</param>
    /// <param name="buf_ptr">pointer to unmanaged heap memory containing data packets</param>
    /// <returns></returns>


    public WintabDN.Structs.WintabPacket[] MarshalDataPacketsFromBuffer(UInt32 num_pkts)
    {
        if (num_pkts == 0 || this.Pointer == IntPtr.Zero)
        {
            return null;
        }

        var packets = new WintabDN.Structs.WintabPacket[num_pkts];

        var temp_packet = new WintabDN.Structs.WintabPacket();
        int pkt_size = System.Runtime.InteropServices.Marshal.SizeOf(temp_packet);

        for (int i = 0; i < num_pkts; i++)
        {
            packets[i] = (WintabDN.Structs.WintabPacket)System.Runtime.InteropServices.Marshal.PtrToStructure(IntPtr.Add(this.Pointer, i * pkt_size), typeof(WintabDN.Structs.WintabPacket));
        }
        return packets;
    }


    /// <summary>
    /// Marshal unmanaged Extension data packets into managed WintabPacketExt data.
    /// </summary>
    /// <param name="num_pkts">number of packets to marshal</param>
    /// <param name="buf_ptr">pointer to unmanaged heap memory containing data packets</param>
    /// <returns></returns>
    public WintabDN.Structs.WintabPacketExt[] MarshalDataExtPacketsFromBuffer(UInt32 num_pkts)
    {
        var packets = new WintabDN.Structs.WintabPacketExt[num_pkts];

        if (num_pkts == 0 || this.Pointer == IntPtr.Zero)
        {
            return null;
        }

        // Marshal each WintabPacketExt in the array separately.
        // This is "necessary" because none of the other ways I tried to marshal
        // seemed to work.  It's ugly, but it works.

        var temp_pkt_ext = new WintabDN.Structs.WintabPacketExt();
        int pkt_size = System.Runtime.InteropServices.Marshal.SizeOf(temp_pkt_ext);
        var bytes = new Byte[num_pkts * pkt_size];
        System.Runtime.InteropServices.Marshal.Copy(this.Pointer, bytes, 0, (int)num_pkts * pkt_size);

        var temp_bytes = new Byte[pkt_size];

        for (int pkt_i = 0; pkt_i < num_pkts; pkt_i++)
        {
            for (int i = 0; i < pkt_size; i++)
            {
                temp_bytes[i] = bytes[(pkt_i * pkt_size) + i];
            }

            using (var tmpbuf = WintabDN.Interop.UnmanagedBuffer.CreateForObject<WintabDN.Structs.WintabPacketExt>())
            {
                System.Runtime.InteropServices.Marshal.Copy(temp_bytes, 0, tmpbuf.Pointer, pkt_size);
                packets[pkt_i] = tmpbuf.MarshallFromBuffer<WintabDN.Structs.WintabPacketExt>();
            }
        }

        return packets;
    }

    public void Dispose()
    {
        if (this.disposed)
        {
            return;
        }
        if (this.Pointer == IntPtr.Zero)
        {
            return;
        }
        UnmanagedBuffer.free_unmanaged_buffer(this.Pointer);
        this.Pointer = IntPtr.Zero;
    }


    ///internals
    ///
    /// <summary>
    /// Allocates a pointer to unmanaged heap memory of sizeof(val_I).
    /// </summary>
    /// <param name="obj">managed object that determines #bytes of unmanaged buf</param>
    /// <returns>Unmanaged buffer pointer.</returns>
    private static IntPtr alloc_unmanaged_bufer(Object obj)
    {
        IntPtr buf = IntPtr.Zero;
        int num_bytes = Marshal.SizeOf(obj);
        buf = Marshal.AllocHGlobal(num_bytes);
        return buf;
    }

    /// <returns>Unmanaged buffer pointer.</returns>
    private static IntPtr alloc_unmanaged_buffer_by_size(int bufsize)
    {
        IntPtr buf = IntPtr.Zero;
        buf = System.Runtime.InteropServices.Marshal.AllocHGlobal(bufsize);
        return buf;
    }

    /// <summary>
    /// Marshals specified buf to the specified type.
    /// </summary>
    /// <typeparam name="T">type to which buf_I is marshalled</typeparam>
    /// <param name="buf_ptr">unmanaged heap pointer</param>
    /// <param name="buf_size">expected size of buf_I</param>
    /// <returns>Managed object of specified type.</returns>
    private static T marshal_buffer_to_object<T>(IntPtr buf_ptr, int buf_size)
    {
        if (buf_ptr == IntPtr.Zero)
        {
            throw new ArgumentNullException(nameof(buf_ptr));
        }

        // If size doesn't match type size, then return a zeroed struct.
        if (buf_size != System.Runtime.InteropServices.Marshal.SizeOf(typeof(T)))
        {
            int typeSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(T));
            var bytes = new Byte[typeSize];
            System.Runtime.InteropServices.Marshal.Copy(bytes, 0, buf_ptr, typeSize);
        }

        return (T)System.Runtime.InteropServices.Marshal.PtrToStructure(buf_ptr, typeof(T));
    }

    /// <summary>
    /// Free unmanaged memory pointed to by buf_I.
    /// </summary>
    /// <param name="buf_ptr">pointer to unmanaged heap memory</param>
    private static void free_unmanaged_buffer(IntPtr buf_ptr)
    {
        if (buf_ptr == IntPtr.Zero)
        {
            return;
        }

        System.Runtime.InteropServices.Marshal.FreeHGlobal(buf_ptr);
        buf_ptr = IntPtr.Zero;
    }

    /// <summary>
    /// Marshals a string from an unmanaged buffer.
    /// </summary>
    /// <param name="buf_ptr">pointer to unmanaged heap memory</param>
    /// <param name="buf_size">size of ASCII string, includes null termination</param>
    /// <returns></returns>
    private static string marshal_buffer_to_string(IntPtr buf_ptr, int buf_size)
    {

        if (buf_ptr == IntPtr.Zero)
        {
            throw new System.ArgumentNullException(nameof(buf_ptr));
        }

        if (buf_size <= 0)
        {
            throw new System.ArgumentOutOfRangeException(nameof(buf_size));
        }

        var bytes = new Byte[buf_size];
        System.Runtime.InteropServices.Marshal.Copy(buf_ptr, bytes, 0, buf_size);
        var encoding = System.Text.Encoding.UTF8;
        string value = encoding.GetString(bytes);
        return value;
    }

    /// <summary>
    /// Marshal unmanaged data packets into managed WintabPacket data.
    /// </summary>
    /// <param name="num_pkts">number of packets to marshal</param>
    /// <param name="buf_ptr">pointer to unmanaged heap memory containing data packets</param>
    /// <returns></returns>

    /// <summary>
    /// Marshal unmanaged data packets into managed WintabPacket data.
    /// </summary>
    /// <param name="num_pkts">number of packets to marshal</param>
    /// <param name="buf_ptr">pointer to unmanaged heap memory containing data packets</param>
    /// <returns></returns>


}