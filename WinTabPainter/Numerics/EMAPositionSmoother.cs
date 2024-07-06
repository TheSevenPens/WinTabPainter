using System;

namespace WinTabPainter.Numerics;

public class EMAPositionSmoother
{
    private double _amount;
    public double SmoothingAmount
    {
        get => this._amount;
        set => this._amount = EMASmoother.ClampSmoothingAmount(value);
    }
    private Geometry.PointD? _old_sm_pos;

    public EMAPositionSmoother(double amount)
    {
        this.SmoothingAmount = amount;
        this._old_sm_pos = null;
    }

    public void SetOldSmoothedValue(Geometry.PointD p)
    {
        this._old_sm_pos = p;
    }

    public void Reset()
    {
        this._old_sm_pos = null;
    }

    public Geometry.PointD Smooth(Geometry.Point value)
    {
        return this.GetNextSmoothed(new Geometry.PointD(value.X, value.Y));
    }

    public Geometry.PointD GetNextSmoothed(Geometry.PointD p)
    {
        Geometry.PointD new_sm_pos;
        if (_old_sm_pos.HasValue)
        {
            new_sm_pos = Interpolation.Lerp(_old_sm_pos.Value, p, 1.0 - this.SmoothingAmount);
        }
        else
        {
            new_sm_pos = p;
        }

        _old_sm_pos = new_sm_pos;
        return new_sm_pos;
    }
}
