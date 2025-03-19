///////////////////////////////////////////////////////////////////////////////
// CMemUtils.cs - memory utility functions for WintabDN
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

//#define TRACE_RAW_BYTES
// Some code requires a newer .NET version.
#define DOTNET_4_OR_LATER 

using System;

namespace WintabDN.Interop;

/// <summary>
/// Provide utility methods for unmanaged memory management.
/// </summary>
public static class CMemUtils
{


    /// <summary>
    /// Allocates a pointer to unmanaged heap memory of given size.
    /// </summary>
    /// <param name="size_I">number of bytes to allocate</param>
    /// <returns>Unmanaged buffer pointer.</returns>
    public static IntPtr AllocUnmanagedBuf(int size_I)
    {
        IntPtr buf = IntPtr.Zero;

        buf = System.Runtime.InteropServices.Marshal.AllocHGlobal(size_I);


        return buf;
    }

    /// <summary>
    /// Marshals specified buf to the specified type.
    /// </summary>
    /// <typeparam name="T">type to which buf_I is marshalled</typeparam>
    /// <param name="buf_ptr">unmanaged heap pointer</param>
    /// <param name="buf_size">expected size of buf_I</param>
    /// <returns>Managed object of specified type.</returns>
    public static T MarshalBufferToObject<T>(IntPtr buf_ptr, int buf_size)
    {
        if (buf_ptr == IntPtr.Zero)
        {
            throw new Exception("MarshalUnmanagedBuf has NULL buf_I");
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
    public static void FreeUnmanagedBuf(IntPtr buf_ptr)
    {
        if (buf_ptr == IntPtr.Zero) { return; }

        System.Runtime.InteropServices.Marshal.FreeHGlobal(buf_ptr);
        buf_ptr = IntPtr.Zero;

    }

    /// <summary>
    /// Marshals a string from an unmanaged buffer.
    /// </summary>
    /// <param name="buf_ptr">pointer to unmanaged heap memory</param>
    /// <param name="buf_size">size of ASCII string, includes null termination</param>
    /// <returns></returns>
    public static string MarshalBufferToString(IntPtr buf_ptr, int buf_size)
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

    public static WintabDN.Structs.WintabPacket[] MarshalDataPackets(UInt32 num_pkts, IntPtr buf_ptr)
    {
        if (num_pkts == 0 || buf_ptr == IntPtr.Zero)
        {
            return null;
        }

        var packets = new WintabDN.Structs.WintabPacket[num_pkts];

        int pkt_size = System.Runtime.InteropServices.Marshal.SizeOf(new WintabDN.Structs.WintabPacket());

        for (int i = 0; i < num_pkts; i++)
        {
            packets[i] = (WintabDN.Structs.WintabPacket)System.Runtime.InteropServices.Marshal.PtrToStructure(IntPtr.Add(buf_ptr, i * pkt_size), typeof(WintabDN.Structs.WintabPacket));
        }
        return packets;
    }


    /// <summary>
    /// Marshal unmanaged Extension data packets into managed WintabPacketExt data.
    /// </summary>
    /// <param name="num_pkts">number of packets to marshal</param>
    /// <param name="buf_ptr">pointer to unmanaged heap memory containing data packets</param>
    /// <returns></returns>
    public static WintabDN.Structs.WintabPacketExt[] MarshalDataExtPackets(UInt32 num_pkts, IntPtr buf_ptr)
    {
        var packets = new WintabDN.Structs.WintabPacketExt[num_pkts];

        if (num_pkts == 0 || buf_ptr == IntPtr.Zero)
        {
            return null;
        }

        // Marshal each WintabPacketExt in the array separately.
        // This is "necessary" because none of the other ways I tried to marshal
        // seemed to work.  It's ugly, but it works.
        int pkt_size = System.Runtime.InteropServices.Marshal.SizeOf(new WintabDN.Structs.WintabPacketExt());
        var bytes = new Byte[num_pkts * pkt_size];
        System.Runtime.InteropServices.Marshal.Copy(buf_ptr, bytes, 0, (int)num_pkts * pkt_size);

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
}
