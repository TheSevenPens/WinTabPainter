namespace SevenLib.Numerics
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

    public class IndexedQueue<T>
    {
        private List<T> items;

        public IndexedQueue(int bufsize)
        {
            items = new List<T>(bufsize);
        }

        // Add item to the end of the queue
        public void Enqueue(T item)
        {
            items.Add(item);
        }

        // Remove and return item from the front of the queue
        public T Dequeue()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Queue is empty");

            T item = items[0];
            items.RemoveAt(0);
            return item;
        }

        // Return item at front without removing it
        public T Peek()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Queue is empty");

            return items[0];
        }

        // Get item at specific index
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                    throw new IndexOutOfRangeException("Index is out of range");
                return items[index];
            }
        }

        // Get number of items in queue
        public int Count
        {
            get { return items.Count; }
        }

        // Check if queue is empty
        public bool IsEmpty
        {
            get { return items.Count == 0; }
        }

        // Clear all items from queue
        public void Clear()
        {
            items.Clear();
        }
    }

}
