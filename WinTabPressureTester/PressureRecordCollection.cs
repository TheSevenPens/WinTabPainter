using System.Text;

namespace WinTabPressureTester
{
    public class PressureRecordCollection
    {
        public List<PressureRecord> items;

        public PressureRecordCollection()
        {
            this.items = new List<PressureRecord>();
        }
        public int Count { get { return this.items.Count; } }

        public string GetText()
        {
            var sb = new StringBuilder();
            foreach (var record  in items)
            {
                string p = string.Format("{0:0.0}", record.PhysicalPressure );
                string l = string.Format("{0:0.0000}", record.LogicalPressure * 100.0);


                sb.Append("[ " + p + " , " + l + " ] , " + "\r\n");
            }
            return sb.ToString();
        }

        public void Add(double physical, double logical)
        {
            var r = new PressureRecord(physical, logical);
            this.items.Add(r);
        }

        public void Clear()
        {
            this.items.Clear();
        }

        public void ClearLast()
        {
            this.items.RemoveAt(this.items.Count - 1);
        }

        

    }
}
