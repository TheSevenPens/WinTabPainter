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
//using System.Runtime.InteropServices;

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
    /// <param name="context_I">logical context for this data object</param>
    public CWintabData(CWintabContext context_I)
    {
        Init(context_I);
    }

    /// <summary>
    /// Initialize this data object.
    /// </summary>
    /// <param name="context_I">logical context for this data object</param>
    private void Init(CWintabContext context_I)
    {
        if (context_I == null)
        {
            throw new Exception("Trying to init CWintabData with null context.");
        }
        m_context = context_I;

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
    /// <param name="numPkts_I">desired #packets in queue</param>
    /// <returns>Returns true if operation successful</returns>
    public bool SetPacketQueueSize(UInt32 numPkts_I)
    {
        bool status = false;

        CheckForValidHCTX("SetPacketQueueSize");
        status = CWintabFuncs.WTQueueSizeSet(m_context.HCtx, numPkts_I);


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
    /// <param name="pktID_I">Identifier for the tablet event packet to return.</param>
    /// <returns>Returns a data packet with non-null context if successful.</returns>
    public Structs.WintabPacketExt GetDataPacketExt(UInt32 hCtx_I, UInt32 pktID_I)
    {
        int size = (int)(System.Runtime.InteropServices.Marshal.SizeOf(new Structs.WintabPacketExt()));
        IntPtr buf = Interop.CMemUtils.AllocUnmanagedBuf(size);
        Structs.WintabPacketExt[] packets = null;

        bool status = false;

        if (pktID_I == 0)
        {
            throw new Exception("GetDataPacket - invalid pktID");
        }

        CheckForValidHCTX("GetDataPacket");
        status = CWintabFuncs.WTPacket(hCtx_I, pktID_I, buf);

        if (status)
        {
            packets = Interop.CMemUtils.MarshalDataExtPackets(1, buf);
        }
        else
        {
            // If fails, make sure context is zero.
            packets[0].pkBase.nContext = 0;
        }


        /**
         * PERFORMANCE FIX: without this line, the memory consume of .NET apps increase
         * exponentially when the PEN is used for long time (or worse when the pen is leaved alone on the tablet screen)
         * causing the app to crash now or later...
         * Author: Alessandro del Gobbo   (alessandro@delgobbo.com)
         */
        Interop.CMemUtils.FreeUnmanagedBuf(buf);

        return packets[0];
    }



    /// <summary>
    /// Returns one packet of WintabPacket data from the packet queue. (Deprecated)
    /// </summary>
    /// <param name="pktID_I">Identifier for the tablet event packet to return.</param>
    /// <returns>Returns a data packet with non-null context if successful.</returns>
    public Structs.WintabPacket GetDataPacket(UInt32 pktID_I)
    {
        return GetDataPacket(m_context.HCtx, pktID_I);
    }



    /// <summary>
    /// Returns one packet of Wintab data from the packet queue.
    /// </summary>
    /// <param name="hCtx_I">Wintab context to be used when asking for the data</param>
    /// <param name="pktID_I">Identifier for the tablet event packet to return.</param>
    /// <returns>Returns a data packet with non-null context if successful.</returns>
    public Structs.WintabPacket GetDataPacket(UInt32 hCtx_I, UInt32 pktID_I)
    {
        IntPtr buf = Interop.CMemUtils.AllocUnmanagedBuf(System.Runtime.InteropServices.Marshal.SizeOf(typeof(Structs.WintabPacket)));
        var packet = new Structs.WintabPacket();

        if (pktID_I == 0)
        {
            throw new Exception("GetDataPacket - invalid pktID");
        }

        CheckForValidHCTX("GetDataPacket");

        if (CWintabFuncs.WTPacket(hCtx_I, pktID_I, buf))
        {
            packet = (Structs.WintabPacket)System.Runtime.InteropServices.Marshal.PtrToStructure(buf, typeof(Structs.WintabPacket));
        }
        else
        {
            //
            // If fails, make sure context is zero.
            //
            packet.pkContext = 0;

        }

        /**
         * PERFORMANCE FIX: without this line, the memory consume of .NET apps increase
         * exponentially when the PEN is used for long time (or worse when the pen is leaved alone on the tablet screen)
         * causing the app to crash now or later...
         * Author: Alessandro del Gobbo   (alessandro@delgobbo.com)
         */
        Interop.CMemUtils.FreeUnmanagedBuf(buf);

        return packet;
    }



    /// <summary>
    /// Removes all pending data packets from the context's queue.
    /// </summary>
    public void FlushDataPackets(uint numPacketsToFlush_I)
    {
        CheckForValidHCTX("FlushDataPackets");
        CWintabFuncs.WTPacketsGet(m_context.HCtx, numPacketsToFlush_I, IntPtr.Zero);
    }



    /// <summary>
    /// Returns an array of Wintab data packets from the packet queue.
    /// </summary>
    /// <param name="maxPkts_I">Specifies the maximum number of packets to return.</param>
    /// <param name="remove_I">If true, returns data packets and removes them from the queue.</param>
    /// <param name="numPkts_O">Number of packets actually returned.</param>
    /// <returns>Returns the next maxPkts_I from the list.  Note that if remove_I is false, then 
    /// repeated calls will return the same packets.  If remove_I is true, then packets will be 
    /// removed and subsequent calls will get different packets (if any).</returns>
    public Structs.WintabPacket[] GetDataPackets(UInt32 maxPkts_I, bool remove_I, ref UInt32 numPkts_O)
    {
        Structs.WintabPacket[] packets = null;

        CheckForValidHCTX("GetDataPackets");

        if (maxPkts_I == 0)
        {
            throw new Exception("GetDataPackets - maxPkts_I is zero.");
        }

        // Packet array is used whether we're just looking or buying.
        int size = (int)(maxPkts_I * System.Runtime.InteropServices.Marshal.SizeOf(new Structs.WintabPacket()));
        IntPtr buf = Interop.CMemUtils.AllocUnmanagedBuf(size);

        if (remove_I)
        {
            // Return data packets and remove packets from queue.
            numPkts_O = CWintabFuncs.WTPacketsGet(m_context.HCtx, maxPkts_I, buf);

            if (numPkts_O > 0)
            {
                packets = Interop.CMemUtils.MarshalDataPackets(numPkts_O, buf);
            }

            //System.Diagnostics.Debug.WriteLine("GetDataPackets: numPkts_O: " + numPkts_O);
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
                { throw new Exception("WTQueuePacketsEx reports zero start packet identifier"); }

                if (pktIDEnd == 0)
                { throw new Exception("WTQueuePacketsEx reports zero end packet identifier"); }

                // Peek up to the max number of packets specified.
                UInt32 numFoundPkts = CWintabFuncs.WTDataPeek(m_context.HCtx, pktIDStart, pktIDEnd, maxPkts_I, buf, ref numPkts_O);

                System.Diagnostics.Debug.WriteLine("GetDataPackets: WTDataPeek - numFoundPkts: " + numFoundPkts + ", numPkts_O: " + numPkts_O);

                if (numFoundPkts > 0 && numFoundPkts < numPkts_O)
                {
                    throw new Exception("WTDataPeek reports more packets returned than actually exist in queue.");
                }

                packets = Interop.CMemUtils.MarshalDataPackets(numPkts_O, buf);
            }
        }



        return packets;
    }


    /// <summary>
    /// Throws exception if logical context for this data object is zero.
    /// </summary>
    private void CheckForValidHCTX(string msg)
    {
        if (m_context.HCtx == 0)
        { 
            throw new Exception(msg + " - Bad Context"); 
        }
    }
}
