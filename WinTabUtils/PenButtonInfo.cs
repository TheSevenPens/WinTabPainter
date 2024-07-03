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

        if (press_status == 0)
        {
            this.PressStatus = PenButtonStatus.NoChange;
        }
        else if (press_status == 1)
        {
            this.PressStatus = PenButtonStatus.Released;
        }
        else if (press_status == 2)
        {
            this.PressStatus = PenButtonStatus.Pressed;
        }
        else
        {
            throw new System.ArgumentOutOfRangeException();
        }

        if (this.Id == 0)
        {
            this.Type = PenButtonType.Tip;
        }
        else if (this.Id == 1)
        {
            this.Type = PenButtonType.LowerButton;
        }
        else if (this.Id == 2)
        {
            this.Type = PenButtonType.UpperButton;
        }
        else
        {
            throw new System.ArgumentOutOfRangeException();
        }

    }

    public override string ToString()
    {
        string s = string.Format("({0},{1})", this.Type, this.PressStatus);
        return s;
    }

}