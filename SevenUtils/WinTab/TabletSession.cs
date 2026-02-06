
namespace SevenUtils.WinTab;

public class TabletSession : IDisposable
{

    public WinTabDN.CWintabContext Context = null;
    public WinTabDN.CWintabData Data = null;
    public TabletInfo TabletInfo;
    public SevenUtils.WinTab.TabletContextType ContextType;
    public System.Action<WinTabDN.Structs.WintabPacket> PacketHandler = null;
    public System.Action<WinTabDN.Structs.WintabPacket, SevenUtils.WinTab.PenButtonPressChange> ButtonChangedHandler = null;

    public TabletSession()
    {
        this.TabletInfo = new TabletInfo();
    }

    public void Open(SevenUtils.WinTab.TabletContextType context_type)
    {
        // convert the context type to something wintab understands
        var wt_context_type = context_type_to_index(context_type);

        // CREATE CONTEXT
        var options = WinTabDN.Enums.ECTXOptionValues.CXO_MESSAGES;
        this.Context = WinTabDN.CWintabInfo.GetDefaultContext(wt_context_type, options);

        if (this.Context == null)
        {
            throw new System.ApplicationException("Failed to get digitizing context");
        }

        this.Context.Options |= (uint)WinTabDN.Enums.ECTXOptionValues.CXO_SYSTEM;

        // Move origin from lower-left to upper left so it matches screen origin
        this.Context.OutExtY = -this.Context.OutExtY;
        var status = this.Context.Open();

        // CREATE DATA

        this.Data = new WinTabDN.CWintabData(this.Context);


        this.TabletInfo.Initialize();

        // HANDLER

        if (this.PacketHandler != null)
        {
            this.Data.SetWTPacketEventHandler(WinTabPacketHandler);
        }
    }

    private static WinTabDN.Enums.EWTICategoryIndex context_type_to_index(SevenUtils.WinTab.TabletContextType context_type)
    {
        return context_type switch
        {
            SevenUtils.WinTab.TabletContextType.System => WinTabDN.Enums.EWTICategoryIndex.WTI_DEFSYSCTX,
            SevenUtils.WinTab.TabletContextType.Digitizer => WinTabDN.Enums.EWTICategoryIndex.WTI_DEFCONTEXT,
            _ => throw new System.ArgumentOutOfRangeException()
        };
    }

    private void WinTabPacketHandler(Object sender, WinTabDN.WinForms.MessageReceivedEventArgs args)
    {
        if (this.Data == null)
        {
            // this situation can occur when the pen itself was used to close the windows form
            // do nothing in this case
            return;
        }

        uint pktId = (uint)args.Message.WParam;
        var wintab_pkt = this.Data.GetDataPacket((uint)args.Message.LParam, pktId);

        if (wintab_pkt.pkContext == this.Context.HCtx)
        {
            var button_info = new SevenUtils.WinTab.PenButtonPressChange(wintab_pkt.pkButtons);
            if (button_info.Change != WinTab.PenButtonPressChangeType.NoChange)
            {
                this.ButtonChangedHandler?.Invoke(wintab_pkt, button_info);

            }

            if (this.PacketHandler != null) 
            {
                this.PacketHandler(wintab_pkt);
            }
        }
    }

    public void Close()
    {
        Dispose();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (this.Data != null)
            {
                this.Data.ClearWTPacketEventHandler();
                this.Data.Dispose();
                this.Data = null;
            }

            if (this.Context != null)
            {
                this.Context.Close(); // Close() calls Dispose() internally now
                this.Context = null;
            }
        }
    }
}
