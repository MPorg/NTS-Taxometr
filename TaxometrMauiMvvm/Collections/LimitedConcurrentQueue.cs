using System.Collections.Concurrent;
using System.Diagnostics;

namespace TaxometrMauiMvvm.Collections
{
    public class LimitedConcurrentQueue<T>(int limit) : ConcurrentQueue<T>
    {
        private int _limit = limit;
        public int Limit => _limit;

        public new void Enqueue(T item)
        {
            if (Count < Limit)
                base.Enqueue(item);
            else
                throw new ArgumentOutOfRangeException("Count", "Count out of Limit");
        }

        public bool TryEnqueue(T item)
        {
            if (Count <= Limit)
            {
                base.Enqueue(item);
                return true;
            }
            return false;
        }
    }
}
