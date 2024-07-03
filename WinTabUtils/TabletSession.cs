namespace WinTabUtils;

public class TabletSession
{

    public WintabDN.CWintabContext wintab_context = null;
    public WintabDN.CWintabData wintab_data = null;
    public TabletInfo tablet_info;
    public TabletSession()
    {
        this.tablet_info = new TabletInfo();



    }

    public void Start()
    {
        this.wintab_context = this.OpenTabletContext();

    }

    public WintabDN.CWintabContext OpenTabletContext()
    {
        // WintabDN.EWTICategoryIndex.WTI_DEFSYSCTX - System context
        // WintabDN.EWTICategoryIndex.WTI_DEFCONTEXT - Digitizer context
        //var context_type = WintabDN.EWTICategoryIndex.WTI_DEFCONTEXT;
        var context_type = WintabDN.EWTICategoryIndex.WTI_DEFSYSCTX;

        var options = WintabDN.ECTXOptionValues.CXO_MESSAGES;
        var context = WintabDN.CWintabInfo.GetDefaultContext(context_type, options);

        if (context == null)
        {
            System.Windows.Forms.MessageBox.Show("Failed to get digitizing context");
        }

        context.Options |= (uint)WintabDN.ECTXOptionValues.CXO_SYSTEM;

        tablet_info.Initialize();


        // In Wintab, the tablet origin is lower left.  Move origin to upper left
        // so that it coincides with screen origin.

        context.OutExtY = -context.OutExtY;

        var status = context.Open();
        this.wintab_data = new WintabDN.CWintabData(context);
        this.wintab_data.SetWTPacketEventHandler(WinTabPacketHandler);

        return context;
    }


    private void WinTabPacketHandler(Object sender, WintabDN.MessageReceivedEventArgs args)
    {
        if (this.wintab_data == null)
        {
            // this case can happen when you use the pen to close the window
            // by clicking x in the upper right of the window
            return;
        }

        uint pktId = (uint)args.Message.WParam;
        var wintab_pkt = this.wintab_data.GetDataPacket((uint)args.Message.LParam, pktId);

        if (wintab_pkt.pkContext == wintab_context.HCtx)
        {
            Console.WriteLine("Packet");
            // collect all the information we need to start painting
        }
    }

    public void Stop()
    {
        this.CloseTabletContext();
    }
    private void CloseTabletContext()
    {
        this.wintab_data.ClearWTPacketEventHandler();
        this.wintab_data = null;

        if (this.wintab_context == null)
        {
            return;
        }

        this.wintab_context.Close();
        this.wintab_context = null;
    }


}


public enum ButtonPressStatus
{
    NoPress,
    Down,
    Up
}

public enum ButtonType
{
    Tip,
    LowerButton,
    UpperButton
}

public struct ButtonInfo
{
    public readonly UInt32 PacketButtons;
    public readonly UInt16 Id;
    public readonly ButtonPressStatus PressStatus;
    public readonly ButtonType Type;

    public ButtonInfo(UInt32 pkt_button)
    {
        this.PacketButtons = pkt_button;
        this.Id = (UInt16)((pkt_button & 0x0000FFFF) >> 0);
        UInt16 press_status = (UInt16)((pkt_button & 0xFFFF0000) >> 16);

        if (press_status == 0)
        {
            this.PressStatus = ButtonPressStatus.NoPress;
        }
        else if (press_status == 1)
        {
            this.PressStatus = ButtonPressStatus.Up;
        }
        else if (press_status == 2)
        {
            this.PressStatus = ButtonPressStatus.Down;
        }
        else
        {
            throw new System.ArgumentOutOfRangeException();
        }

        if (this.Id == 0)
        {
            this.Type = ButtonType.Tip;
        }
        else if (this.Id == 1)
        {
            this.Type = ButtonType.LowerButton;
        }
        else if (this.Id == 2)
        {
            this.Type = ButtonType.UpperButton;
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