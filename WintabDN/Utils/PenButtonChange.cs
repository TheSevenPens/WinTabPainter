using System;

namespace WinTabDN.Utils;


public struct PenButtonChange
{
    public readonly PenButtonChangeType Change;
    public readonly PenButtonChangeButtonId ButtonId;

    public PenButtonChange(UInt32 pkt_button)
    {
        UInt16 button_id = (UInt16)((pkt_button & 0x0000FFFF) >> 0);
        UInt16 press_change = (UInt16)((pkt_button & 0xFFFF0000) >> 16);

        this.Change = press_change switch
        {
            0 => PenButtonChangeType.NoChange,
            1 => PenButtonChangeType.Released,
            2 => PenButtonChangeType.Pressed,
            _ => throw new System.ArgumentOutOfRangeException()
        };

        this.ButtonId = button_id switch
        {
            0 => PenButtonChangeButtonId.Tip,
            1 => PenButtonChangeButtonId.LowerButton,
            2 => PenButtonChangeButtonId.UpperButton,
            _ => throw new System.ArgumentOutOfRangeException()
        };
    }

    public override string ToString()
    {
        string s = string.Format("({0},{1})", this.ButtonId, this.Change);
        return s;
    }

}