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

namespace WintabDN;
using P_HCTX = UInt32;
using P_HWND = System.IntPtr;

/// <summary>
/// P/Invoke wrappers for Wintab functions.
/// See Wintab_v140.doc (Wintab 1.4 spec) and related Wintab documentation for details.
/// </summary>
public class CWintabFuncs
{
    /// <summary>
    /// This function returns global information about the interface in an application-supplied buffer. 
    /// Different types of information are specified by different index arguments. Applications use this 
    /// function to receive information about tablet coordinates, physical dimensions, capabilities, and 
    /// cursor types.
    /// </summary>
    /// <param name="wCategory_I">Identifies the category from which information is being requested.</param>
    /// <param name="nIndex_I">Identifies which information is being requested from within the category.</param>
    /// <param name="lpOutput_O">Points to a buffer to hold the requested information.</param>
    /// <returns>The return value specifies the size of the returned information in bytes. If the information 
    /// is not supported, the function returns zero. If a tablet is not physically present, this function 
    /// always returns zero.
    /// </returns>
    [DllImport("Wintab32.dll", CharSet = CharSet.Auto)]
    public static extern UInt32 WTInfoA(UInt32 wCategory_I, UInt32 nIndex_I, IntPtr lpOutput_O);

    /// <summary>
    /// This function establishes an active context on the tablet. On successful completion of this function, 
    /// the application may begin receiving tablet events via messages (if they were requested), and may use 
    /// the handle returned to poll the context, or to perform other context-related functions.
    /// </summary>
    /// <param name="hWnd_I">Identifies the window that owns the tablet context, and receives messages from the context.</param>
    /// <param name="logContext_I">Points to an application-provided WintabLogContext data structure describing the context to be opened.</param>
    /// <param name="enable_I">Specifies whether the new context will immediately begin processing input data.</param>
    /// <returns>The return value identifies the new context. It is NULL if the context is not opened.</returns>
    [DllImport("Wintab32.dll", CharSet = CharSet.Auto)]
    public static extern P_HCTX WTOpenA(P_HWND hWnd_I, ref Structs.WintabLogContext logContext_I, bool enable_I);

    /// <summary>
    /// This function closes and destroys the tablet context object.
    /// </summary>
    /// <param name="hctx_I">Identifies the context to be closed.</param>
    /// <returns>The function returns a non-zero value if the context was valid and was destroyed. Otherwise, it returns zero.</returns>
    [DllImport("Wintab32.dll", CharSet = CharSet.Auto)]
    public static extern bool WTClose(P_HCTX hctx_I);

    /// <summary>
    /// This function enables or disables a tablet context, temporarily turning on or off the processing of packets.
    /// </summary>
    /// <param name="hctx_I">Identifies the context to be enabled or disabled.</param>
    /// <param name="enable_I">Specifies enabling if non-zero, disabling if zero.</param>
    /// <returns>The function returns true if the enable or disable request was satisfied.</returns>
    [DllImport("Wintab32.dll", CharSet = CharSet.Auto)]
    public static extern bool WTEnable(P_HCTX hctx_I, bool enable_I);

    /// <summary>
    /// This function sends a tablet context to the top or bottom of the order of overlapping tablet contexts.
    /// </summary>
    /// <param name="hctx_I">Identifies the context to move within the overlap order.</param>
    /// <param name="toTop_I">Specifies sending the context to the top of the overlap order true, or to the bottom if false.</param>
    /// <returns>The function returns true if successful.</returns>
    [DllImport("Wintab32.dll", CharSet = CharSet.Auto)]
    public static extern bool WTOverlap(P_HCTX hctx_I, bool toTop_I);

    /// <summary>
    /// This function returns the number of packets the context's queue can hold.
    /// </summary>
    /// <param name="hctx_I">Identifies the context whose queue size is being returned.</param>
    /// <returns>The number of packets the queue can hold.</returns>
    [DllImport("Wintab32.dll", CharSet = CharSet.Auto)]
    public static extern UInt32 WTQueueSizeGet(P_HCTX hctx_I);

    /// <summary>
    /// This function attempts to change the context's queue size to the value specified in nPkts_I.
    /// </summary>
    /// <param name="hctx_I">Identifies the context whose queue size is being set.</param>
    /// <param name="nPkts_I">Specifies the requested queue size.</param>
    /// <returns>The return value is true if the queue size was successfully changed.</returns>
    [DllImport("Wintab32.dll", CharSet = CharSet.Auto)]
    public static extern bool WTQueueSizeSet(P_HCTX hctx_I, UInt32 nPkts_I);

    /// <summary>
    /// This function fills in the passed pktBuf_O buffer with the context event packet having 
    /// the specified serial number. The returned packet and any older packets are removed from 
    /// the context's internal queue.
    /// </summary>
    /// <param name="hctx_I">Identifies the context whose packets are being returned.</param>
    /// <param name="pktSerialNum_I">Serial number of the tablet event to return.</param>
    /// <param name="pktBuf_O">Buffer to receive the event packet.</param>
    /// <returns>The return value is true if the specified packet was found and returned. 
    /// It is false if the specified packet was not found in the queue.</returns>
    [DllImport("Wintab32.dll", CharSet = CharSet.Auto)]
    public static extern bool WTPacket(P_HCTX hctx_I, UInt32 pktSerialNum_I, IntPtr pktBuf_O);

    /// <summary>
    /// This function copies the next maxPkts_I events from the packet queue of context hCtx to 
    /// the passed pktBuf_O buffer and removes them from the queue
    /// </summary>
    /// <param name="hctx_I">Identifies the context whose packets are being returned.</param>
    /// <param name="maxPkts_I">Specifies the maximum number of packets to return</param>
    /// <param name="pktBuf_O">Buffer to receive the event packets.</param>
    /// <returns>The return value is the number of packets copied in the buffer.</returns>
    [DllImport("Wintab32.dll", CharSet = CharSet.Auto)]
    public static extern UInt32 WTPacketsGet(P_HCTX hctx_I, UInt32 maxPkts_I, IntPtr pktBuf_O);     

    /// <summary>A
    /// This function copies all packets with Identifiers between pktIDStart_I and pktIDEnd_I 
    /// inclusive from the context's queue to the passed buffer and removes them from the queue.
    /// </summary>
    /// <param name="hctx_I">Identifies the context whose packets are being returned.</param>
    /// <param name="pktIDStart_I">Identifier of the oldest tablet event to return.</param>
    /// <param name="pktIDEnd_I">Identifier of the newest tablet event to return.</param>
    /// <param name="maxPkts_I">Specifies the maximum number of packets to return.</param>
    /// <param name="pktBuf_O">Buffer to receive the event packets.</param>
    /// <param name="numPkts_O">Number of packets actually copied.</param>
    /// <returns>The return value is the total number of packets found in the queue 
    /// between pktIDStart_I and pktIDEnd_I.</returns>
    [DllImport("Wintab32.dll", CharSet = CharSet.Auto)]
    public static extern UInt32 WTDataGet(P_HCTX hctx_I, UInt32 pktIDStart_I, UInt32 pktIDEnd_I,
        UInt32 maxPkts_I, IntPtr pktBuf_O, ref UInt32 numPkts_O);

    /// <summary>
    /// This function copies all packets with serial numbers between pktIDStart_I and pktIDEnd_I
    /// inclusive, from the context's queue to the passed buffer without removing them from the queue.
    /// </summary>
    /// <param name="hctx_I">Identifies the context whose packets are being read.</param>
    /// <param name="pktIDStart_I">Identifier of the oldest tablet event to return.</param>
    /// <param name="pktIDEnd_I">Identifier of the newest tablet event to return.</param>
    /// <param name="maxPkts_I">Specifies the maximum number of packets to return.</param>
    /// <param name="pktBuf_O">Buffer to receive the event packets.</param>
    /// <param name="numPkts_O">Number of packets actually copied.</param>
    /// <returns>The return value is the total number of packets found in the queue between 
    /// pktIDStart_I and pktIDEnd_I.</returns>
    [DllImport("Wintab32.dll", CharSet = CharSet.Auto)]
    public static extern UInt32 WTDataPeek(P_HCTX hctx_I, UInt32 pktIDStart_I, UInt32 pktIDEnd_I,
        UInt32 maxPkts_I, IntPtr pktBuf_O, ref UInt32 numPkts_O);

    /// <summary>
    /// This function returns the identifiers of the oldest and newest packets currently in the queue.
    /// </summary>
    /// <param name="hctx_I">Identifies the context whose queue is being queried.</param>
    /// <param name="pktIDOldest_O">Identifier of the oldest packet in the queue.</param>
    /// <param name="pktIDNewest_O">Identifier of the newest packet in the queue.</param>
    /// <returns>This function returns bool if successful.</returns>
    [DllImport("Wintab32.dll", CharSet = CharSet.Auto)]
    public static extern bool WTQueuePacketsEx(P_HCTX hctx_I, ref UInt32 pktIDOldest_O, ref UInt32 pktIDNewest_O);

    /// <summary>
    /// This function retrieves any context-specific data for an extension.
    /// </summary>
    /// <param name="hctx_I">Identifies the context whose extension attributes are being retrieved.</param>
    /// <param name="extTag_I">Identifies the extension tag for which context-specific data is being retrieved.</param>
    /// <param name="extData_O">Points to a buffer to hold retrieved data (WTExtensionProperty).</param>
    /// <returns></returns>
    [DllImport("Wintab32.dll", CharSet = CharSet.Auto)]
    public static extern bool WTExtGet(P_HCTX hctx_I, UInt32 extTag_I, IntPtr extData_O);

    /// <summary>
    /// This function sets any context-specific data for an extension.
    /// </summary>
    /// <param name="hctx_I">Identifies the context whose extension attributes are being modified.</param>
    /// <param name="extTag_I">Identifies the extension tag for which context-specific data is being modified.</param>
    /// <param name="extData_I">Points to the new data (WTExtensionProperty).</param>
    /// <returns></returns>
    [DllImport("Wintab32.dll", CharSet = CharSet.Auto)]
    public static extern bool WTExtSet(P_HCTX hctx_I, UInt32 extTag_I, IntPtr extData_I);


    public static string WTInfoAString(uint cat, uint index)
    {
        using (var buf = Interop.UnmanagedBuffer.CreateForString())
        {
            int size = (int)CWintabFuncs.WTInfoA(cat, index, buf.Pointer);
            string val = buf.GetValueString(size);
            return val;
        }
    }

    public static T WTInfoAObject<T>(uint cat, uint index) where T : new()
    {
        using (var buf = Interop.UnmanagedBuffer.CreateForObject<T>())
        {
            int size = (int)CWintabFuncs.WTInfoA(cat, index, buf.Pointer);
            T val = buf.GetValueObject<T>(size);
            return val;
        }
    }
}
