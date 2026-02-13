// See copright.md for copyright information.

using System;

namespace SevenLib.WinTab;

/// <summary>
/// Class to access Wintab interface data.
/// </summary>
public class CWintabInfo
{
    public const int MAX_STRING_SIZE = 256;

    /// <summary>
    /// Returns TRUE if Wintab service is running and responsive.
    /// </summary>
    /// <returns></returns>
    public static bool IsWintabAvailable()
    {
        IntPtr v = CWintabFuncs.WTInfoAObject<IntPtr>(0, 0);
        bool status = v > 0;
        return status;
    }

    /// <summary>
    /// Returns a string containing device name.
    /// </summary>
    /// <returns></returns>
    public static String GetDeviceInfo()
    {
        string s = CWintabFuncs.WTInfoAString(
            (uint)Enums.EWTICategoryIndex.WTI_DEVICES,
            (uint)Enums.EWTIDevicesIndex.DVC_NAME);

        return s;
    }

    /// <summary>
    /// Returns the default system context or digitizer , with useful context overrides.
    /// </summary>
    /// <param name="cat">EWTICategoryIndex.WTI_DEFCONTEXT for digitizer context. EWTICategoryIndex.WTI_DEFSYSCTX for system context</param>
    /// <param name="options_I">caller's options; OR'd into context options</param>
    /// <returns>A 
    public static CWintabContext GetDefaultContext(Enums.EWTICategoryIndex cat, Enums.ECTXOptionValues options_I = 0)
    {
        // SevenPens: In the original code this is made up of two separate methods that
        // do almost the exact same thing. I've merged them and added the cat parameter
        // indicates to indicate which kind of context to build

        // EWTICategoryIndex.WTI_DEFSYSCTX = System context
        // EWTICategoryIndex.WTI_DEFCONTEXT = Digitizer context

        if (cat != Enums.EWTICategoryIndex.WTI_DEFSYSCTX && cat != Enums.EWTICategoryIndex.WTI_DEFCONTEXT)
        {
            throw new System.ArgumentOutOfRangeException(nameof(cat));
        }

        var context = GetDefaultContext(cat);

        if (context == null)
        {
            return context;
        }

        // Add caller's options.
        context.Options |= (uint)options_I;

        if (cat == Enums.EWTICategoryIndex.WTI_DEFSYSCTX)
        {
            // Make sure we get data packet messages.
            context.Options |= (uint)Enums.ECTXOptionValues.CXO_MESSAGES;
        }

        // Send all possible data bits (not including extended data).
        // This is redundant with CWintabContext initialization, which
        // also inits with PK_PKTBITS_ALL.
        uint PACKETDATA = (uint)Enums.EWintabPacketBit.PK_PKTBITS_ALL;  // The Full Monty 
        uint PACKETMODE = (uint)Enums.EWintabPacketBit.PK_BUTTONS;

        // Set the context data bits.
        context.PktData = PACKETDATA;
        context.PktMode = PACKETMODE;
        context.MoveMask = PACKETDATA;
        context.BtnUpMask = context.BtnDnMask;

        // Name the context
        context.Name = (cat == Enums.EWTICategoryIndex.WTI_DEFSYSCTX) ? "SYSTEM CONTEXT" : "DIGITIZER CONTEXT";

        return context;
    }

    /// <summary>
    /// Helper function to get digitizing or system default context.
    /// </summary>
    /// <param name="contextType_I">Use WTI_DEFCONTEXT for digital context or WTI_DEFSYSCTX for system context</param>
    /// <returns>Returns the default context or null on error.</returns>
    private static CWintabContext GetDefaultContext(Enums.EWTICategoryIndex contextIndex_I)
    {
        var context = new CWintabContext();
        context.LogicalContext = CWintabFuncs.WTInfoAObject<Structs.WintabLogContext>((uint)contextIndex_I, 0);
        return context;
    }

    /// <summary>
    /// Returns the default device.  If this value is -1, then it also known as a "virtual device".
    /// </summary>
    /// <returns></returns>
    public static Int32 GetDefaultDeviceIndex()
    {
        Int32 value = CWintabFuncs.WTInfoAObject<Int32>(
                (uint)Enums.EWTICategoryIndex.WTI_DEFCONTEXT,
                (uint)Enums.EWTIContextIndex.CTX_DEVICE);

        return value;
    }



    /// <summary>
    /// Returns the WintabAxis object for specified device and dimension.
    /// </summary>
    /// <param name="devIndex_I">Device index (-1 = virtual device)</param>
    /// <param name="dim_I">Dimension: AXIS_X, AXIS_Y or AXIS_Z</param>
    /// <returns></returns>
    public static Structs.WintabAxis GetDeviceAxis(Int32 devIndex_I, Enums.EAxisDimension dim_I)
    {
        var axis = CWintabFuncs.WTInfoAObject<Structs.WintabAxis>(
                (uint)(Enums.EWTICategoryIndex.WTI_DEVICES + devIndex_I),
                (uint)dim_I);
        return axis;
    }

    /// <summary>
    /// Returns a 3-element array describing the tablet's orientation range and resolution capabilities.
    /// </summary>
    /// <returns></returns>
    public static Structs.WintabAxisArray GetDeviceOrientation(out bool tiltSupported_O)
    {
        tiltSupported_O = false;

        var axis_array = CWintabFuncs.WTInfoAObject<Structs.WintabAxisArray>(
            (uint)Enums.EWTICategoryIndex.WTI_DEVICES,
            (uint)Enums.EWTIDevicesIndex.DVC_ORIENTATION);

        // If size == 0, then returns a zeroed struct.
        tiltSupported_O = (axis_array.array[0].axResolution != 0 && axis_array.array[1].axResolution != 0);
        return axis_array;
    }


    /// <summary>
    /// Returns a 3-element array describing the tablet's rotation range and resolution capabilities
    /// </summary>
    /// <returns></returns>
    public static Structs.WintabAxisArray GetDeviceRotation(out bool rotationSupported_O)
    {
        rotationSupported_O = false;

        var axis_array = CWintabFuncs.WTInfoAObject<Structs.WintabAxisArray>(
            (uint)Enums.EWTICategoryIndex.WTI_DEVICES,
            (uint)Enums.EWTIDevicesIndex.DVC_ROTATION);

        rotationSupported_O = (axis_array.array[0].axResolution != 0 && axis_array.array[1].axResolution != 0);
        return axis_array;
    }

    /// <summary>
    /// Returns the number of devices connected.
    /// </summary>
    /// <returns></returns>
    public static UInt32 GetNumberOfDevices()
    {
        UInt32 value = CWintabFuncs.WTInfoAObject<UInt32>(
            (uint)Enums.EWTICategoryIndex.WTI_INTERFACE,
            (uint)Enums.EWTIInterfaceIndex.IFC_NDEVICES);
        return value;
    }

    /// <summary>
    /// Returns whether a stylus is currently connected to the active cursor.
    /// </summary>
    /// <returns></returns>
    public static bool IsStylusActive()
    {
        var value = CWintabFuncs.WTInfoAObject<bool>(
            (uint)Enums.EWTICategoryIndex.WTI_INTERFACE,
            (uint)Enums.EWTIInterfaceIndex.IFC_NDEVICES);
        return value;
    }


    /// <summary>
    /// Returns a string containing the name of the selected stylus. 
    /// </summary>
    /// <param name="index_I">indicates stylus type</param>
    /// <returns></returns>
    public static string GetStylusName(Enums.EWTICursorNameIndex index_I)
    {
        string value = CWintabFuncs.WTInfoAString(
            (uint)index_I,
            (uint)Enums.EWTICursorsIndex.CSR_NAME);

        return value;

    }




    /// <summary>
    /// Return max normal pressure supported by tablet.
    /// </summary>
    /// <param name="getNormalPressure_I">TRUE=> normal pressure; 
    /// FALSE=> tangential pressure (not supported on all tablets)</param>
    /// <returns>maximum pressure value or zero on error</returns>
    public static Int32 GetMaxPressure(bool getNormalPressure_I = true)
    {
        Enums.EWTIDevicesIndex devIdx = (getNormalPressure_I ?
                Enums.EWTIDevicesIndex.DVC_NPRESSURE : Enums.EWTIDevicesIndex.DVC_TPRESSURE);

        var axis = CWintabFuncs.WTInfoAObject<Structs.WintabAxis>(
            (uint)Enums.EWTICategoryIndex.WTI_DEVICES,
            (uint)devIdx);
        return axis.axMax;
    }



    /// <summary>
    /// Return the WintabAxis object for the specified dimension.
    /// </summary>
    /// <param name="dimension_I">Dimension to fetch (eg: x, y)</param>
    /// <returns></returns>
    public static Structs.WintabAxis GetTabletAxis(Enums.EAxisDimension dimension_I)
    {
        var axis = CWintabFuncs.WTInfoAObject<Structs.WintabAxis>(
            (uint)Enums.EWTICategoryIndex.WTI_DEVICES
           , (uint)dimension_I);
        return axis;

    }

}
