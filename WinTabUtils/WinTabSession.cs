namespace WinTabUtils;

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

    public void Open(TabletContextType context_type)
    {
        // convert the context type to something wintab understands

        WintabDN.EWTICategoryIndex wt_context_type = context_type switch
        {
            TabletContextType.System => WintabDN.EWTICategoryIndex.WTI_DEFSYSCTX,
            TabletContextType.Digitizer => WintabDN.EWTICategoryIndex.WTI_DEFCONTEXT,
            _ => throw new System.ArgumentOutOfRangeException()
        };

        // CREATE CONTEXT
        var options = WintabDN.ECTXOptionValues.CXO_MESSAGES;

        this.Context = WintabDN.CWintabInfo.GetDefaultContext(wt_context_type, options);

        if (this.Context == null)
        {
            throw new System.ApplicationException("Failed to get digitizing context");
        }

        this.Context.Options |= (uint)WintabDN.ECTXOptionValues.CXO_SYSTEM;


        // Move origin from lower-left to upper left to it matches screen origin
        this.Context.OutExtY = -this.Context.OutExtY; 
        var status = this.Context.Open();

        // CREATE DATA

        this.Data = new WintabDN.CWintabData(this.Context);


        this.TabletInfo.Initialize();

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
