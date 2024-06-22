using System.Runtime.CompilerServices;

namespace WinTabPainter
{

    public static class HelperMethods
    {
        public static double ClampRange(double value, double min, double max)
        {
            if (value < min) { value = min; }
            else if (value > max) { value = max; }
            else { /* dnothing */}
            return value;
        }


    }
}
