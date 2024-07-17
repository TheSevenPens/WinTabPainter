using System;

namespace WinTabPainter.Numerics;

public class SimpleCurve
{

    double _curve_amt;

    private static OrderedRangeD _curve_amt_range = new OrderedRangeD(-1.0,1.0);
    public double CurveAmount { 
        get => this._curve_amt; 
        set => this._curve_amt = _curve_amt_range.Clamp(value); }

    public SimpleCurve()
    {
        this.CurveAmount = 0.0;
    }

    public SimpleCurve(double amount)
    {
        this.CurveAmount = amount;
    }

    public double ApplyCurve(double value)
    {

        double new_value;

        if (this.CurveAmount > 0)
        {
            new_value = Math.Pow(value, 1.0 - this.CurveAmount);
        }
        else if (this.CurveAmount < 0)
        {
            new_value = Math.Pow(value, 1.0 / (1.0 + this.CurveAmount));
        }
        else
        {
            new_value = value;
        }

        return new_value;
    }
}
