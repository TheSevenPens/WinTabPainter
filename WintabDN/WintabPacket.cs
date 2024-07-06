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
using System.Runtime.InteropServices;

namespace WintabDN;

/// <summary>
/// Wintab data packet.  Contains the "Full Monty" for all possible data values.
/// </summary>
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct WintabPacket
{
    /// <summary>
    /// Specifies the context that generated the event.
    /// </summary>
    public HCTX pkContext;                      // PK_CONTEXT

    public UInt32 SPACING;                     // SevenPens - when migrating to .NET 8 I had to add this initial space to get the properties aligned

    /// <summary>
    /// Specifies various status and error conditions. These conditions can be 
    /// combined by using the bitwise OR opera-tor. The pkStatus field can be any
    /// any combination of the values defined in EWintabPacketStatusValue.
    /// </summary>
    public UInt32 pkStatus;                     // PK_STATUS

    /// <summary>
    /// In absolute mode, specifies the system time at which the event was posted. In
    /// relative mode, specifies the elapsed time in milliseconds since the last packet.
    /// </summary>
    public UInt32 pkTime;                       // PK_TIME

    /// <summary>
    /// Specifies which of the included packet data items have changed since the 
    /// previously posted event.
    /// </summary>
    public WTPKT pkChanged;                     // PK_CHANGED

    /// <summary>
    /// This is an identifier assigned to the packet by the context. Consecutive 
    /// packets will have consecutive serial numbers.
    /// </summary>
    public UInt32 pkSerialNumber;               // PK_SERIAL_NUMBER

    /// <summary>
    /// Specifies which cursor type generated the packet.
    /// </summary>
    public UInt32 pkCursor;                     // PK_CURSOR

    /// <summary>
    /// In absolute mode, is a UInt32 containing the current button state. 
    /// In relative mode, is a UInt32 whose low word contains a button number, 
    /// and whose high word contains one of the codes in EWintabPacketButtonCode.
    /// </summary>
    public UInt32 pkButtons;                    // PK_BUTTONS

    /// <summary>
    /// In absolute mode, each is a UInt32 containing the scaled cursor location 
    /// along the X axis.  In relative mode, this is an Int32 containing 
    /// scaled change in cursor position.
    /// </summary>
    public Int32 pkX;                           // PK_X

    /// <summary>
    /// In absolute mode, each is a UInt32 containing the scaled cursor location 
    /// along the Y axis.  In relative mode, this is an Int32 containing 
    /// scaled change in cursor position.
    /// </summary>
    public Int32 pkY;                           // PK_Y

    /// <summary>
    /// In absolute mode, each is a UInt32 containing the scaled cursor location 
    /// along the Z axis.  In relative mode, this is an Int32 containing 
    /// scaled change in cursor position.
    /// </summary>
    public Int32 pkZ;                           // PK_Z    

    /// <summary>
    /// In absolute mode, this is a UINT containing the adjusted state  
    /// of the normal pressure, respectively. In relative mode, this is
    /// an int containing the change in adjusted pressure state.
    /// </summary>
    public UInt32 pkNormalPressure;   // PK_NORMAL_PRESSURE

    /// <summary>
    /// In absolute mode, this is a UINT containing the adjusted state  
    /// of the tangential pressure, respectively. In relative mode, this is
    /// an int containing the change in adjusted pressure state.
    /// </summary>
    public UInt32 pkTangentPressure; // PK_TANGENT_PRESSURE

    /// <summary>
    /// Contains updated cursor orientation information. See the 
    /// WTOrientation structure for details.
    /// </summary>
    public WTOrientation pkOrientation;         // ORIENTATION
}
