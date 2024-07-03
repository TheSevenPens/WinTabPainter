namespace WinTabUtils;

public struct PenButtonPressChange
{
    public readonly PenButtonPressChangeType Change;
    public readonly PenButtonIdentifier ButtonId;

    public PenButtonPressChange(UInt32 pkt_button)
    {
        UInt16 button_id = (UInt16)((pkt_button & 0x0000FFFF) >> 0);
        UInt16 press_change = (UInt16)((pkt_button & 0xFFFF0000) >> 16);

        this.Change = press_change switch
        {
            0 => PenButtonPressChangeType.NoChange,
            1 => PenButtonPressChangeType.Released,
            2 => PenButtonPressChangeType.Pressed,
            _ => throw new System.ArgumentOutOfRangeException()
        };

        this.ButtonId = button_id switch
        {
            0 => PenButtonIdentifier.Tip,
            1 => PenButtonIdentifier.LowerButton,
            2 => PenButtonIdentifier.UpperButton,
            _ => throw new System.ArgumentOutOfRangeException()
        };
    }

    public override string ToString()
    {
        string s = string.Format("({0},{1})", this.ButtonId, this.Change);
        return s;
    }

}