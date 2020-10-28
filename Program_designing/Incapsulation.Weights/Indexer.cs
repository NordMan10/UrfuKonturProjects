using System;
using System.Collections.Generic;

namespace Incapsulation.Weights
{
	public class Indexer
    {
        public Indexer(double[] array, int start, int length)
        {
            if (IsRangeValid(start, length, array.Length))
            {
                Start = start;
                Length = length;
                arraySegment = new ArraySegment<double>(array, Start, Length);
            }
            else throw new ArgumentException();
        }

        private readonly ArraySegment<double> arraySegment;
        public int Start { get; }

        public int Length { get; }

        public double this[int index]
        {
            get 
            {
                CheckIndexValid(index);
                var temp = (IList<double>)arraySegment;
                return temp[index];
            }
            set 
            {
                CheckIndexValid(index);
                var temp = (IList<double>)arraySegment;
                temp[index] = value;
            }
        }

        private bool IsRangeValid(int start, int length, int arrayLength)
        {
            return start + length - 1 < arrayLength;
        }

        private void CheckIndexValid(int index)
        {
            if (index < 0 || index >= Length)
                throw new IndexOutOfRangeException();
        }
    }
}
