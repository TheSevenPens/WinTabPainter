namespace WinTabPainter;


public static class HelperMethods
{
    public static double Quantize(double value, int quant_levels)
    {
        double new_value = ((int)(value * quant_levels)) / (double)quant_levels;
        return new_value;
    }


}
