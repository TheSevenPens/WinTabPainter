using System;

namespace WinTab.Utils;


public struct StylusButtonChange
{
    public readonly StylusButtonChangeType Change;
    public readonly StylusButtonId ButtonId;

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
            0 => StylusButtonId.Tip,
            1 => StylusButtonId.LowerButton,
            2 => StylusButtonId.UpperButton,
            3 => StylusButtonId.BarrelButton,
            _ => throw new System.ArgumentOutOfRangeException()
        };
    }

    public override string ToString()
    {
        string s = string.Format("({0},{1})", this.ButtonId, this.Change);
        return s;
    }

}