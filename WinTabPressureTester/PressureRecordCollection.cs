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
        public int Count => this.items.Count;

        public string GetText()
        {
            var sb = new StringBuilder();
            int i = 0;
            foreach (var record  in items)
            {
                string str_physical = string.Format("{0:0.0}", record.PhysicalPressure );
                string str_logical = string.Format("{0:0.0000}", record.LogicalPressure * 100.0);

                string comma = i==(items.Count-1) ? "" : ",";

                sb.Append($"[ {str_physical} , {str_logical} ] {comma}\r\n");
                i++;
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
