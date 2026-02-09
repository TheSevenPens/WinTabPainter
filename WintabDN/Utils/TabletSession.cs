
namespace WinTabDN.Utils;


public class TabletSession : System.IDisposable
{

    public WinTabDN.CWintabContext Context = null;
    public WinTabDN.CWintabData Data = null;
    public TabletInfo TabletInfo;
    public TabletContextType ContextType;
    public System.Action<WinTabDN.Structs.WintabPacket> PacketHandler = null;
    public System.Action<WinTabDN.Structs.WintabPacket, StylusButtonChange> ButtonChangedHandler = null;
    public uint StylusButtonState;

    public readonly uint STYLUS_BUTTON_TIP_MASK = 0x0001;
    public readonly uint STYLUS_BUTTON_LOWER_MASK = 0x0002;
    public readonly uint STYLUS_BUTTON_UPPER_MASK = 0x0004;
    public readonly uint STYLUS_BUTTON_BARREL_MASK = 0x0008;
    public TabletSession()
    {
        this.TabletInfo = new TabletInfo();
        this.StylusButtonState = 0; // Initialize to indicate no buttons are pressed
    }

    public void Open(TabletContextType context_type)
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

    private static WinTabDN.Enums.EWTICategoryIndex context_type_to_index(TabletContextType context_type)
    {
        return context_type switch
        {
            TabletContextType.System => WinTabDN.Enums.EWTICategoryIndex.WTI_DEFSYSCTX,
            TabletContextType.Digitizer => WinTabDN.Enums.EWTICategoryIndex.WTI_DEFCONTEXT,
            _ => throw new System.ArgumentOutOfRangeException()
        };
    }

    private void WinTabPacketHandler(System.Object sender, WinTabDN.WinForms.MessageReceivedEventArgs args)
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
            var button_info = new StylusButtonChange(wintab_pkt.pkButtons);

            if (button_info.Change != StylusButtonChangeType.NoChange)
            {
                // there's been some change to the buttons
                if (button_info.Change == StylusButtonChangeType.Pressed)
                {
                    if (button_info.ButtonId == StylusButtonId.Tip)
                    {
                        this.StylusButtonState |= STYLUS_BUTTON_TIP_MASK; // Set bit for button 1
                    }
                    else if (button_info.ButtonId == StylusButtonId.LowerButton)
                    {
                        this.StylusButtonState |= STYLUS_BUTTON_LOWER_MASK; // Set bit for button 2
                    }
                    else if (button_info.ButtonId == StylusButtonId.UpperButton)
                    {
                        this.StylusButtonState |= STYLUS_BUTTON_UPPER_MASK;
                    }
                }
                else if (button_info.Change == StylusButtonChangeType.Released)
                {
                    if (button_info.ButtonId == StylusButtonId.Tip)
                    {
                        this.StylusButtonState &= ~STYLUS_BUTTON_TIP_MASK; // Clear bit for button 1
                    }
                    else if (button_info.ButtonId == StylusButtonId.LowerButton)
                    {
                        this.StylusButtonState &= ~STYLUS_BUTTON_LOWER_MASK; // Clear bit for button 2
                    }
                    else if (button_info.ButtonId == StylusButtonId.UpperButton)
                    {
                        this.StylusButtonState &= ~STYLUS_BUTTON_UPPER_MASK;
                    }
                }
            }


            if (button_info.Change != StylusButtonChangeType.NoChange)
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
