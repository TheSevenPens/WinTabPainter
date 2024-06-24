using System.Runtime.CompilerServices;

namespace WinTabPainter
{

    public static class HelperMethods
    {
        public static int ClampRangeInt(int value, int min, int max)
        {
            if (value < min) { value = min; }
            else if (value > max) { value = max; }
            else { /* dnothing */}
            return value;
        }
        public static double ClampRangeDouble(double value, double min, double max)
        {
            if (value < min) { value = min; }
            else if (value > max) { value = max; }
            else { /* dnothing */}
            return value;
        }


    }
}
