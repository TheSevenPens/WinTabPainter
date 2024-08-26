using System;

namespace WinTabPainter.Numerics;

public class SimpleCurve
{

    double amt;

    private static OrderedRangeD range = new OrderedRangeD(-1.0,1.0);
    public double CurveAmount { 
        get => this.amt; 
        set => this.amt = range.Clamp(value); }

    public SimpleCurve()
    {
        this.CurveAmount = 0.0;
    }

    public SimpleCurve(double amount)
    {
        this.CurveAmount = range.Clamp(amount);
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
