///////////////////////////////////////////////////////////////////////////////
// CWintabInfo.cs - Wintab information access for WintabDN
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
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.DirectoryServices.ActiveDirectory;
using System.Security.Policy;



namespace WintabDN
{

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
            IntPtr buf = IntPtr.Zero;
            bool status = false;

            status = (CWintabFuncs.WTInfoA(0, 0, buf) > 0);

            return status;
        }

        /// <summary>
        /// Returns a string containing device name.
        /// </summary>
        /// <returns></returns>
        public static String GetDeviceInfo()
        {
            string s = _WTInfoA_STRING(
                (uint)EWTICategoryIndex.WTI_DEVICES,
                (uint)EWTIDevicesIndex.DVC_NAME);

            return s;
        }

        /// <summary>
        /// Returns the default system context or digitizer , with useful context overrides.
        /// </summary>
        /// <param name="cat">EWTICategoryIndex.WTI_DEFCONTEXT for digitizer context. EWTICategoryIndex.WTI_DEFSYSCTX for system context</param>
        /// <param name="options_I">caller's options; OR'd into context options</param>
        /// <returns>A 
        public static CWintabContext GetDefaultContext(EWTICategoryIndex cat, ECTXOptionValues options_I = 0)
        {
            // SevenPens: In the original code this is made up of two separate methods that
            // do almost the exact same thing. I've merged them and added the cat parameter
            // indicates to indicate which kind of context to build

            // EWTICategoryIndex.WTI_DEFSYSCTX = System context
            // EWTICategoryIndex.WTI_DEFCONTEXT = Digitizer context

            if ( cat != EWTICategoryIndex.WTI_DEFSYSCTX && cat != EWTICategoryIndex.WTI_DEFCONTEXT)
            {
                throw new System.ArgumentOutOfRangeException(nameof(cat));
            }

            var context = GetDefaultContext(cat);

            if ( context == null)
            {
                return context;
            }

            // Add caller's options.
            context.Options |= (uint)options_I; 

            if (cat == EWTICategoryIndex.WTI_DEFSYSCTX)
            {
                // Make sure we get data packet messages.
                context.Options |= (uint)ECTXOptionValues.CXO_MESSAGES;
            }

            // Send all possible data bits (not including extended data).
            // This is redundant with CWintabContext initialization, which
            // also inits with PK_PKTBITS_ALL.
            uint PACKETDATA = (uint)EWintabPacketBit.PK_PKTBITS_ALL;  // The Full Monty 
            uint PACKETMODE = (uint)EWintabPacketBit.PK_BUTTONS; 

            // Set the context data bits.
            context.PktData = PACKETDATA; 
            context.PktMode = PACKETMODE; 
            context.MoveMask = PACKETDATA; 
            context.BtnUpMask = context.BtnDnMask; 

            // Name the context
            context.Name = cat == EWTICategoryIndex.WTI_DEFSYSCTX ? "SYSTEM CONTEXT" : "DIGITIZER CONTEXT";

            return context;
        }

        /// <summary>
        /// Helper function to get digitizing or system default context.
        /// </summary>
        /// <param name="contextType_I">Use WTI_DEFCONTEXT for digital context or WTI_DEFSYSCTX for system context</param>
        /// <returns>Returns the default context or null on error.</returns>
        private static CWintabContext GetDefaultContext(EWTICategoryIndex contextIndex_I)        
        {
            var lc = _WTInfoA_OBJECT<WintabLogContext>((uint)contextIndex_I, 0);
            CWintabContext context = new CWintabContext();
            context.LogContext = lc;
            return context;
        }

        /// <summary>
        /// Returns the default device.  If this value is -1, then it also known as a "virtual device".
        /// </summary>
        /// <returns></returns>
        public static Int32 GetDefaultDeviceIndex()
        {
            Int32 deviceIndex = _WTInfoA_OBJECT<Int32>(
                    (uint)EWTICategoryIndex.WTI_DEFCONTEXT,
                    (uint)EWTIContextIndex.CTX_DEVICE);

            return deviceIndex;
        }



        /// <summary>
        /// Returns the WintabAxis object for specified device and dimension.
        /// </summary>
        /// <param name="devIndex_I">Device index (-1 = virtual device)</param>
        /// <param name="dim_I">Dimension: AXIS_X, AXIS_Y or AXIS_Z</param>
        /// <returns></returns>
        public static WintabAxis GetDeviceAxis(Int32 devIndex_I, EAxisDimension dim_I)
        {
            var a = _WTInfoA_OBJECT<WintabAxis>((uint)(EWTICategoryIndex.WTI_DEVICES + devIndex_I),
                    (uint)dim_I);
            return a;
        }

        /// <summary>
        /// Returns a 3-element array describing the tablet's orientation range and resolution capabilities.
        /// </summary>
        /// <returns></returns>
        public static WintabAxisArray GetDeviceOrientation( out bool tiltSupported_O )
        {
            tiltSupported_O = false;

            var aa = _WTInfoA_OBJECT<WintabAxisArray>((uint)EWTICategoryIndex.WTI_DEVICES,
                    (uint)EWTIDevicesIndex.DVC_ORIENTATION);

            // If size == 0, then returns a zeroed struct.
            tiltSupported_O = (aa.array[0].axResolution != 0 && aa.array[1].axResolution != 0);
            return aa;
        }


        /// <summary>
        /// Returns a 3-element array describing the tablet's rotation range and resolution capabilities
        /// </summary>
        /// <returns></returns>
        public static WintabAxisArray GetDeviceRotation(out bool rotationSupported_O)
        {
            rotationSupported_O = false;

            var aa = _WTInfoA_OBJECT<WintabAxisArray>((uint)EWTICategoryIndex.WTI_DEVICES,
                    (uint)EWTIDevicesIndex.DVC_ROTATION);
            rotationSupported_O = (aa.array[0].axResolution != 0 && aa.array[1].axResolution != 0);
            return aa;
        }

        /// <summary>
        /// Returns the number of devices connected.
        /// </summary>
        /// <returns></returns>
        public static UInt32 GetNumberOfDevices()
        {
            UInt32 numdevices =
                _WTInfoA_OBJECT<UInt32>((uint)EWTICategoryIndex.WTI_INTERFACE,
                    (uint)EWTIInterfaceIndex.IFC_NDEVICES);
            return numdevices;
        }

        /// <summary>
        /// Returns whether a stylus is currently connected to the active cursor.
        /// </summary>
        /// <returns></returns>
        public static bool IsStylusActive()
        {
            using (var ub = UnmanagedBuffer.ForObjectType<bool>())
            {
                int size = (int)CWintabFuncs.WTInfoA(
                    (uint)EWTICategoryIndex.WTI_INTERFACE,
                    (uint)EWTIInterfaceIndex.IFC_NDEVICES, ub.BufferPointer);

                bool isStylusActive = ub.GetValueObject<bool>(size);
                return isStylusActive;
            }
        }


        /// <summary>
        /// Returns a string containing the name of the selected stylus. 
        /// </summary>
        /// <param name="index_I">indicates stylus type</param>
        /// <returns></returns>
        public static string GetStylusName(EWTICursorNameIndex index_I)
        {
            string s = _WTInfoA_STRING(
                (uint)index_I,
                (uint)EWTICursorsIndex.CSR_NAME);

            return s;

        }




        /// <summary>
        /// Return max normal pressure supported by tablet.
        /// </summary>
        /// <param name="getNormalPressure_I">TRUE=> normal pressure; 
        /// FALSE=> tangential pressure (not supported on all tablets)</param>
        /// <returns>maximum pressure value or zero on error</returns>
        public static Int32 GetMaxPressure(bool getNormalPressure_I = true)
        {
            EWTIDevicesIndex devIdx = (getNormalPressure_I ?
                    EWTIDevicesIndex.DVC_NPRESSURE : EWTIDevicesIndex.DVC_TPRESSURE);

            var a = _WTInfoA_OBJECT<WintabAxis>(
                (uint)EWTICategoryIndex.WTI_DEVICES,
                (uint)devIdx);
            return a.axMax;
        }



        /// <summary>
        /// Return the WintabAxis object for the specified dimension.
        /// </summary>
        /// <param name="dimension_I">Dimension to fetch (eg: x, y)</param>
        /// <returns></returns>
        public static WintabAxis GetTabletAxis(EAxisDimension dimension_I)
        {
            var a = _WTInfoA_OBJECT<WintabAxis>(
                (uint)EWTICategoryIndex.WTI_DEVICES
               , (uint)dimension_I);
            return a;

        }

        public static string _WTInfoA_STRING(uint cat, uint index)
        {
            using (var ub = UnmanagedBuffer.ForStringType())
            {
                int size = (int)CWintabFuncs.WTInfoA(cat, index, ub.BufferPointer);
                string s = ub.GetValueString(size);
                return s;
            }
        }

        public static T _WTInfoA_OBJECT<T>(uint cat, uint index) where T :new()
        {
            using (var ub = UnmanagedBuffer.ForObjectType<T>())
            {
                int size = (int)CWintabFuncs.WTInfoA(cat, index, ub.BufferPointer);
                T s = ub.GetValueObject<T>(size);
                return s;
            }
        }
    }

}
