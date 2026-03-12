namespace WinTabPressureTester
{
    public class PressureRecord
    {
        public double PhysicalPressure { get; }
        public double LogicalPressure { get; }

        public PressureRecord(double physical, double logical)
        {
            this.PhysicalPressure = physical;
            this.LogicalPressure = logical; 
        }
    }
}
