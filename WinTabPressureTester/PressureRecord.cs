namespace WinTabPressureTester
{
    public class PressureRecord
    {
        public readonly double PhysicalPressure;
        public readonly double LogicalPressure;
        public PressureRecord(double physical, double logical)
        {
            this.PhysicalPressure = physical;
            this.LogicalPressure = logical; 
        }
    }
}
