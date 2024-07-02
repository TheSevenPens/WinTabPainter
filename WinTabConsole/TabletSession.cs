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
