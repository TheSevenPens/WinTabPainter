using System;
using System.Net.Sockets;

namespace SevenLib.WinTab;


public class WinTabSession : System.IDisposable
{
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
        this.StylusButtonState = new SevenLib.Stylus.StylusButtonState(0); // Initialize to indicate no buttons are pressed
    }

    public void Open(SevenLib.WinTab.Tablet.TabletContextType context_type)
    {
        // Convert the context type to something wintab understands
        var wt_context_type = context_type_to_index(context_type);

        // Create WinTab context with options to receive messages
        var options = SevenLib.WinTab.Enums.ECTXOptionValues.CXO_MESSAGES;
        this.Context = SevenLib.WinTab.WinTabContextFactory.GetDefaultContext(wt_context_type, options);
        if (this.Context == null)
        {
            throw new System.ApplicationException("Failed to get digitizing context");
        }
        this.Context.Options |= (uint)SevenLib.WinTab.Enums.ECTXOptionValues.CXO_SYSTEM;
        // Move origin from lower-left to upper left so it matches screen origin
        // Invert Y values to match the screen coordinates (where Y increases downwards)
        // This is commonly needed for Windows apps so do this out of convenience for consumers of this library
        this.Context.OutExtY = -this.Context.OutExtY;
        var status = this.Context.Open();

        // Create data
        this.Data = new WinTabData(this.Context);

        // Collect information about the tablet
        this.TabletInfo.Initialize();

        // Attach event handler for receiving wintab packets
        this.Data.SetWTPacketEventHandler(WinTabPacketHandler);
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
                // Some callers want the raw wintab packet
                this.OnWinTabPacketReceived(wintab_pkt);
            }

            if (this.OnStandardPointerEvent != null)
            {
                // other callers will want the standardized pointer event 
                // which simplified things for the caller
                var pointerdata = new SevenLib.Stylus.PointerData();
                pointerdata.Time = DateTime.Now;

                var screenPos = new Geometry.Point(wintab_pkt.pkX, wintab_pkt.pkY);
                pointerdata.DisplayPoint = new Geometry.PointD(screenPos.X, screenPos.Y);

                pointerdata.Height = wintab_pkt.pkZ;
                float normalized_pressure = (float)wintab_pkt.pkNormalPressure / this.TabletInfo.MaxPressure;
                pointerdata.PressureNormalized = normalized_pressure;
                pointerdata.TiltAADeg = new Trigonometry.TiltAA(wintab_pkt.pkOrientation.orAzimuth / 10, wintab_pkt.pkOrientation.orAltitude / 10);
                pointerdata.TiltXYDeg = pointerdata.TiltAADeg.ToXY_Deg();
                pointerdata.Twist = wintab_pkt.pkOrientation.orTwist;
                pointerdata.ButtonState = this.StylusButtonState;

                this.OnStandardPointerEvent(pointerdata);
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
