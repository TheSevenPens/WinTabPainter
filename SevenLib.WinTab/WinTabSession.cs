using System;

namespace SevenLib.WinTab;


public class WinTabSession : System.IDisposable
{
    public SevenLib.Stylus.PointerData PointerData = new SevenLib.Stylus.PointerData();
    public Structs.WintabPacket WinTabPacket;

    public WinTabContext Context = null;
    public WinTabData Data = null;
    public SevenLib.WinTab.Tablet.TabletInfo TabletInfo;
    public SevenLib.WinTab.Tablet.TabletContextType ContextType;
    
    // callbacks for consumers
    public System.Action<Structs.WintabPacket, SevenLib.WinTab.Stylus.StylusButtonChange> OnButtonStateChanged = null;
    public System.Action<Structs.WintabPacket> OnWinTabPacketReceived = null;
    public System.Action<SevenLib.Stylus.PointerData> OnStandardPointerEvent =null;

    public Action _onPointerStatsUpdated;


    public SevenLib.Stylus.StylusButtonState StylusButtonState;

    public WinTabSession()
    {
        this.TabletInfo = new SevenLib.WinTab.Tablet.TabletInfo();
        this.PointerData = new SevenLib.Stylus.PointerData();
        this.StylusButtonState = new SevenLib.Stylus.StylusButtonState(0); // Initialize to indicate no buttons are pressed
        this.OnWinTabPacketReceived = HandleRawPacket;
    }

    public void Open(SevenLib.WinTab.Tablet.TabletContextType context_type)
    {
        // convert the context type to something wintab understands
        var wt_context_type = context_type_to_index(context_type);

        // CREATE CONTEXT
         var options = SevenLib.WinTab.Enums.ECTXOptionValues.CXO_MESSAGES;
         this.Context = SevenLib.WinTab.WinTabContextFactory.GetDefaultContext(wt_context_type, options);

        if (this.Context == null)
        {
            throw new System.ApplicationException("Failed to get digitizing context");
        }

        this.Context.Options |= (uint)SevenLib.WinTab.Enums.ECTXOptionValues.CXO_SYSTEM;

        // Move origin from lower-left to upper left so it matches screen origin
        this.Context.OutExtY = -this.Context.OutExtY;
        var status = this.Context.Open();

        // CREATE DATA

        this.Data = new WinTabData(this.Context);


        this.TabletInfo.Initialize();

        // HANDLER

        if (this.OnWinTabPacketReceived != null)
        {
            this.Data.SetWTPacketEventHandler(WinTabPacketHandler);
        }
    }

    private static Enums.EWTICategoryIndex context_type_to_index(SevenLib.WinTab.Tablet.TabletContextType context_type)
    {
        return context_type switch
        {
            SevenLib.WinTab.Tablet.TabletContextType.System => SevenLib.WinTab.Enums.EWTICategoryIndex.WTI_DEFSYSCTX,
            SevenLib.WinTab.Tablet.TabletContextType.Digitizer => SevenLib.WinTab.Enums.EWTICategoryIndex.WTI_DEFCONTEXT,
            _ => throw new System.ArgumentOutOfRangeException()
        };
    }

    private void WinTabPacketHandler(System.Object sender, WinForms.MessageReceivedEventArgs args)
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
            var button_info = new SevenLib.WinTab.Stylus.StylusButtonChange(wintab_pkt.pkButtons);

            if (button_info.Change != SevenLib.WinTab.Stylus.StylusButtonChangeType.NoChange)
            {
                // there's been some change to the buttons
                this.StylusButtonState = SevenLib.WinTab.Stylus.StylusUtils.Update(this.StylusButtonState, button_info);
            }


            if (button_info.Change != SevenLib.WinTab.Stylus.StylusButtonChangeType.NoChange)
            {
                this.OnButtonStateChanged?.Invoke(wintab_pkt, button_info);
            }

            if (this.OnWinTabPacketReceived != null) 
            {
                this.OnWinTabPacketReceived(wintab_pkt);
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

    private void HandleRawPacket(Structs.WintabPacket packet)
    {

        this.WinTabPacket = packet;
        this.PointerData = new SevenLib.Stylus.PointerData();
        this.PointerData.Time = DateTime.Now;

        var screenPos = new Geometry.Point(packet.pkX, packet.pkY);
        this.PointerData.DisplayPoint = new Geometry.PointD(screenPos.X, screenPos.Y);

        this.PointerData.Height = packet.pkZ;
        float normalized_pressure = (float)packet.pkNormalPressure / this.TabletInfo.MaxPressure;
        this.PointerData.PressureNormalized = normalized_pressure;
        this.PointerData.TiltAADeg = new Trigonometry.TiltAA(packet.pkOrientation.orAzimuth / 10, packet.pkOrientation.orAltitude / 10);
        this.PointerData.TiltXYDeg = this.PointerData.TiltAADeg.ToXY_Deg();
        this.PointerData.Twist = packet.pkOrientation.orTwist;
        this.PointerData.ButtonState = this.StylusButtonState;

        if (this.OnStandardPointerEvent != null)
        {
            this.OnStandardPointerEvent(this.PointerData);
        }
    }

}
