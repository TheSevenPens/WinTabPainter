namespace WinTabPainter.Painting;

public class PaintSettings
{
    // brush settings
    private int brushWidth = 25;
    public static readonly Numerics.OrderedRange SYS_BRUSHSIZE_RANGE = new Numerics.OrderedRange(1, 100);

    // pressure settings
    public Numerics.SimpleCurve pressure_curve = new Numerics.SimpleCurve();
    public int PressureQuantizeLevels = -1;

    // noise
    public int PosXNoise;
    public int PosYNoise;


    // smoothing settings
    public readonly Numerics.EMAPositionSmoother PositionSmoother;
    public readonly Numerics.EMASmoother PressureSmoother;

    public static readonly Numerics.OrderedRangeD SYS_SMOOTHING_RANGE = new Numerics.OrderedRangeD(0, 1);
    public static readonly Numerics.OrderedRangeD SYS_SMOOTHING_RANGE_LIMITED = new Numerics.OrderedRangeD(0.0, 0.995);


    public int BrushWidth { 
        get => brushWidth; 
        set => brushWidth = PaintSettings.SYS_BRUSHSIZE_RANGE.Clamp(value); }

    public bool AntiAliasing = true;
    public PaintSettings()
    {
        this.PositionSmoother = new Numerics.EMAPositionSmoother(0);
        this.PressureSmoother = new Numerics.EMASmoother(0);
    }



}
