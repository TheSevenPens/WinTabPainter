namespace WinTabUtils;

public class WinTabSession
{

    public WintabDN.CWintabContext Context = null;
    public WintabDN.CWintabData Data = null;
    public TabletInfo TabletInfo;
    public WinTabSession()
    {
        this.TabletInfo = new TabletInfo();
    }

    public void Start()
    {
        this.Context = this.OpenTabletContext();
    }

    private WintabDN.CWintabContext OpenTabletContext()
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

        TabletInfo.Initialize();


        // In Wintab, the tablet origin is lower left.  Move origin to upper left
        // so that it coincides with screen origin.

        context.OutExtY = -context.OutExtY;

        var status = context.Open();
        this.Data = new WintabDN.CWintabData(context);

        return context;
    }



    public void Stop()
    {
        this.CloseTabletContext();
    }
    private void CloseTabletContext()
    {
        this.Data.ClearWTPacketEventHandler();
        this.Data = null;

        if (this.Context == null)
        {
            return;
        }

        this.Context.Close();
        this.Context = null;
    }


}
