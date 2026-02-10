using System;
using static WinTabPainter.FormWinTabPainterApp;

namespace WinTabPainter;


public static class HelperMethods
{
    public static double Quantize(double value, int quant_levels)
    {
        //double new_value = ((int)(value * quant_levels)) / (double)quant_levels;

        double new_value  = (System.Math.Floor(value*quant_levels) / quant_levels);
        //double new_value = (System.Math.Round(value * quant_levels) / quant_levels);
        //Console.WriteLine("{0,7}: in {1,-20}, em {2}, em2 {3}", i, in_p, em_p, em_p2);


        return new_value;
    }

    public static double DegreesToRadians(double degrees)
    {
        var radians = degrees * Math.PI / 180;
        return radians;
    }

    public static double RadiansToDegrees(double radians)
    {
        var degrees = radians * 180 / Math.PI;
        return  degrees;
    }

    public static bool UpdatesOld( int val, int? oldval ) 
    {
        return ((!oldval.HasValue) || (oldval.HasValue && (val != oldval.Value)));
    }

    public static bool UpdatesOld(RecStatusEnum val, RecStatusEnum? oldval)
    {
        return ((!oldval.HasValue) || (oldval.HasValue && (val != oldval.Value)));
    }

}
