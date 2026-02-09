// See copright.md for copyright information.


namespace WinTab.Enums;

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
