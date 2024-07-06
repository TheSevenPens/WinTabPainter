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
using WintabDN.Structs;

namespace WintabDN;


/// <summary>
/// Class to support access to Wintab context management.
/// </summary>
public class CWintabContext
{
    // Context data.
    private WintabLogContext m_logContext = new WintabLogContext();
    private HCTX                m_hCTX = 0;

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
    public Structs.HCTX Open(HWND hwnd_I, bool enable_I)
    {
        m_hCTX = CWintabFuncs.WTOpenA(hwnd_I, ref m_logContext, enable_I);


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
        var hwnd = MessageEvents.WindowHandle;

        m_hCTX = CWintabFuncs.WTOpenA(hwnd, ref m_logContext, true);

        return (m_hCTX > 0);
    }

    /// <summary>
    /// Close the context for this object.
    /// </summary>
    /// <returns>true if context successfully closed</returns>
    public bool Close()
    {
        bool status = false;

        if (m_hCTX == 0)
        {
            throw new Exception("CloseContext: invalid context");
        }

        status = CWintabFuncs.WTClose(m_hCTX);
        m_hCTX = 0;
        m_logContext = new WintabLogContext();


        return status;
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
    public WintabLogContext LogContext { get { return m_logContext; } set { m_logContext = value; } }

    /// <summary>
    /// Handle (identifier) used to identify this context.
    /// </summary>
    public HCTX HCtx    { get { return m_hCTX; } }

    /// <summary>
    /// Get/Set context name.
    /// </summary>
    public string Name      { get { return m_logContext.lcName; }     set { m_logContext.lcName = value; } }

    /// <summary>
    /// Specifies options for the context. These options can be 
    /// combined by using the bitwise OR operator. The lcOptions 
    /// field can be any combination of the values defined in 
    /// ECTXOptionValues.
    /// </summary>
    public UInt32 Options   { get { return m_logContext.lcOptions; }  set { m_logContext.lcOptions = value; } }

    /// <summary>
    /// Specifies current status conditions for the context. 
    /// These conditions can be combined by using the bitwise OR 
    /// operator. The lcStatus field can be any combination of 
    /// the values defined in ECTXStatusValues.
    /// </summary>
    public UInt32 Status    { get { return m_logContext.lcStatus; }   set { m_logContext.lcStatus = value; } }

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
    public UInt32 Locks     { get { return m_logContext.lcLocks; }    set { m_logContext.lcLocks = value; } }

    /// <summary>
    /// Specifies the range of message numbers that will be used for 
    /// reporting the activity of the context. 
    /// </summary>
    public UInt32 MsgBase   { get { return m_logContext.lcMsgBase; }  set { m_logContext.lcMsgBase = value; } }

    /// <summary>
    /// Specifies the device whose input the context processes.
    /// </summary>
    public UInt32 Device    { get { return m_logContext.lcDevice; }   set { m_logContext.lcDevice = value; } }

    /// <summary>
    /// Specifies the desired packet report rate in Hertz. Once the con-text is opened, this field will 
    /// contain the actual report rate.
    /// </summary>
    public UInt32 PktRate   { get { return m_logContext.lcPktRate; }  set { m_logContext.lcPktRate = value; } }

    /// <summary>
    /// Specifies which optional data items will be in packets returned from the context. Requesting 
    /// unsupported data items will cause Open() to fail.
    /// </summary>
    public WTPKT PktData    { get { return m_logContext.lcPktData; }  set { m_logContext.lcPktData = value; } }

    /// <summary>
    /// Specifies whether the packet data items will be returned in absolute or relative mode. If the item's 
    /// bit is set in this field, the item will be returned in relative mode. Bits in this field for items not 
    /// selected in the PktData property will be ignored.  Bits for data items that only allow one mode (such 
    /// as the packet identifier) will also be ignored.        
    /// </summary>
    public WTPKT PktMode    { get { return m_logContext.lcPktMode; }  set { m_logContext.lcPktMode = value; } }

    /// <summary>
    /// Specifies which packet data items can generate move events in the context. Bits for items that 
    /// are not part of the packet definition in the PktData property will be ignored. The bits for buttons, 
    /// time stamp, and the packet identifier will also be ignored. In the case of overlapping contexts, movement 
    /// events for data items not selected in this field may be processed by underlying contexts.
    /// </summary>
    public WTPKT MoveMask   { get { return m_logContext.lcMoveMask; } set { m_logContext.lcMoveMask = value; } }

    /// <summary>
    /// Specifies the buttons for which button press events will be processed in the context. In the case of 
    /// overlapping contexts, button press events for buttons that are not selected in this field may be 
    /// processed by underlying contexts.
    /// </summary>
    public UInt32 BtnDnMask { get { return m_logContext.lcBtnDnMask; } set { m_logContext.lcBtnDnMask = value; } }

    /// <summary>
    /// Specifies the buttons for which button release events will be processed in the context. In the case 
    /// of overlapping contexts, button release events for buttons that are not selected in this field may be 
    /// processed by underlying contexts.  If both press and release events are selected for a button (see the 
    /// BtnDnMask property), then the interface will cause the context to implicitly capture all tablet events 
    /// while the button is down. In this case, events occurring outside the context will be clipped to the 
    /// context and processed as if they had occurred in the context. When the button is released, the context 
    /// will receive the button release event, and then event processing will return to normal.
    /// </summary>
    public UInt32 BtnUpMask { get { return m_logContext.lcBtnUpMask; } set { m_logContext.lcBtnUpMask = value; } }

    /// <summary>
    /// Specifies the X origin of the context's input area in the tablet's native coordinates. Value is clipped 
    /// to the tablet native coordinate space when the context is opened or modified.
    /// </summary>
    public Int32 InOrgX     { get { return m_logContext.lcInOrgX; }   set { m_logContext.lcInOrgX = value; } }

    /// <summary>
    /// Specifies the Y origin of the context's input area in the tablet's native coordinates. Value is clipped 
    /// to the tablet native coordinate space when the context is opened or modified.
    /// </summary>
    public Int32 InOrgY     { get { return m_logContext.lcInOrgY; }   set { m_logContext.lcInOrgY = value; } }

    /// <summary>
    /// Specifies the Z origin of the context's input area in the tablet's native coordinates. Value is clipped 
    /// to the tablet native coordinate space when the context is opened or modified.
    /// </summary>
    public Int32 InOrgZ     { get { return m_logContext.lcInOrgZ; }   set { m_logContext.lcInOrgZ = value; } }

    /// <summary>
    /// Specifies the X extent of the context's input area in the tablet's native coordinates. Value is clipped 
    /// to the tablet native coordinate space when the context is opened or modified.
    /// </summary>
    public Int32 InExtX     { get { return m_logContext.lcInExtX; }   set { m_logContext.lcInExtX = value; } }

    /// <summary>
    /// Specifies the Y extent of the context's input area in the tablet's native coordinates. Value is clipped 
    /// to the tablet native coordinate space when the context is opened or modified. 
    /// </summary>
    public Int32 InExtY     { get { return m_logContext.lcInExtY; }   set { m_logContext.lcInExtY = value; } }

    /// <summary>
    /// Specifies the Z extent of the context's input area in the tablet's native coordinates. Value is clipped 
    /// to the tablet native coordinate space when the context is opened or modified. 
    /// </summary>
    public Int32 InExtZ     { get { return m_logContext.lcInExtZ; }   set { m_logContext.lcInExtZ = value; } }

    /// <summary>
    /// Specifies the X origin of the context's output area in context output coordinates.  Value is used in 
    /// coordinate scaling for absolute mode only.
    /// </summary>
    public Int32 OutOrgX    { get { return m_logContext.lcOutOrgX; }  set { m_logContext.lcOutOrgX = value; } }

    /// <summary>
    /// Specifies the Y origin of the context's output area in context output coordinates.  Value is used in 
    /// coordinate scaling for absolute mode only.
    /// </summary>
    public Int32 OutOrgY    { get { return m_logContext.lcOutOrgY; }  set { m_logContext.lcOutOrgY = value; } }

    /// <summary>
    /// Specifies the Z origin of the context's output area in context output coordinates.  Value is used in 
    /// coordinate scaling for absolute mode only.
    /// </summary>
    public Int32 OutOrgZ    { get { return m_logContext.lcOutOrgZ; }  set { m_logContext.lcOutOrgZ = value; } }

    /// <summary>
    /// Specifies the X extent of the context's output area in context output coordinates.  Value is used 
    /// in coordinate scaling for absolute mode only.
    /// </summary>
    public Int32 OutExtX    { get { return m_logContext.lcOutExtX; }  set { m_logContext.lcOutExtX = value; } }

    /// <summary>
    /// Specifies the Y extent of the context's output area in context output coordinates.  Value is used 
    /// in coordinate scaling for absolute mode only.
    /// </summary>
    public Int32 OutExtY    { get { return m_logContext.lcOutExtY; }  set { m_logContext.lcOutExtY = value; } }

    /// <summary>
    /// Specifies the Z extent of the context's output area in context output coordinates.  Value is used 
    /// in coordinate scaling for absolute mode only.
    /// </summary>
    public Int32 OutExtZ    { get { return m_logContext.lcOutExtZ; }  set { m_logContext.lcOutExtZ = value; } }

    /// <summary>
    /// Specifies the relative-mode sensitivity factor for the x axis.
    /// </summary>
    public FIX32 SensX      { get { return m_logContext.lcSensX; }    set { m_logContext.lcSensX = value; } }

    /// <summary>
    /// Specifies the relative-mode sensitivity factor for the y axis.
    /// </summary>
    public FIX32 SensY      { get { return m_logContext.lcSensY; }    set { m_logContext.lcSensY = value; } }

    /// <summary>
    /// Specifies the relative-mode sensitivity factor for the Z axis.
    /// </summary>
    public FIX32 SensZ      { get { return m_logContext.lcSensZ; }    set { m_logContext.lcSensZ = value; } }

    /// <summary>
    /// Specifies the system cursor tracking mode. Zero specifies absolute; non-zero means relative.
    /// </summary>
    public bool SysMode     { get { return m_logContext.lcSysMode; }  set { m_logContext.lcSysMode = value; } }

    /// <summary>
    /// Specifies the X origin of the screen mapping area for system cursor tracking, in screen coordinates.
    /// </summary>
    public Int32 SysOrgX    { get { return m_logContext.lcSysOrgX; }  set { m_logContext.lcSysOrgX = value; } }

    /// <summary>
    /// Specifies the Y origin of the screen mapping area for system cursor tracking, in screen coordinates.
    /// </summary>
    public Int32 SysOrgY    { get { return m_logContext.lcSysOrgY; }  set { m_logContext.lcSysOrgY = value; } }

    /// <summary>
    /// Specifies the X extent of the screen mapping area for system cursor tracking, in screen coordinates.
    /// </summary>
    public Int32 SysExtX    { get { return m_logContext.lcSysExtX; }  set { m_logContext.lcSysExtX = value; } }

    /// <summary>
    /// Specifies the Y extent of the screen mapping area for system cursor tracking, in screen coordinates.
    /// </summary>
    public Int32 SysExtY    { get { return m_logContext.lcSysExtY; }  set { m_logContext.lcSysExtY = value; } }

    /// <summary>
    /// Specifies the system-cursor relative-mode sensitivity factor for the x axis.
    /// </summary>
    public FIX32 SysSensX   { get { return m_logContext.lcSysSensX; } set { m_logContext.lcSysSensX = value; } }

    /// <summary>
    /// Specifies the system-cursor relative-mode sensitivity factor for the y axis.
    /// </summary>
    public FIX32 SysSensY   { get { return m_logContext.lcSysSensY; } set { m_logContext.lcSysSensY = value; } }

}

