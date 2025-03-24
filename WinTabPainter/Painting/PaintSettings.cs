﻿using System;
using System.Windows.Forms;

namespace WinTabPainter.Painting;


public class PaintSettingsDynamics
{
    // dynamics
    public readonly WinTabUtils.Numerics.SimpleCurve PressureCurve;
    public readonly WinTabUtils.Numerics.EMASmoother PressureSmoother;
    public readonly WinTabUtils.Numerics.EMAPositionSmoother PositionSmoother;

    public PaintSettingsDynamics()
    {
        this.PressureCurve = new WinTabUtils.Numerics.SimpleCurve();
        this.PressureSmoother = new WinTabUtils.Numerics.EMASmoother(0);
        this.PositionSmoother = new WinTabUtils.Numerics.EMAPositionSmoother(0);
    }
}


public class PaintSettings
{
    // statics
    public static readonly WinTabUtils.Numerics.OrderedRange SYS_BRUSHSIZE_RANGE = new WinTabUtils.Numerics.OrderedRange(1, 100);
    public static readonly WinTabUtils.Numerics.OrderedRangeD SYS_SMOOTHING_RANGE = new WinTabUtils.Numerics.OrderedRangeD(0, 1);
    public static readonly WinTabUtils.Numerics.OrderedRangeD SYS_SMOOTHING_RANGE_LIMITED = new WinTabUtils.Numerics.OrderedRangeD(0.0, 0.995);

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
    public int PostionNoiseX;
    public int PositionNoiseY;

    // Smoothing 
    public double PositionSmoothingAmount
    {
        get => this.Dynamics.PositionSmoother.SmoothingAmount;
        set => this.Dynamics.PositionSmoother.SmoothingAmount = PaintSettings.SYS_SMOOTHING_RANGE.Clamp(value);
    }

    public double PressureSmoothingAmount
    {
        get => this.Dynamics.PressureSmoother.SmoothingAmount;
        set => this.Dynamics.PressureSmoother.SmoothingAmount = PaintSettings.SYS_SMOOTHING_RANGE.Clamp(value);
    }

    public double PressureCurveAmount
    {
        get => this.Dynamics.PressureCurve.CurveAmount;
        set => this.Dynamics.PressureCurve.CurveAmount  = value;
    }



    // Anti-Aliasing
    public bool AntiAliasing = true;

    public PaintSettings()
    {
        this.Dynamics = new PaintSettingsDynamics();
    }

    public void CopyTo(PaintSettings to)
    {
        to.PressureCurveAmount = this.PressureCurveAmount;
        to.PressureSmoothingAmount = this.PressureSmoothingAmount;
        to.PositionSmoothingAmount = this.PositionSmoothingAmount;
        to.PressureQuantizeLevels = this.PressureQuantizeLevels;
        to.AntiAliasing = this.AntiAliasing;
        to.PostionNoiseX = this.PostionNoiseX;
        to.PositionNoiseY = this.PositionNoiseY;
    }
}
