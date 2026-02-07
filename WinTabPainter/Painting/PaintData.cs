namespace WinTabPainter.Painting;

public struct PaintData
{
    // STATUS
    public readonly PaintDataStatus Status;


    // TIME
    public readonly uint Time;

    // POSITION
    public readonly SevenUtils.Geometry.Point PosScreen;
    public readonly SevenUtils.Geometry.Point PosScreenEffective;
    public readonly SevenUtils.Geometry.Point PosCanvas;
    public readonly SevenUtils.Geometry.Point PosCanvasEffective;

    // HOVER
    public readonly int PenHover;

    //TILT
    public readonly double TiltAltitude;
    public readonly double TiltAzimuth;
    public readonly double TiltX;
    public readonly double TiltY;


    // PRESSURE
    public readonly uint PressureRaw;
    public readonly double PressureNormalized;
    public readonly double PressureSmoothed;
    public readonly double PressureCurved;
    public readonly double PressureEffective;



    // BRUSH
    public readonly double BrushWidthEffective;

    public PaintData()
    {
        this.Status = PaintDataStatus.INVALID;
    }

    static System.Random random  = new System.Random();
    public PaintData(WinTabDN.Structs.WintabPacket pkt, WinTabDN.Utils.TabletInfo tablet, PaintSettings paintsettings, System.Func<SevenUtils.Geometry.Point,SevenUtils.Geometry.Point> to_canv)
    {
        // STATUS
        this.Status = PaintDataStatus.VALID;

        // TIME
        this.Time = pkt.pkTime;

        // POSITION
        this.PosScreen = new SevenUtils.Geometry.Point(pkt.pkX, pkt.pkY);
        this.PosScreenEffective = paintsettings.Dynamics.PositionSmoother.Smooth(this.PosScreen).Round().ToPoint();

        this.PosCanvas = to_canv(this.PosScreen);
        this.PosCanvasEffective = to_canv(this.PosScreenEffective);

        if (paintsettings.PostionNoiseX > 0)
        {
            this.PosCanvasEffective = new SevenUtils.Geometry.Point(this.PosCanvasEffective.X + random.Next( paintsettings.PostionNoiseX), this.PosCanvasEffective.Y);
        }

        if (paintsettings.PositionNoiseY > 0)
        {
            this.PosCanvasEffective = new SevenUtils.Geometry.Point(this.PosCanvasEffective.X, this.PosCanvasEffective.Y + random.Next(paintsettings.PositionNoiseY));
        }


        // HOVER
        this.PenHover = pkt.pkZ;

        // TILT
        this.TiltAltitude = pkt.pkOrientation.orAltitude / 10.0;
        this.TiltAzimuth = pkt.pkOrientation.orAzimuth / 10.0;

        (this.TiltX, this.TiltY) = SevenUtils.Trigonometry.Angles.AzimuthAndAltudeToTiltDeg(this.TiltAzimuth, this.TiltAltitude);

        // PRESSURE
        this.PressureRaw = pkt.pkNormalPressure;
        this.PressureNormalized = this.PressureRaw / (double) tablet.MaxPressure;

        if (paintsettings.PressureQuantizeLevels >= 2)
        {
            this.PressureNormalized = HelperMethods.Quantize(this.PressureNormalized, paintsettings.PressureQuantizeLevels);
        }
        
        this.PressureSmoothed = paintsettings.Dynamics.PressureSmoother.GetNextSmoothed(this.PressureNormalized);
        this.PressureCurved = paintsettings.Dynamics.PressureCurve.ApplyCurve(this.PressureSmoothed);
        this.PressureEffective = this.PressureCurved;

        // BRUSH SIZE
        // Calculate the brush width taking into account the pen pressure
        if (this.PressureRaw > 0)
        {
            double effective_width = this.PressureEffective * paintsettings.BrushWidth;
            effective_width = PaintSettings.SYS_BRUSHSIZE_RANGE.Clamp((int)effective_width);
            this.BrushWidthEffective = effective_width; 
        }
        else
        {
            this.BrushWidthEffective = 0.0;
        }
    }
}
