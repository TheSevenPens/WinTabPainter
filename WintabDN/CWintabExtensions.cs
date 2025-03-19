///////////////////////////////////////////////////////////////////////////////
// CWintabExtensions.cs - Wintab extensions access for WintabDN
//
// Copyright (c) 2013, Wacom Technology Corporation
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
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace WintabDN;


/// <summary>
/// API for Wintab extensions functionality (Wintab 1.4).
/// 
/// Wintab Extensions provides support for overriding tablet control functionality with
/// functionality provided by an application. The tablet controls that can be 
/// overriden with extensions are: Express Keys, Touch Rings and Touch Strips.
/// 
/// For example, an application can respond to an Express Key button press by
/// defining what action should occur within that application when the button is pressed.
/// Similarly, an application can define actions for Touch Ring and Touch Strip button
/// modes, and respond to user swipes on those controls to provide customized behavior.
/// </summary>
public class CWintabExtensions
{
    /// <summary>
    /// Return the extension mask for the given tag.
    /// </summary>
    /// <param name="tag_I">type of extension being searched for</param>
    /// <returns>0xFFFFFFFF on error</returns>
    public static UInt32 GetWTExtensionMask(Enums.EWTXExtensionTag tag_I)
    {
        UInt32 extMask = 0;           
        UInt32 extIndex = FindWTExtensionIndex(tag_I);
    
        // Supported if extIndex != -1
        if (extIndex != 0xFFFFFFFF)
        {
            extMask = CWintabFuncs.WTInfoAObject<UInt32>((uint)Enums.EWTICategoryIndex.WTI_EXTENSIONS + (uint)extIndex,
                (uint)Enums.EWTIExtensionIndex.EXT_MASK);
        }
            
        return extMask;
    }

    /// <summary>
    /// Returns extension index tag for given tag, if possible.
    /// </summary>
    /// <param name="tag_I">type of extension being searched for</param>
    /// <returns>0xFFFFFFFF on error</returns>
    public static UInt32 FindWTExtensionIndex(Enums.EWTXExtensionTag tag_I)
    {
        UInt32 thisTag = 0;
        UInt32 extIndex = 0xFFFFFFFF;

        using (var buf = Interop.UnmanagedBuffer.CreateForObject<UInt32>())
        {

            for (Int32 loopIdx = 0, size = -1; size != 0; loopIdx++)
            {
                size = (int)CWintabFuncs.WTInfoA(
                    (uint)Enums.EWTICategoryIndex.WTI_EXTENSIONS + (UInt32)loopIdx,
                    (uint)Enums.EWTIExtensionIndex.EXT_TAG, buf.Pointer);

                if (size > 0)
                {
                    thisTag = buf.MarshallFromBuffer<UInt32>();

                    if ((Enums.EWTXExtensionTag)thisTag == tag_I)
                    {
                        extIndex = (UInt32)loopIdx;
                        break;
                    }
                }
            }


            return extIndex;
        }
    }


    /// <summary>
    /// Get a property value from an extension.
    /// </summary>
    /// <param name="context_I">Wintab context</param>
    /// <param name="extTagIndex_I">extension index tag</param>
    /// <param name="tabletIndex_I">tablet index</param>
    /// <param name="controlIndex_I">control index on the tablet</param>
    /// <param name="functionIndex_I">function index on the control</param>
    /// <param name="propertyID_I">ID of the property requested</param>
    /// <param name="result_O">value of the property requested</param>
    /// <returns>true if property obtained</returns>
    public static bool ControlPropertyGet(
        Structs.HCTX context_I,
        byte extTagIndex_I,
        byte tabletIndex_I,
        byte controlIndex_I,
        byte functionIndex_I,
        ushort propertyID_I,
        ref UInt32 result_O
        )
    {
        bool retStatus = false;
        var extProperty = new Structs.WTExtensionProperty();

        using (var buf = WintabDN.Interop.UnmanagedBuffer.CreateForObject<WintabDN.Structs.WTExtensionProperty>())
        {

            extProperty.extBase.version = 0;
            extProperty.extBase.tabletIndex = tabletIndex_I;
            extProperty.extBase.controlIndex = controlIndex_I;
            extProperty.extBase.functionIndex = functionIndex_I;
            extProperty.extBase.propertyID = propertyID_I;
            extProperty.extBase.reserved = 0;
            extProperty.extBase.dataSize = (uint)System.Runtime.InteropServices.Marshal.SizeOf(result_O);

            buf.MarshallIntoBuffer(extProperty);

            bool status = CWintabFuncs.WTExtGet((UInt32)context_I, (UInt32)extTagIndex_I, buf.Pointer);

            if (status)
            {
                var retProp = buf.MarshallFromBuffer<Structs.WTExtensionProperty>();
                result_O = retProp.data[0];
                retStatus = true;
            }


        }
        return retStatus;
    }



    /// <summary>
    /// Sets an extension control property value.
    /// </summary>
    /// <param name="context_I">wintab context</param>
    /// <param name="extTagIndex_I">which extension tag we're setting</param>
    /// <param name="tabletIndex_I">index of the tablet being set</param>
    /// <param name="controlIndex_I">the index of the control being set</param>
    /// <param name="functionIndex_I">the index of the control function being set</param>
    /// <param name="propertyID_I">ID of the property being set</param>
    /// <param name="value_I">value of the property being set</param>
    /// <returns>true if successful</returns>
    public static bool ControlPropertySet(
        Structs.HCTX context_I,
        byte extTagIndex_I,
        byte tabletIndex_I,
        byte controlIndex_I,
        byte functionIndex_I,
        ushort propertyID_I,
        UInt32 value_I
    )
    {
        bool retStatus = false;
        var extProperty = new Structs.WTExtensionProperty();

        using (var buf = WintabDN.Interop.UnmanagedBuffer.CreateForObject<Structs.WTExtensionProperty>())
        { 


        byte[] valueBytes = BitConverter.GetBytes(value_I);

        extProperty.extBase.version = 0;
        extProperty.extBase.tabletIndex = tabletIndex_I;
        extProperty.extBase.controlIndex = controlIndex_I;
        extProperty.extBase.functionIndex = functionIndex_I;
        extProperty.extBase.propertyID = propertyID_I;
        extProperty.extBase.reserved = 0;
        extProperty.extBase.dataSize = (uint)System.Runtime.InteropServices.Marshal.SizeOf(value_I);
        extProperty.data = new byte[Structs.WTExtensionsGlobal.WTExtensionPropertyMaxDataBytes];

        // Send input value as an array of bytes.
        System.Buffer.BlockCopy(valueBytes, 0, extProperty.data, 0, (int)extProperty.extBase.dataSize);

        buf.MarshallIntoBuffer(extProperty.data);

        retStatus = CWintabFuncs.WTExtSet((UInt32)context_I, (UInt32)extTagIndex_I, buf.Pointer);

        return retStatus;
        }
    }



    /// <summary>
    /// Sets an extension control property string.
    /// </summary>
    /// <param name="context_I">wintab context</param>
    /// <param name="extTagIndex_I">which extension tag we're setting</param>
    /// <param name="tabletIndex_I">index of the tablet being set</param>
    /// <param name="controlIndex_I">the index of the control being set</param>
    /// <param name="functionIndex_I">the index of the control function being set</param>
    /// <param name="propertyID_I">ID of the property being set</param>
    /// <param name="value_I">value of the property being set (a string)</param>
    /// <returns>true if successful</returns>
    public static bool ControlPropertySet(
        Structs.HCTX context_I,
        byte extTagIndex_I,
        byte tabletIndex_I,
        byte controlIndex_I,
        byte functionIndex_I,
        ushort propertyID_I,
        String value_I
        )
    {
        bool retStatus = false;
        var extProperty = new Structs.WTExtensionProperty();

        using (var buf = WintabDN.Interop.UnmanagedBuffer.CreateForObject<Structs.WTExtensionProperty>())
        {

            // Convert unicode string value_I to UTF8-encoded bytes
            byte[] utf8Bytes = System.Text.Encoding.Convert(Encoding.Unicode, Encoding.UTF8, Encoding.Unicode.GetBytes(value_I));

            extProperty.extBase.version = 0;
            extProperty.extBase.tabletIndex = tabletIndex_I;
            extProperty.extBase.controlIndex = controlIndex_I;
            extProperty.extBase.functionIndex = functionIndex_I;
            extProperty.extBase.propertyID = propertyID_I;
            extProperty.extBase.reserved = 0;
            extProperty.extBase.dataSize = (uint)utf8Bytes.Length;
            extProperty.data = new byte[Structs.WTExtensionsGlobal.WTExtensionPropertyMaxDataBytes];

            // Send input value as an array of UTF8-encoded bytes.
            System.Buffer.BlockCopy(utf8Bytes, 0, extProperty.data, 0, (int)extProperty.extBase.dataSize);

            buf.MarshallIntoBuffer(extProperty);

            retStatus = CWintabFuncs.WTExtSet((UInt32)context_I, (UInt32)extTagIndex_I, buf.Pointer);

        return retStatus;
        }
    }



    /// <summary>
    /// Sets an extension control property image (if supported by tablet).
    /// </summary>
    /// <param name="context_I">wintab context</param>
    /// <param name="extTagIndex_I">which extension tag we're setting</param>
    /// <param name="tabletIndex_I">index of the tablet being set</param>
    /// <param name="controlIndex_I">the index of the control being set</param>
    /// <param name="functionIndex_I">the index of the control function being set</param>
    /// <param name="propertyID_I">ID of the property being set</param>
    /// <param name="value_I">value of the property being set (a string)</param>
    /// <returns>true if successful</returns>
    public static bool ControlPropertySetImage(
        Structs.HCTX context_I,
        byte extTagIndex_I,
        byte tabletIndex_I,
        byte controlIndex_I,
        byte functionIndex_I,
        ushort propertyID_I,
        String imageFilePath_I
        )
    {
        bool retStatus = false;
        var extProperty = new Structs.WTExtensionImageProperty();

        using (var buf = WintabDN.Interop.UnmanagedBuffer.CreateForObject<Structs.WTExtensionImageProperty>())
        {

            //IntPtr buf = Interop.CMemUtils.AllocUnmanagedBuf(extProperty);


            byte[] imageBytes = null;
            System.Drawing.Image newImage = Image.FromFile(imageFilePath_I);

            if (newImage == null)
            {
                MessageBox.Show("Oops - couldn't find/read image: " + imageFilePath_I);
                return false;
            }

            using (MemoryStream ms = new MemoryStream())
            {
                newImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                imageBytes = ms.ToArray();
            }

            extProperty.extBase.version = 0;
            extProperty.extBase.tabletIndex = tabletIndex_I;
            extProperty.extBase.controlIndex = controlIndex_I;
            extProperty.extBase.functionIndex = functionIndex_I;
            extProperty.extBase.propertyID = propertyID_I;
            extProperty.extBase.reserved = 0;
            extProperty.extBase.dataSize = (uint)imageBytes.Length;
            extProperty.data = new byte[Structs.WTExtensionsGlobal.WTExtensionPropertyImageMaxDataBytes];

            // Send image as an array of bytes.
            System.Buffer.BlockCopy(imageBytes, 0, extProperty.data, 0, (int)extProperty.extBase.dataSize);

            buf.MarshallIntoBuffer(extProperty);

            retStatus = CWintabFuncs.WTExtSet((UInt32)context_I, (UInt32)extTagIndex_I, buf.Pointer);


            return retStatus;
        }
    }


    /// <summary>
    /// Set tablet OLED display property.
    /// </summary>
    /// <param name="context_I">wintab context</param>
    /// <param name="extTagIndex_I">which extension tag we're setting</param>
    /// <param name="tabletIndex_I">index of the tablet being set</param>
    /// <param name="controlIndex_I">the index of the control being set</param>
    /// <param name="functionIndex_I">the index of the control function being set</param>
    /// <param name="imageFilePath_I">path to PNG image file</param>
    /// <returns>true if successful and tablet supports property</returns>
    public static bool SetDisplayProperty(
        CWintabContext context_I,
        Enums.EWTXExtensionTag extTagIndex_I,
        UInt32 tabletIndex_I,
        UInt32 controlIndex_I,
        UInt32 functionIndex_I,
        String imageFilePath_I)
    {
        UInt32 iconFmt = 0;

        // Bail out if image file not found.
        if (imageFilePath_I == "" ||
             !System.IO.File.Exists(imageFilePath_I))
        {
            return false;
        }

        if (!CWintabExtensions.ControlPropertyGet(
            context_I.HCtx,
            (byte)extTagIndex_I,
            (byte)tabletIndex_I,
            (byte)controlIndex_I,
            (byte)functionIndex_I,
            (ushort)Enums.EWTExtensionTabletProperty.TABLET_PROPERTY_ICON_FORMAT,
            ref iconFmt))
        { throw new Exception("Oops - Failed ControlPropertyGet for TABLET_PROPERTY_ICON_FORMAT"); }

        if ((Enums.EWTExtensionIconProperty)iconFmt != Enums.EWTExtensionIconProperty.TABLET_ICON_FMT_NONE)
        {
            // Get the width and height of the display icon.
            UInt32 iconWidth = 0;
            UInt32 iconHeight = 0;

            if (!CWintabExtensions.ControlPropertyGet(
                context_I.HCtx,
                (byte)extTagIndex_I,
                (byte)tabletIndex_I,
                (byte)controlIndex_I,
                (byte)functionIndex_I,
                (ushort)Enums.EWTExtensionTabletProperty.TABLET_PROPERTY_ICON_WIDTH,
                ref iconWidth))
            { throw new Exception("Oops - Failed ControlPropertyGet for TABLET_PROPERTY_ICON_WIDTH"); }

            if (!CWintabExtensions.ControlPropertyGet(
                context_I.HCtx,
                (byte)extTagIndex_I,
                (byte)tabletIndex_I,
                (byte)controlIndex_I,
                (byte)functionIndex_I,
                (ushort)Enums.EWTExtensionTabletProperty.TABLET_PROPERTY_ICON_HEIGHT,
                ref iconHeight))
            { throw new Exception("Oops - Failed ControlPropertyGet for TABLET_PROPERTY_ICON_HEIGHT"); }

            return SetIcon(context_I, extTagIndex_I, tabletIndex_I, controlIndex_I, functionIndex_I, imageFilePath_I);
        }


        // Not supported by tablet.
        return false;
    }

    /// <summary>
    /// Write out an image to a tablet's OLED (Organic Light Emitting Diode)
    /// if supported by the tablet (eg: Intuos4).
    /// </summary>
    /// <param name="context_I">wintab context</param>
    /// <param name="extTagIndex_I">which extension tag we're setting</param>
    /// <param name="tabletIndex_I">index of the tablet being set</param>
    /// <param name="controlIndex_I">the index of the control being set</param>
    /// <param name="functionIndex_I">the index of the control function being set</param>
    /// <param name="imageFilePath_I">path to PNG image file</param>
    private static bool SetIcon(
        CWintabContext context_I,
        Enums.EWTXExtensionTag extTagIndex_I,
        UInt32 tabletIndex_I,
        UInt32 controlIndex_I,
        UInt32 functionIndex_I,
        String imageFilePath_I)
    {
        try
        {
            if (!CWintabExtensions.ControlPropertySetImage(
                context_I.HCtx,
                (byte)extTagIndex_I,
                (byte)tabletIndex_I,
                (byte)controlIndex_I,
                (byte)functionIndex_I,
                (ushort)Enums.EWTExtensionTabletProperty.TABLET_PROPERTY_OVERRIDE_ICON,
                imageFilePath_I))
            { throw new Exception("Oops - FAILED ControlPropertySet for TABLET_PROPERTY_OVERRIDE"); }
        }
        catch (Exception ex)
        {
            throw ex;
        }

        return true;
    }

} // end class CWintabExtensions
// end namespace WintabDN
