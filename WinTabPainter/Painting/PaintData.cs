using SD = System.Drawing;

namespace WinTabPainter.Painting;

public struct PaintData
{
    // TIME
    public uint Time;

    // POSITION
    public Geometry.Point PenPos;
    public Geometry.Point PenPosSmoothed;

    // HOVER
    public int PenHover;

    //TILT
    public double TiltAltitude;
    public double TiltAzimuth;


    // PRESSURE
    public uint PressureRaw;
    public double PressureNormalized;
    public double PressureSmoothed;
    public double PressureCurved;
    public double PressureEffective;

    // BRUSH
    public int BrushWidthEffective;

    public PaintData(WintabDN.WintabPacket pkt, TabletInfo tablet, PaintSettings paintsettings)
    {

        // TIME
        this.Time = pkt.pkTime;

        // POSITION
        this.PenPos = new Geometry.Point(pkt.pkX, pkt.pkY);
        this.PenPosSmoothed = paintsettings.PositionSmoother.Smooth(this.PenPos).Round().ToPoint();

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
