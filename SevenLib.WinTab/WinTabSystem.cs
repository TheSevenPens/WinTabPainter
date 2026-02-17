// See copright.md for copyright information.

using System;

namespace SevenLib.WinTab;

/// <summary>
/// Class to access Wintab system-level interface data.
/// </summary>
public static class WinTabSystem
{
    /// <summary>
    /// Returns TRUE if Wintab service is running and responsive.
    /// </summary>
    /// <returns></returns>
    public static bool IsWintabAvailable()
    {
        IntPtr v = Interop.WinTabFunctions.WTInfoAObject<IntPtr>(0, 0);
        bool status = v > 0;
        return status;
    }

    /// <summary>
    /// Returns the number of devices connected.
    /// </summary>
    /// <returns></returns>
    public static UInt32 GetNumberOfDevices()
    {
        UInt32 value = Interop.WinTabFunctions.WTInfoAObject<UInt32>(
            (uint)Enums.EWTICategoryIndex.WTI_INTERFACE,
            (uint)Enums.EWTIInterfaceIndex.IFC_NDEVICES);
        return value;
    }

    /// <summary>
    /// Returns the default device.  If this value is -1, then it also known as a "virtual device".
    /// </summary>
    /// <returns></returns>
    public static Int32 GetDefaultDeviceIndex()
    {
        Int32 value = Interop.WinTabFunctions.WTInfoAObject<Int32>(
                (uint)Enums.EWTICategoryIndex.WTI_DEFCONTEXT,
                (uint)Enums.EWTIContextIndex.CTX_DEVICE);

        return value;
    }

    /// <summary>
    /// Returns whether a stylus is currently connected to the active cursor.
    /// </summary>
    /// <returns></returns>
    public static bool IsStylusActive()
    {
        var value = Interop.WinTabFunctions.WTInfoAObject<bool>(
            (uint)Enums.EWTICategoryIndex.WTI_INTERFACE,
            (uint)Enums.EWTIInterfaceIndex.IFC_NDEVICES);
        return value;
    }
}
