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
using System.Runtime.InteropServices;

namespace WintabDN.Structs;

/// <summary>
/// Managed version of Wintab LOGCONTEXT struct.  This structure determines what events an 
/// application will get, how they will be processed, and how they will be delivered to the 
/// application or to Windows itself.
/// </summary>
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct WintabLogContext
{
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]    //LCNAMELEN
    public string lcName;
    public uint lcOptions;
    public uint lcStatus;
    public uint lcLocks;
    public uint lcMsgBase;
    public uint lcDevice;
    public uint lcPktRate;
    public WTPKT lcPktData;
    public WTPKT lcPktMode;
    public WTPKT lcMoveMask;
    public uint lcBtnDnMask;
    public uint lcBtnUpMask;
    public int lcInOrgX;
    public int lcInOrgY;
    public int lcInOrgZ;
    public int lcInExtX;
    public int lcInExtY;
    public int lcInExtZ;
    public int lcOutOrgX;
    public int lcOutOrgY;
    public int lcOutOrgZ;
    public int lcOutExtX;
    public int lcOutExtY;
    public int lcOutExtZ;
    public FIX32 lcSensX;
    public FIX32 lcSensY;
    public FIX32 lcSensZ;
    public bool lcSysMode;
    public int lcSysOrgX;
    public int lcSysOrgY;
    public int lcSysExtX;
    public int lcSysExtY;
    public FIX32 lcSysSensX;
    public FIX32 lcSysSensY;
}

