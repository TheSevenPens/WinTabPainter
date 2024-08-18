namespace WinTabPainter
{
    public class QuantItem
    {
        public string Key { get; set; }
        public int Value { get; set; }

        public QuantItem(string key, int value)
        {
            this.Key = key;
            this.Value = value;
        }
    }
}
