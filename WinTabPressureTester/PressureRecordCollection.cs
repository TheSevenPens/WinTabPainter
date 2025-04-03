using System.Text;

namespace WinTabPressureTester
{
    public class PressureRecordCollection
    {
        List<PressureRecord> records;

        public PressureRecordCollection()
        {
            this.records = new List<PressureRecord>();
        }
        public int Count { get { return this.records.Count; } }

        public string GetText()
        {
            var sb = new StringBuilder();
            foreach (var record  in records)
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
            this.records.Add(r);
        }

        public void Clear()
        {
            this.records.Clear();
        }
    }
}
