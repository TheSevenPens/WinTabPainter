///////////////////////////////////////////////////////////////////////////////
// CWintabContext.cs - Wintab context management for WintabDN
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

namespace WintabDN;

/// <summary>
/// Class to support access to Wintab context management.
/// </summary>
public class CWintabContext : IDisposable
{
    // Context data
    private Structs.WintabLogContext m_logicalcontext = new Structs.WintabLogContext();
    private Structs.HCTX m_hCTX = 0;

    /// <summary>
    /// Default constructor sets all data bits to be captured.
    /// </summary>
    public CWintabContext()
    {
        // Init with all bits set (The Full Monty) to get all non-extended data types.
        PktData = (uint)
            (Enums.EWintabPacketBit.PK_CONTEXT |
                Enums.EWintabPacketBit.PK_STATUS |
                Enums.EWintabPacketBit.PK_TIME |
                Enums.EWintabPacketBit.PK_CHANGED |
                Enums.EWintabPacketBit.PK_SERIAL_NUMBER |
                Enums.EWintabPacketBit.PK_CURSOR |
                Enums.EWintabPacketBit.PK_BUTTONS |
                Enums.EWintabPacketBit.PK_X |
                Enums.EWintabPacketBit.PK_Y |
                Enums.EWintabPacketBit.PK_Z |
                Enums.EWintabPacketBit.PK_NORMAL_PRESSURE |
                Enums.EWintabPacketBit.PK_TANGENT_PRESSURE |
                Enums.EWintabPacketBit.PK_ORIENTATION);
        MoveMask = PktData;
    }


    /// <summary>
    /// Open a Wintab context to the specified hwnd.
    /// </summary>
    /// <param name="hwnd_I">parent window for the context</param>
    /// <param name="enable_I">true to enable, false to disable</param>
    /// <returns>Returns non-zero context handle if successful.</returns>
    public Structs.HCTX Open(Structs.HWND hwnd_I, bool enable_I)
    {
        m_hCTX = CWintabFuncs.WTOpenA(hwnd_I, ref m_logicalcontext, enable_I);


        return m_hCTX;
    }

    /// <summary>
    /// Open a Wintab context that will send packet events to a message window.
    /// </summary>
    /// <returns>Returns true if successful.</returns>
    public bool Open()
    {
        // Get the handle of the anonymous MessageEvents window. This is a 
        // static (global) object, so there's only one of these at a time.
        var hwnd = WinForms.MessageEvents.WindowHandle;

        m_hCTX = CWintabFuncs.WTOpenA(hwnd, ref m_logicalcontext, true);

        return (m_hCTX > 0);
    }

    /// <summary>
    /// Close the context for this object.
    /// </summary>
    /// <returns>true if context successfully closed</returns>
    public bool Close()
    {
        if (m_hCTX == 0)
        {
            return false;
        }

        Dispose();
        return true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (m_hCTX != 0)
        {
            CWintabFuncs.WTClose(m_hCTX);
            m_hCTX = 0;
            m_logicalcontext = new Structs.WintabLogContext();
        }
    }

    ~CWintabContext()
    {
        Dispose(false);
    }

    /// <summary>
    /// Enable/disable this Wintab context.
    /// </summary>
    /// <param name="enable_I">true = enable</param>
    /// <returns>Returns true if completed successfully</returns>
    public bool Enable(bool enable_I)
    {
        bool status = false;

        if (m_hCTX == 0)
        {
            throw new Exception("EnableContext: invalid context");
        }

        status = CWintabFuncs.WTEnable(m_hCTX, enable_I);


        return status;
    }

    /// <summary>
    /// Sends a tablet context to the top or bottom of the order of overlapping tablet contexts
    /// </summary>
    /// <param name="toTop_I">true = send tablet to top of order</param>
    /// <returns>Returns true if successsful</returns>
    public bool SetOverlapOrder(bool toTop_I)
    {
        bool status = false;

        if (m_hCTX == 0)
        {
            throw new Exception("EnableContext: invalid context");
        }

        status = CWintabFuncs.WTOverlap(m_hCTX, toTop_I);


        return status;
    }


    /// <summary>
    /// Logical Wintab context managed by this object.  
    /// </summary>
    public Structs.WintabLogContext LogicalContext { get { return m_logicalcontext; } set { m_logicalcontext = value; } }

    /// <summary>
    /// Handle (identifier) used to identify this context.
    /// </summary>
    public Structs.HCTX HCtx { get { return m_hCTX; } }

    /// <summary>
    /// Get/Set context name.
    /// </summary>
    public string Name { get { return m_logicalcontext.lcName; } set { m_logicalcontext.lcName = value; } }

    /// <summary>
    /// Specifies options for the context. These options can be 
    /// combined by using the bitwise OR operator. The lcOptions 
    /// field can be any combination of the values defined in 
    /// ECTXOptionValues.
    /// </summary>
    public UInt32 Options { get { return m_logicalcontext.lcOptions; } set { m_logicalcontext.lcOptions = value; } }

    /// <summary>
    /// Specifies current status conditions for the context. 
    /// These conditions can be combined by using the bitwise OR 
    /// operator. The lcStatus field can be any combination of 
    /// the values defined in ECTXStatusValues.
    /// </summary>
    public UInt32 Status { get { return m_logicalcontext.lcStatus; } set { m_logicalcontext.lcStatus = value; } }

    /// <summary>
    /// Specifies which attributes of the context the application 
    /// wishes to be locked. Lock conditions specify attributes 
    /// of the context that cannot be changed once the context 
    /// has been opened (calls to WTConfig will have no effect 
    /// on the locked attributes). The lock conditions can be 
    /// combined by using the bitwise OR operator. The lcLocks 
    /// field can be any combination of the values defined in 
    /// ECTXLockValues. Locks can only be changed by the task 
    /// or process that owns the context.
    /// </summary>
    public UInt32 Locks { get { return m_logicalcontext.lcLocks; } set { m_logicalcontext.lcLocks = value; } }

    /// <summary>
    /// Specifies the range of message numbers that will be used for 
    /// reporting the activity of the context. 
    /// </summary>
    public UInt32 MsgBase { get { return m_logicalcontext.lcMsgBase; } set { m_logicalcontext.lcMsgBase = value; } }

    /// <summary>
    /// Specifies the device whose input the context processes.
    /// </summary>
    public UInt32 Device { get { return m_logicalcontext.lcDevice; } set { m_logicalcontext.lcDevice = value; } }

    /// <summary>
    /// Specifies the desired packet report rate in Hertz. Once the con-text is opened, this field will 
    /// contain the actual report rate.
    /// </summary>
    public UInt32 PktRate { get { return m_logicalcontext.lcPktRate; } set { m_logicalcontext.lcPktRate = value; } }

    /// <summary>
    /// Specifies which optional data items will be in packets returned from the context. Requesting 
    /// unsupported data items will cause Open() to fail.
    /// </summary>
    public Structs.WTPKT PktData { get { return m_logicalcontext.lcPktData; } set { m_logicalcontext.lcPktData = value; } }

    /// <summary>
    /// Specifies whether the packet data items will be returned in absolute or relative mode. If the item's 
    /// bit is set in this field, the item will be returned in relative mode. Bits in this field for items not 
    /// selected in the PktData property will be ignored.  Bits for data items that only allow one mode (such 
    /// as the packet identifier) will also be ignored.        
    /// </summary>
    public Structs.WTPKT PktMode { get { return m_logicalcontext.lcPktMode; } set { m_logicalcontext.lcPktMode = value; } }

    /// <summary>
    /// Specifies which packet data items can generate move events in the context. Bits for items that 
    /// are not part of the packet definition in the PktData property will be ignored. The bits for buttons, 
    /// time stamp, and the packet identifier will also be ignored. In the case of overlapping contexts, movement 
    /// events for data items not selected in this field may be processed by underlying contexts.
    /// </summary>
    public Structs.WTPKT MoveMask { get { return m_logicalcontext.lcMoveMask; } set { m_logicalcontext.lcMoveMask = value; } }

    /// <summary>
    /// Specifies the buttons for which button press events will be processed in the context. In the case of 
    /// overlapping contexts, button press events for buttons that are not selected in this field may be 
    /// processed by underlying contexts.
    /// </summary>
    public UInt32 BtnDnMask { get { return m_logicalcontext.lcBtnDnMask; } set { m_logicalcontext.lcBtnDnMask = value; } }

    /// <summary>
    /// Specifies the buttons for which button release events will be processed in the context. In the case 
    /// of overlapping contexts, button release events for buttons that are not selected in this field may be 
    /// processed by underlying contexts.  If both press and release events are selected for a button (see the 
    /// BtnDnMask property), then the interface will cause the context to implicitly capture all tablet events 
    /// while the button is down. In this case, events occurring outside the context will be clipped to the 
    /// context and processed as if they had occurred in the context. When the button is released, the context 
    /// will receive the button release event, and then event processing will return to normal.
    /// </summary>
    public UInt32 BtnUpMask { get { return m_logicalcontext.lcBtnUpMask; } set { m_logicalcontext.lcBtnUpMask = value; } }

    /// <summary>
    /// Specifies the X origin of the context's input area in the tablet's native coordinates. Value is clipped 
    /// to the tablet native coordinate space when the context is opened or modified.
    /// </summary>
    public Int32 InOrgX { get { return m_logicalcontext.lcInOrgX; } set { m_logicalcontext.lcInOrgX = value; } }

    /// <summary>
    /// Specifies the Y origin of the context's input area in the tablet's native coordinates. Value is clipped 
    /// to the tablet native coordinate space when the context is opened or modified.
    /// </summary>
    public Int32 InOrgY { get { return m_logicalcontext.lcInOrgY; } set { m_logicalcontext.lcInOrgY = value; } }

    /// <summary>
    /// Specifies the Z origin of the context's input area in the tablet's native coordinates. Value is clipped 
    /// to the tablet native coordinate space when the context is opened or modified.
    /// </summary>
    public Int32 InOrgZ { get { return m_logicalcontext.lcInOrgZ; } set { m_logicalcontext.lcInOrgZ = value; } }

    /// <summary>
    /// Specifies the X extent of the context's input area in the tablet's native coordinates. Value is clipped 
    /// to the tablet native coordinate space when the context is opened or modified.
    /// </summary>
    public Int32 InExtX { get { return m_logicalcontext.lcInExtX; } set { m_logicalcontext.lcInExtX = value; } }

    /// <summary>
    /// Specifies the Y extent of the context's input area in the tablet's native coordinates. Value is clipped 
    /// to the tablet native coordinate space when the context is opened or modified. 
    /// </summary>
    public Int32 InExtY { get { return m_logicalcontext.lcInExtY; } set { m_logicalcontext.lcInExtY = value; } }

    /// <summary>
    /// Specifies the Z extent of the context's input area in the tablet's native coordinates. Value is clipped 
    /// to the tablet native coordinate space when the context is opened or modified. 
    /// </summary>
    public Int32 InExtZ { get { return m_logicalcontext.lcInExtZ; } set { m_logicalcontext.lcInExtZ = value; } }

    /// <summary>
    /// Specifies the X origin of the context's output area in context output coordinates.  Value is used in 
    /// coordinate scaling for absolute mode only.
    /// </summary>
    public Int32 OutOrgX { get { return m_logicalcontext.lcOutOrgX; } set { m_logicalcontext.lcOutOrgX = value; } }

    /// <summary>
    /// Specifies the Y origin of the context's output area in context output coordinates.  Value is used in 
    /// coordinate scaling for absolute mode only.
    /// </summary>
    public Int32 OutOrgY { get { return m_logicalcontext.lcOutOrgY; } set { m_logicalcontext.lcOutOrgY = value; } }

    /// <summary>
    /// Specifies the Z origin of the context's output area in context output coordinates.  Value is used in 
    /// coordinate scaling for absolute mode only.
    /// </summary>
    public Int32 OutOrgZ { get { return m_logicalcontext.lcOutOrgZ; } set { m_logicalcontext.lcOutOrgZ = value; } }

    /// <summary>
    /// Specifies the X extent of the context's output area in context output coordinates.  Value is used 
    /// in coordinate scaling for absolute mode only.
    /// </summary>
    public Int32 OutExtX { get { return m_logicalcontext.lcOutExtX; } set { m_logicalcontext.lcOutExtX = value; } }

    /// <summary>
    /// Specifies the Y extent of the context's output area in context output coordinates.  Value is used 
    /// in coordinate scaling for absolute mode only.
    /// </summary>
    public Int32 OutExtY { get { return m_logicalcontext.lcOutExtY; } set { m_logicalcontext.lcOutExtY = value; } }

    /// <summary>
    /// Specifies the Z extent of the context's output area in context output coordinates.  Value is used 
    /// in coordinate scaling for absolute mode only.
    /// </summary>
    public Int32 OutExtZ { get { return m_logicalcontext.lcOutExtZ; } set { m_logicalcontext.lcOutExtZ = value; } }

    /// <summary>
    /// Specifies the relative-mode sensitivity factor for the x axis.
    /// </summary>
    public Structs.FIX32 SensX { get { return m_logicalcontext.lcSensX; } set { m_logicalcontext.lcSensX = value; } }

    /// <summary>
    /// Specifies the relative-mode sensitivity factor for the y axis.
    /// </summary>
    public Structs.FIX32 SensY { get { return m_logicalcontext.lcSensY; } set { m_logicalcontext.lcSensY = value; } }

    /// <summary>
    /// Specifies the relative-mode sensitivity factor for the Z axis.
    /// </summary>
    public Structs.FIX32 SensZ { get { return m_logicalcontext.lcSensZ; } set { m_logicalcontext.lcSensZ = value; } }

    /// <summary>
    /// Specifies the system cursor tracking mode. Zero specifies absolute; non-zero means relative.
    /// </summary>
    public bool SysMode { get { return m_logicalcontext.lcSysMode; } set { m_logicalcontext.lcSysMode = value; } }

    /// <summary>
    /// Specifies the X origin of the screen mapping area for system cursor tracking, in screen coordinates.
    /// </summary>
    public Int32 SysOrgX { get { return m_logicalcontext.lcSysOrgX; } set { m_logicalcontext.lcSysOrgX = value; } }

    /// <summary>
    /// Specifies the Y origin of the screen mapping area for system cursor tracking, in screen coordinates.
    /// </summary>
    public Int32 SysOrgY { get { return m_logicalcontext.lcSysOrgY; } set { m_logicalcontext.lcSysOrgY = value; } }

    /// <summary>
    /// Specifies the X extent of the screen mapping area for system cursor tracking, in screen coordinates.
    /// </summary>
    public Int32 SysExtX { get { return m_logicalcontext.lcSysExtX; } set { m_logicalcontext.lcSysExtX = value; } }

    /// <summary>
    /// Specifies the Y extent of the screen mapping area for system cursor tracking, in screen coordinates.
    /// </summary>
    public Int32 SysExtY { get { return m_logicalcontext.lcSysExtY; } set { m_logicalcontext.lcSysExtY = value; } }

    /// <summary>
    /// Specifies the system-cursor relative-mode sensitivity factor for the x axis.
    /// </summary>
    public Structs.FIX32 SysSensX { get { return m_logicalcontext.lcSysSensX; } set { m_logicalcontext.lcSysSensX = value; } }

    /// <summary>
    /// Specifies the system-cursor relative-mode sensitivity factor for the y axis.
    /// </summary>
    public Structs.FIX32 SysSensY { get { return m_logicalcontext.lcSysSensY; } set { m_logicalcontext.lcSysSensY = value; } }

}

