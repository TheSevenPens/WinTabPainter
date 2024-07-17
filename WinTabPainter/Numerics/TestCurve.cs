using System;
using System.Runtime.CompilerServices;

namespace WinTabPainter.Numerics;

public class TestCurve
{

    double _curve_amt;

    private static OrderedRangeD curve_amt_range = new OrderedRangeD(-1.0, 1.0);
    public double CurveAmount
    {
        get => this._curve_amt;
        set => this._curve_amt = curve_amt_range.Clamp(value);
    }

    private double _inputMin;
    private double _inputMax;
    private double _outputMin;
    private double _outputMax;

    public double InputMin { get => _inputMin; set => _inputMin = value; }
    public double InputMax { get => _inputMax; set => _inputMax = value; }
    public double OutputMin { get => _outputMin; set => _outputMin = value; }
    public double OutputMax { get => _outputMax; set => _outputMax = value; }

    public TestCurve()
    {
        this.CurveAmount = 0.0;
        this.InputMin = 0.0;
        this.InputMax = 1.0;
        this.OutputMin = 0.0;
        this.OutputMax = 1.0;
    }

    public TestCurve(double amount)
    {
        this.CurveAmount = amount;
        this.InputMin = 0.0;
        this.InputMax = 1.0;
        this.OutputMin = 0.0;
        this.OutputMax = 1.0;
    }

    private double ClampRangeDouble(double value, double min, double max)
    {
        if (value < min) { value = min; }
        else if (value > max) { value = max; }
        else { /* dnothing */}
        return value;
    }
    public double ApplyCurve(double value)
    {
        var in_pressure = value;
        // handle everything outside the desired input range
        if (in_pressure < this._inputMin)
        {
            return this._outputMin;
        }
        if (in_pressure > this._inputMax)
        {
            return this._outputMax;
        }

        // normalize input pressure into range [0,1]
        double out_pressure = Numerics.Interpolation.InverseLerp(this._inputMin, this._inputMax, in_pressure);

        // apply curve amount
        if (this.CurveAmount > 0)
        {
            out_pressure = Math.Pow(value, 1.0 - this.CurveAmount);
        }
        else if (this.CurveAmount < 0)
        {
            out_pressure = Math.Pow(value, 1.0 / (1.0 + this.CurveAmount));
        }

        // scale out pressure into desired output range
        out_pressure = Numerics.Interpolation.Lerp(this._outputMin, this._outputMax, out_pressure);
        out_pressure = ClampRangeDouble(out_pressure, this._outputMin, this._outputMax);
        return out_pressure;
    }
}
