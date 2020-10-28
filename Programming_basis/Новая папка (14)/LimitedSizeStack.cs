using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApplication
{
    public class LimitedSizeStack<T>
    {
        LinkedList<T> linkedList = new LinkedList<T>();
        private int limit;
        public int Limit
        {
            get
            {
                return limit;
            }
            set
            {
                if (value < 1) throw new ArgumentException();
                limit = value;
            }
        }
        
        public LimitedSizeStack(int limit)
        {
            Limit = limit;
        }

        public void Push(T item)
        {
            linkedList.AddFirst(item);
            if (linkedList.Count > Limit)
            {
                linkedList.RemoveLast();
            }
        }

        public T Pop()
        {
            var result = linkedList.First();
            linkedList.RemoveFirst();
            return result;
        }

        public int Count
        {
            get
            {
                return linkedList.Count;
            }
        }
    }
}