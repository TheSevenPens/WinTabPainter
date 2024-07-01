using SD = System.Drawing;

namespace WinTabPainter.Painting;

public enum PaintDataStatus
{ 
    INVALID,
    VALID
}


public struct PaintData
{
    // STATUS
    public readonly PaintDataStatus Status;


    // TIME
    public readonly uint Time;

    // POSITION
    public readonly Geometry.Point PosScreen;
    public readonly Geometry.Point PosScreenSmoothed;
    public readonly Geometry.Point PosCanvas;
    public readonly Geometry.Point PosCanvasSmoothed;

    // HOVER
    public readonly int PenHover;

    //TILT
    public readonly double TiltAltitude;
    public readonly double TiltAzimuth;


    // PRESSURE
    public readonly uint PressureRaw;
    public readonly double PressureNormalized;
    public readonly double PressureSmoothed;
    public readonly double PressureCurved;
    public readonly double PressureEffective;

    // BRUSH
    public readonly int BrushWidthEffective;

    public PaintData()
    {
        this.Status = PaintDataStatus.INVALID;
    }


    public PaintData(WintabDN.WintabPacket pkt, TabletInfo tablet, PaintSettings paintsettings, System.Func<Geometry.Point,Geometry.Point> to_canv)
    {
        // STATUS
        this.Status = PaintDataStatus.VALID;

        // TIME
        this.Time = pkt.pkTime;

        // POSITION
        this.PosScreen = new Geometry.Point(pkt.pkX, pkt.pkY);
        this.PosScreenSmoothed = paintsettings.PositionSmoother.Smooth(this.PosScreen).Round().ToPoint();
        this.PosCanvas = to_canv(this.PosScreen);
        this.PosCanvasSmoothed = to_canv(this.PosScreenSmoothed);

        // HOVER
        this.PenHover = pkt.pkZ;

        // TILT
        this.TiltAltitude = pkt.pkOrientation.orAltitude / 10.0;
        this.TiltAzimuth = pkt.pkOrientation.orAzimuth / 10.0;

        // PRESSURE
        this.PressureRaw = pkt.pkNormalPressure;
        this.PressureNormalized = this.PressureRaw / (double) tablet.MaxPressure;

        this.PressureSmoothed = paintsettings.PressureSmoother.GetNextSmoothed(this.PressureNormalized);
        this.PressureCurved = paintsettings.pressure_curve.ApplyCurve(this.PressureSmoothed);
        this.PressureEffective = this.PressureCurved;

        // BRUSH SIZE
        // Calculate the brush width taking into account the pen pressure
        if (this.PressureRaw > 0)
        {
            double effective_width = this.PressureEffective * paintsettings.BrushWidth;
            effective_width = PaintSettings.BRUSHSIZE_RANGE.Clamp((int)effective_width);
            this.BrushWidthEffective = (int) effective_width;
        }
        else
        {
            this.BrushWidthEffective = 0;
        }
    }
}
