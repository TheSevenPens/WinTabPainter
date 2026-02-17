namespace SevenLib.Trigonometry
{

    public static class Angles
    {

        public static double DegreesToRadians(double degrees)
        {
            var radians = degrees * Math.PI / 180;
            return radians;
        }

        public static double RadiansToDegrees(double radians)
        {
            var degrees = radians * 180 / Math.PI;
            return degrees;
        }
    }
}
