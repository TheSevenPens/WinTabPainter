// See copright.md for copyright information.

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
        buf.Pointer = System.Runtime.InteropServices.Marshal.AllocHGlobal(bufsize);
        buf.disposed = false;
        buf.expected_type = null;
        return buf;
    }

    public static UnmanagedBuffer CreateForObject<T>() where T : new()
    {
        var buf = new UnmanagedBuffer();
        
        var temp_value = new T();
        int num_bytes = Marshal.SizeOf(temp_value);

        buf.Pointer = Marshal.AllocHGlobal(num_bytes);
        buf.disposed = false;
        buf.expected_type = typeof(T);

        return buf;
    }
    public static UnmanagedBuffer CreateForString()
    {
        var buf = new UnmanagedBuffer();
        buf.Pointer = System.Runtime.InteropServices.Marshal.AllocHGlobal(CWintabInfo.MAX_STRING_SIZE);
        buf.disposed = false;
        buf.expected_type = typeof(string);
        return buf;
    }

    private void assert_type(Type t)
    {
        if (this.expected_type != t)
        {
            throw new System.ArgumentOutOfRangeException("mismatch in types");
        }

    }

    public T MarshallObjectFromBuffer<T>(int size) where T : new()
    {
        this.assert_type(typeof(T));

        if (this.Pointer == IntPtr.Zero)
        {
            throw new ArgumentNullException(nameof(this.Pointer));
        }

        // If size doesn't match type size, then return a zeroed struct.
        if (size != System.Runtime.InteropServices.Marshal.SizeOf(typeof(T)))
        {
            int typeSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(T));
            var bytes = new Byte[typeSize];
            System.Runtime.InteropServices.Marshal.Copy(bytes, 0, this.Pointer, typeSize);
        }

        T value = (T)System.Runtime.InteropServices.Marshal.PtrToStructure(this.Pointer, typeof(T));

        return value;
    }

    public string MarshalStringFromBuffer(int size)
    {
        this.assert_type(typeof(string));
        
        // Strip off final null character before marshalling.
        int buf_size = size - 1;

        if (this.Pointer == IntPtr.Zero)
        {
            throw new System.ArgumentNullException(nameof(this.Pointer));
        }

        if (size <= 0)
        {
            throw new System.ArgumentOutOfRangeException(nameof(size));
        }

        var bytes = new Byte[buf_size];
        System.Runtime.InteropServices.Marshal.Copy(this.Pointer, bytes, 0, buf_size);
        var encoding = System.Text.Encoding.UTF8;
        string value = encoding.GetString(bytes);
        return value;

    }

    public void MarshalObjectlIntoBuffer(object structure)
    {
        this.assert_type(structure.GetType());
        System.Runtime.InteropServices.Marshal.StructureToPtr(structure, this.Pointer, false);
    }

    public T MarshalObjectFromBuffer<T>()
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
        if (num_pkts == 0)
        {
            return null;
        }

        if (this.Pointer == IntPtr.Zero)
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

        if (num_pkts == 0)
        {
            return null;
        }

        if (this.Pointer == IntPtr.Zero)
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
                packets[pkt_i] = tmpbuf.MarshalObjectFromBuffer<WintabDN.Structs.WintabPacketExt>();
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

        System.Runtime.InteropServices.Marshal.FreeHGlobal(this.Pointer);
        this.Pointer = IntPtr.Zero;
    }

}