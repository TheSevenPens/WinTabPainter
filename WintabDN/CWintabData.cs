///////////////////////////////////////////////////////////////////////////////
// CWintabData.cs - Wintab data management for WintabDN
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
using WintabDN.Interop;

namespace WintabDN;

/// <summary>
/// Class to support capture and management of Wintab daa.
/// </summary>
public class CWintabData
{
    private CWintabContext m_context;

    /// <summary>
    /// CWintabData constructor
    /// </summary>
    /// <param name="context">logical context for this data object</param>
    public CWintabData(CWintabContext context)
    {
        Init(context);
    }

    /// <summary>
    /// Initialize this data object.
    /// </summary>
    /// <param name="context">logical context for this data object</param>
    private void Init(CWintabContext context)
    {
        if (context == null)
        {
            throw new Exception("Trying to init CWintabData with null context.");
        }
        m_context = context;

        // Watch for the Wintab WT_PACKET event.
        WinForms.MessageEvents.WatchMessage((int)Enums.EWintabEventMessage.WT_PACKET);

        // Watch for the Wintab WT_PACKETEXT event.
        WinForms.MessageEvents.WatchMessage((int)Enums.EWintabEventMessage.WT_PACKETEXT);

    }

    /// <summary>
    /// Set the handler to be called when WT_PACKET events are received.
    /// </summary>
    /// <param name="handler_I">WT_PACKET event handler supplied by the client.</param>
    public void SetWTPacketEventHandler(EventHandler<WinForms.MessageReceivedEventArgs> handler_I)
    {
        WinForms.MessageEvents.MessageReceived += handler_I;
    }

    public void ClearWTPacketEventHandler()
    {
        WinForms.MessageEvents.ClearMessageEvents();
    }


    /// <summary>
    /// Set packet queue size for this data object's context.
    /// </summary>
    /// <param name="num_pkts">desired #packets in queue</param>
    /// <returns>Returns true if operation successful</returns>
    public bool SetPacketQueueSize(UInt32 num_pkts)
    {
        bool status = false;

        CheckForValidHCTX("SetPacketQueueSize");
        status = CWintabFuncs.WTQueueSizeSet(m_context.HCtx, num_pkts);


        return status;
    }

    /// <summary>
    /// Get packet queue size for this data object's context.
    /// </summary>
    /// <returns>Returns a packet queue size in #packets or 0 if fails</returns>
    public UInt32 GetPacketQueueSize()
    {
        UInt32 numPkts = 0;

        CheckForValidHCTX("GetPacketQueueSize");
        numPkts = CWintabFuncs.WTQueueSizeGet(m_context.HCtx);


        return numPkts;
    }



    /// <summary>
    /// Returns one packet of WintabPacketExt data from the packet queue.
    /// </summary>
    /// <param name="hCtx_I">Wintab context to be used when asking for the data</param>
    /// <param name="pkt_id">Identifier for the tablet event packet to return.</param>
    /// <returns>Returns a data packet with non-null context if successful.</returns>
    public Structs.WintabPacketExt GetDataPacketExt(UInt32 hCtx_I, UInt32 pkt_id)
    {
        if (pkt_id == 0)
        {
            throw new System.ArgumentOutOfRangeException(nameof(pkt_id), "Cannot be zero");
        }

        CheckForValidHCTX("GetDataPacket");

        using (var buf = WintabDN.Interop.UnmanagedBuffer.CreateForObject<Structs.WintabPacketExt>())
        {

            bool status = CWintabFuncs.WTPacket(hCtx_I, pkt_id, buf.Pointer);

            Structs.WintabPacketExt[] packets = null;
            if (status)
            {
                packets = buf.MarshalDataExtPacketsFromBuffer(1);
            }
            else
            {
                // If fails, make sure context is zero.
                packets[0].pkBase.nContext = 0;
            }

            return packets[0];
        }
    }



    /// <summary>
    /// Returns one packet of WintabPacket data from the packet queue. (Deprecated)
    /// </summary>
    /// <param name="pkt_id">Identifier for the tablet event packet to return.</param>
    /// <returns>Returns a data packet with non-null context if successful.</returns>
    public Structs.WintabPacket GetDataPacket(UInt32 pkt_id)
    {
        return GetDataPacket(m_context.HCtx, pkt_id);
    }



    /// <summary>
    /// Returns one packet of Wintab data from the packet queue.
    /// </summary>
    /// <param name="hCtx_I">Wintab context to be used when asking for the data</param>
    /// <param name="pkt_id">Identifier for the tablet event packet to return.</param>
    /// <returns>Returns a data packet with non-null context if successful.</returns>
    public Structs.WintabPacket GetDataPacket(UInt32 hCtx_I, UInt32 pkt_id)
    {

        var packet = new Structs.WintabPacket();

        if (pkt_id == 0)
        {
            throw new System.ArgumentOutOfRangeException(nameof(pkt_id), "Cannot be zero");
        }

        CheckForValidHCTX("GetDataPacket");

        using (var buf = WintabDN.Interop.UnmanagedBuffer.CreateForObject<Structs.WintabPacket>())
        {
            bool status = CWintabFuncs.WTPacket(hCtx_I, pkt_id, buf.Pointer);

            if (status)
            {
                packet = buf.MarshalObjectFromBuffer<Structs.WintabPacket>();
            }
            else
            {
                //
                // If fails, make sure context is zero.
                //
                packet.pkContext = 0;

            }
            return packet;
        }
    }



    /// <summary>
    /// Removes all pending data packets from the context's queue.
    /// </summary>
    public void FlushDataPackets(uint num_pkts)
    {
        CheckForValidHCTX("FlushDataPackets");
        CWintabFuncs.WTPacketsGet(m_context.HCtx, num_pkts, IntPtr.Zero);
    }



    /// <summary>
    /// Returns an array of Wintab data packets from the packet queue.
    /// </summary>
    /// <param name="max_pkts">Specifies the maximum number of packets to return.</param>
    /// <param name="remove">If true, returns data packets and removes them from the queue.</param>
    /// <param name="num_pkts">Number of packets actually returned.</param>
    /// <returns>Returns the next maxPkts_I from the list.  Note that if remove_I is false, then 
    /// repeated calls will return the same packets.  If remove_I is true, then packets will be 
    /// removed and subsequent calls will get different packets (if any).</returns>
    public Structs.WintabPacket[] GetDataPackets(UInt32 max_pkts, bool remove, ref UInt32 num_pkts)
    {
        Structs.WintabPacket[] packets = null;

        CheckForValidHCTX("GetDataPackets");

        if (max_pkts == 0)
        {
            throw new System.ArgumentOutOfRangeException(nameof(max_pkts), "Cannot be zero");
        }

        // Packet array is used whether we're just looking or buying.

        using (var buf = WintabDN.Interop.UnmanagedBuffer.CreateForObjectArray<Structs.WintabPacket>((int)max_pkts))
        {


            if (remove)
            {
                // Return data packets and remove packets from queue.
                num_pkts = CWintabFuncs.WTPacketsGet(m_context.HCtx, max_pkts, buf.Pointer);

                if (num_pkts > 0)
                {
                    packets = buf.MarshalDataPacketsFromBuffer(num_pkts);
                }
            }
            else
            {
                // Return data packets, but leave on queue.  (Peek mode)
                UInt32 pktIDOldest = 0;
                UInt32 pktIDNewest = 0;

                // Get oldest and newest packet identifiers in the queue.  These will bound the
                // packets that are actually returned.
                if (CWintabFuncs.WTQueuePacketsEx(m_context.HCtx, ref pktIDOldest, ref pktIDNewest))
                {
                    UInt32 pktIDStart = pktIDOldest;
                    UInt32 pktIDEnd = pktIDNewest;

                    if (pktIDStart == 0)
                    {
                        throw new System.ArgumentOutOfRangeException(nameof(pktIDStart), "WTQueuePacketsEx reports zero start packet identifier");
                    }

                    if (pktIDEnd == 0)
                    {
                        throw new System.ArgumentOutOfRangeException(nameof(pktIDEnd), "WTQueuePacketsEx reports zero end packet identifier");
                    }

                    // Peek up to the max number of packets specified.
                    UInt32 numFoundPkts = CWintabFuncs.WTDataPeek(m_context.HCtx, pktIDStart, pktIDEnd, max_pkts, buf.Pointer, ref num_pkts);

                    if (numFoundPkts > 0 && numFoundPkts < num_pkts)
                    {
                        throw new System.Exception("WTDataPeek reports more packets returned than actually exist in queue.");
                    }

                    packets = buf.MarshalDataPacketsFromBuffer(num_pkts);
                }
            }
            return packets;
        }
    }


    /// <summary>
    /// Throws exception if logical context for this data object is zero.
    /// </summary>
    private void CheckForValidHCTX(string msg)
    {
        if (m_context.HCtx == 0)
        {
            throw new System.ArgumentOutOfRangeException(nameof(m_context.HCtx), msg + " - Bad Context");
        }
    }
}
