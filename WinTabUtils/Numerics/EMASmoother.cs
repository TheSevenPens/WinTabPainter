namespace WinTabUtils.Numerics;

public class EMASmoother
{

    private double _amount;

    public double SmoothingAmount 
    { 
        get => this._amount; 
        set => this._amount = EMASmoother.ClampSmoothingAmount(value); 
    }

    private double? _old_sm_val;


    public EMASmoother(double alpha)
    {
        this.SmoothingAmount = alpha;
        this._old_sm_val = null;
    }

    public void SetOldSmoothedValue(double value)
    {
        _old_sm_val = value;
    }
    public void Reset()
    {
        this._old_sm_val = null;
    }

    public double GetNextSmoothed(double value)
    {
        double new_sm_val;
        if (_old_sm_val.HasValue)
        {

            new_sm_val = Interpolation.Lerp(_old_sm_val.Value, value, 1.0-this.SmoothingAmount);
        }
        else
        {
            new_sm_val = value;
        }

        _old_sm_val = new_sm_val;
        return new_sm_val;
    }
    public static double ClampSmoothingAmount(double value)
    {
        double min = 0.0;
        double max = 1.0;
        if (value < min) { value = min; }
        else if (value > max) { value = max; }
        else { /* dnothing */}
        return value;
    }
}
