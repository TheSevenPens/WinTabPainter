using System;

namespace SevenLib.WinTab.Utils;


public struct StylusButtonChange
{
    public readonly StylusButtonChangeType Change;
    public readonly SevenLib.Stylus.StylusButtonId ButtonId;

    public StylusButtonChange(UInt32 pkt_button)
    {
        UInt16 button_id = (UInt16)((pkt_button & 0x0000FFFF) >> 0);
        UInt16 press_change = (UInt16)((pkt_button & 0xFFFF0000) >> 16);

        this.Change = press_change switch
        {
            0 => StylusButtonChangeType.NoChange,
            1 => StylusButtonChangeType.Released,
            2 => StylusButtonChangeType.Pressed,
            _ => throw new System.ArgumentOutOfRangeException()
        };

        this.ButtonId = button_id switch
        {
            0 => SevenLib.Stylus.StylusButtonId.Tip,
            1 => SevenLib.Stylus.StylusButtonId.LowerButton,
            2 => SevenLib.Stylus.StylusButtonId.UpperButton,
            3 => SevenLib.Stylus.StylusButtonId.BarrelButton,
            _ => throw new System.ArgumentOutOfRangeException()
        };
    }

    public override string ToString()
    {
        string s = string.Format("({0},{1})", this.ButtonId, this.Change);
        return s;
    }

}