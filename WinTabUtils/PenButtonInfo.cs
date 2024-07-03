namespace WinTabUtils;

public struct PenButtonInfo
{
    public readonly UInt32 PacketButtons;
    public readonly UInt16 Id;
    public readonly PenButtonStatus PressStatus;
    public readonly PenButtonType Type;

    public PenButtonInfo(UInt32 pkt_button)
    {
        this.PacketButtons = pkt_button;
        this.Id = (UInt16)((pkt_button & 0x0000FFFF) >> 0);
        UInt16 press_status = (UInt16)((pkt_button & 0xFFFF0000) >> 16);

        this.PressStatus = press_status switch
        {
            0 => PenButtonStatus.NoChange,
            1 => PenButtonStatus.Released,
            2 => PenButtonStatus.Pressed,
            _ => throw new System.ArgumentOutOfRangeException()
        };

        this.Type = this.Id switch
        {
            0 => PenButtonType.Tip,
            1 => PenButtonType.LowerButton,
            2 => PenButtonType.UpperButton,
            _ => throw new System.ArgumentOutOfRangeException()
        };
    }

    public override string ToString()
    {
        string s = string.Format("({0},{1})", this.Type, this.PressStatus);
        return s;
    }

}