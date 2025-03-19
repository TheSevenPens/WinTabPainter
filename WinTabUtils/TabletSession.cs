using System;

namespace WinTabUtils;

public class TabletSession
{

    public WintabDN.CWintabContext Context = null;
    public WintabDN.CWintabData Data = null;
    public TabletInfo TabletInfo;
    public TabletContextType ContextType;
    public System.Action<WintabDN.Structs.WintabPacket> PacketHandler = null;
    public System.Action<WintabDN.Structs.WintabPacket, WinTabUtils.PenButtonPressChange> ButtonChangedHandler = null;

    public TabletSession()
    {
        this.TabletInfo = new TabletInfo();
    }

    public void Open(TabletContextType context_type)
    {
        // convert the context type to something wintab understands
        var wt_context_type = context_type_to_index(context_type);

        // CREATE CONTEXT
        var options = WintabDN.Enums.ECTXOptionValues.CXO_MESSAGES;
        this.Context = WintabDN.CWintabInfo.GetDefaultContext(wt_context_type, options);

        if (this.Context == null)
        {
            throw new System.ApplicationException("Failed to get digitizing context");
        }

        this.Context.Options |= (uint)WintabDN.Enums.ECTXOptionValues.CXO_SYSTEM;

        // Move origin from lower-left to upper left to it matches screen origin
        this.Context.OutExtY = -this.Context.OutExtY;
        var status = this.Context.Open();

        // CREATE DATA

        this.Data = new WintabDN.CWintabData(this.Context);


        this.TabletInfo.Initialize();

        // HANDLER

        if (this.PacketHandler != null)
        {
            this.Data.SetWTPacketEventHandler(WinTabPacketHandler);
        }
    }

    private static WintabDN.Enums.EWTICategoryIndex context_type_to_index(TabletContextType context_type)
    {
        return context_type switch
        {
            TabletContextType.System => WintabDN.Enums.EWTICategoryIndex.WTI_DEFSYSCTX,
            TabletContextType.Digitizer => WintabDN.Enums.EWTICategoryIndex.WTI_DEFCONTEXT,
            _ => throw new System.ArgumentOutOfRangeException()
        };
    }

    private void WinTabPacketHandler(Object sender, WintabDN.WinForms.MessageReceivedEventArgs args)
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
            var button_info = new WinTabUtils.PenButtonPressChange(wintab_pkt.pkButtons);
            if (button_info.Change != PenButtonPressChangeType.NoChange)
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
