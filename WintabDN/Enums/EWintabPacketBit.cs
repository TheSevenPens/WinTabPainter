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
namespace WintabDN.Enums;

/// <summary>
/// Wintab Packet bits.
/// </summary>
public enum EWintabPacketBit
{
	    PK_CONTEXT			= 0x0001,	/* reporting context */
	    PK_STATUS			= 0x0002,	/* status bits */
	    PK_TIME				= 0x0004,	/* time stamp */
	    PK_CHANGED			= 0x0008,	/* change bit vector */
	    PK_SERIAL_NUMBER	= 0x0010,	/* packet serial number */
	    PK_CURSOR			= 0x0020,	/* reporting cursor */
	    PK_BUTTONS			= 0x0040,	/* button information */
	    PK_X				= 0x0080,	/* x axis */
	    PK_Y				= 0x0100,	/* y axis */
	    PK_Z				= 0x0200,	/* z axis */
	    PK_NORMAL_PRESSURE	= 0x0400,	/* normal or tip pressure */
	    PK_TANGENT_PRESSURE	= 0x0800,	/* tangential or barrel pressure */
	    PK_ORIENTATION		= 0x1000,	/* orientation info: tilts */
   PK_PKTBITS_ALL      = 0x1FFF    // The Full Monty - all the bits execept Rotation - not supported
}
