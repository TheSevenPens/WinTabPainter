namespace WinTabPressureTester
{
    public record class ScaleRecord
    {
        public string Line { get; set; }
        public string ReadingAsString { get; set; }
        public double ReadingAsDouble { get; set; }

        public ScaleRecord()
        {
        }
    }
}
