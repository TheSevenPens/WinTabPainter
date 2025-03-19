namespace WinTabPainter.Painting;


public class PaintSettingsDynamics
{
    // dynamics
    public readonly Numerics.SimpleCurve PressureCurve;
    public readonly Numerics.EMASmoother PressureSmoother;
    public readonly Numerics.EMAPositionSmoother PositionSmoother;

    public PaintSettingsDynamics()
    {
        this.PressureCurve = new Numerics.SimpleCurve();
        this.PressureSmoother = new Numerics.EMASmoother(0);
        this.PositionSmoother = new Numerics.EMAPositionSmoother(0);
    }
}


public class PaintSettings
{
    // statics
    public static readonly Numerics.OrderedRange SYS_BRUSHSIZE_RANGE = new Numerics.OrderedRange(1, 100);
    public static readonly Numerics.OrderedRangeD SYS_SMOOTHING_RANGE = new Numerics.OrderedRangeD(0, 1);
    public static readonly Numerics.OrderedRangeD SYS_SMOOTHING_RANGE_LIMITED = new Numerics.OrderedRangeD(0.0, 0.995);

    // dynamics
    public PaintSettingsDynamics Dynamics;

    // Brush Width
    private int brushWidth = 25;
    public int BrushWidth
    {
        get => brushWidth;
        set => brushWidth = PaintSettings.SYS_BRUSHSIZE_RANGE.Clamp(value);
    }

    // pressure settings
    public int PressureQuantizeLevels = -1;

    // Position Noise
    public int PosXNoise;
    public int PosYNoise;

    // Smoothing 


    // Anti-Aliasing
    public bool AntiAliasing = true;

    public PaintSettings()
    {
        this.Dynamics = new PaintSettingsDynamics();
    }
}
