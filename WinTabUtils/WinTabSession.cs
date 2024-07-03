namespace WinTabUtils;

public enum TabletContextType
{
    Digitizer,
    System
}

public class WinTabSession
{

    public WintabDN.CWintabContext Context = null;
    public WintabDN.CWintabData Data = null;
    public TabletInfo TabletInfo;
    public TabletContextType ContextType;
    public WinTabSession()
    {
        this.TabletInfo = new TabletInfo();
    }

    public void Open(TabletContextType ct)
    {
        // WintabDN.EWTICategoryIndex.WTI_DEFSYSCTX - System context
        // WintabDN.EWTICategoryIndex.WTI_DEFCONTEXT - Digitizer context
        // var context_type = WintabDN.EWTICategoryIndex.WTI_DEFCONTEXT;
        // WintabDN.EWTICategoryIndex context_type = WintabDN.EWTICategoryIndex.WTI_DEFSYSCTX;

        WintabDN.EWTICategoryIndex context_type = ct switch
        {
            TabletContextType.System => WintabDN.EWTICategoryIndex.WTI_DEFSYSCTX,
            TabletContextType.Digitizer => WintabDN.EWTICategoryIndex.WTI_DEFCONTEXT,
            _ => throw new System.ArgumentOutOfRangeException()
        };


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

        this.Context= context;
    }

    public void Close()
    {
        if (this.Data != null)
        {
            this.Data.ClearWTPacketEventHandler();
            this.Data = null;
        }

        if (this.Context != null)
        {
            this.Context.Close();
            this.Context = null;
        }
    }
}
