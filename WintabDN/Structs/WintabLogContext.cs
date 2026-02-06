// See copright.md for copyright information.


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

