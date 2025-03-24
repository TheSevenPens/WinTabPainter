using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinTabUtils.Numerics
{
    public class MovingAverage
    {
        int window_size;
        private readonly Queue<double> samples;
        private double sum;

        public MovingAverage(int size)
        {
            this.window_size = size;
            this.samples = new Queue<double>(this.window_size);
            this.sum = 0.0;

        }
        public void AddSample(double value)
        {
            samples.Enqueue(value);
            sum += value;

            if (samples.Count > this.window_size)
            {
                sum -= samples.Dequeue();
            }
        }

        public double GetAverage()
        {
            if (samples.Count == 0)
                return 0.0;

            return sum / samples.Count;
        }

        public int SampleCount => samples.Count;

        public void Clear()
        {
            samples.Clear();
            sum = 0.0;
        }
    }


}
