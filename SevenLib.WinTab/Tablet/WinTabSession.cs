
using SevenLib.WinTab.Stylus;
using System;

namespace SevenLib.WinTab.Tablet;


public class WinTabSession : System.IDisposable
{

    public SevenLib.WinTab.Tablet.PointerState PointerState = new SevenLib.WinTab.Tablet.PointerState();

    public SevenLib.WinTab.CWintabContext Context = null;
    public SevenLib.WinTab.CWintabData Data = null;
    public TabletInfo TabletInfo;
    public TabletContextType ContextType;
    public System.Action<SevenLib.WinTab.Structs.WintabPacket> OnRawPacketReceived = null;
    public System.Action<SevenLib.WinTab.Structs.WintabPacket, StylusButtonChange> OnRawButtonChanged = null;

    public Action _onPointerStatsUpdated;
    public Action _PointerUpCallback;
    public Action _PointerDownCallback;
    public Action _PointerUpdateCallback;


    public SevenLib.Stylus.StylusButtonState StylusButtonState;

    public WinTabSession()
    {
        this.TabletInfo = new TabletInfo();
        this.PointerState = new SevenLib.WinTab.Tablet.PointerState();
        this.StylusButtonState = new SevenLib.Stylus.StylusButtonState(0); // Initialize to indicate no buttons are pressed
    }

    public void Open(TabletContextType context_type)
    {
        // convert the context type to something wintab understands
        var wt_context_type = context_type_to_index(context_type);

        // CREATE CONTEXT
        var options = SevenLib.WinTab.Enums.ECTXOptionValues.CXO_MESSAGES;
        this.Context = SevenLib.WinTab.CWintabInfo.GetDefaultContext(wt_context_type, options);

        if (this.Context == null)
        {
            throw new System.ApplicationException("Failed to get digitizing context");
        }

        this.Context.Options |= (uint)SevenLib.WinTab.Enums.ECTXOptionValues.CXO_SYSTEM;

        // Move origin from lower-left to upper left so it matches screen origin
        this.Context.OutExtY = -this.Context.OutExtY;
        var status = this.Context.Open();

        // CREATE DATA

        this.Data = new SevenLib.WinTab.CWintabData(this.Context);


        this.TabletInfo.Initialize();

        // HANDLER

        if (this.OnRawPacketReceived != null)
        {
            this.Data.SetWTPacketEventHandler(WinTabPacketHandler);
        }
    }

    private static SevenLib.WinTab.Enums.EWTICategoryIndex context_type_to_index(TabletContextType context_type)
    {
        return context_type switch
        {
            TabletContextType.System => SevenLib.WinTab.Enums.EWTICategoryIndex.WTI_DEFSYSCTX,
            TabletContextType.Digitizer => SevenLib.WinTab.Enums.EWTICategoryIndex.WTI_DEFCONTEXT,
            _ => throw new System.ArgumentOutOfRangeException()
        };
    }

    private void WinTabPacketHandler(System.Object sender, SevenLib.WinTab.WinForms.MessageReceivedEventArgs args)
    {
        if (this.Data == null)
        {
            // this situation can occur when the pen itself was used to close the windows form
            // do nothing in this case
            return;
        }

        uint pktId = (uint)args.Message.WParam;

        if (pktId == 0)
        {
            return;
        }

        var wintab_pkt = this.Data.GetDataPacket((uint)args.Message.LParam, pktId);

        if (wintab_pkt.pkContext == this.Context.HCtx)
        {
            var button_info = new StylusButtonChange(wintab_pkt.pkButtons);

            if (button_info.Change != StylusButtonChangeType.NoChange)
            {
                // there's been some change to the buttons
                this.StylusButtonState = StylusUtils.Update(this.StylusButtonState, button_info);
            }


            if (button_info.Change != StylusButtonChangeType.NoChange)
            {
                this.OnRawButtonChanged?.Invoke(wintab_pkt, button_info);
            }

            if (this.OnRawPacketReceived != null) 
            {
                this.OnRawPacketReceived(wintab_pkt);
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
        System.GC.SuppressFinalize(this);
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
                this.Context.Close(); // Close() calls Dispose() internally 
                this.Context = null;
            }
        }
    }
}
