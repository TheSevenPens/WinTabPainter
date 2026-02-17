// See copright.md for copyright information.

using System;

namespace SevenLib.WinTab;

/// <summary>
/// Class to access Wintab device capabilities and properties.
/// </summary>
public static class WinTabDevice
{
    /// <summary>
    /// Returns a string containing device name.
    /// </summary>
    /// <returns></returns>
    public static String GetDeviceInfo()
    {
        string s = Interop.WinTabFunctions.WTInfoAString(
            (uint)Enums.EWTICategoryIndex.WTI_DEVICES,
            (uint)Enums.EWTIDevicesIndex.DVC_NAME);

        return s;
    }

    /// <summary>
    /// Returns the WintabAxis object for specified device and dimension.
    /// </summary>
    /// <param name="devIndex_I">Device index (-1 = virtual device)</param>
    /// <param name="dim_I">Dimension: AXIS_X, AXIS_Y or AXIS_Z</param>
    /// <returns></returns>
    public static Structs.WintabAxis GetDeviceAxis(Int32 devIndex_I, Enums.EAxisDimension dim_I)
    {
        var axis = Interop.WinTabFunctions.WTInfoAObject<Structs.WintabAxis>(
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

        var axis_array = Interop.WinTabFunctions.WTInfoAObject<Structs.WintabAxisArray>(
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

        var axis_array = Interop.WinTabFunctions.WTInfoAObject<Structs.WintabAxisArray>(
            (uint)Enums.EWTICategoryIndex.WTI_DEVICES,
            (uint)Enums.EWTIDevicesIndex.DVC_ROTATION);

        rotationSupported_O = (axis_array.array[0].axResolution != 0 && axis_array.array[1].axResolution != 0);
        return axis_array;
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

        var axis = Interop.WinTabFunctions.WTInfoAObject<Structs.WintabAxis>(
            (uint)Enums.EWTICategoryIndex.WTI_DEVICES,
            (uint)devIdx);
        return axis.axMax;
    }

    /// <summary>
    /// Returns a string containing the name of the selected stylus. 
    /// </summary>
    /// <param name="index_I">indicates stylus type</param>
    /// <returns></returns>
    public static string GetStylusName(Enums.EWTICursorNameIndex index_I)
    {
        string value = Interop.WinTabFunctions.WTInfoAString(
            (uint)index_I,
            (uint)Enums.EWTICursorsIndex.CSR_NAME);

        return value;
    }

    /// <summary>
    /// Return the WintabAxis object for the specified dimension.
    /// </summary>
    /// <param name="dimension_I">Dimension to fetch (eg: x, y)</param>
    /// <returns></returns>
    public static Structs.WintabAxis GetTabletAxis(Enums.EAxisDimension dimension_I)
    {
        var axis = Interop.WinTabFunctions.WTInfoAObject<Structs.WintabAxis>(
            (uint)Enums.EWTICategoryIndex.WTI_DEVICES
           , (uint)dimension_I);
        return axis;
    }
}
