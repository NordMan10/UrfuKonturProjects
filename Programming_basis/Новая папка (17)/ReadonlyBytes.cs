using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace hashes
{
   public class ReadonlyBytes : IEnumerable<byte>
   {
        private byte[] Bytes { get; }
        public int Length { get { return Bytes.Length; } }
        private int hash;

        public ReadonlyBytes(params byte[] values)
        {
            if (values is null) throw new ArgumentNullException();
            Bytes = values;
        }

        public override bool Equals(object obj)
        {
            if (obj is null || obj.GetType() != GetType()) return false;
            var readOnlyBytes = obj as ReadonlyBytes;
            if (Length != readOnlyBytes.Length) return false;
            if (GetHashCode() == readOnlyBytes.GetHashCode()) return true;
            return false;
        }

        public override int GetHashCode()
        {
            if (hash != 0) return hash;
            for (var i = 0; i < Length; i++)
            {
                unchecked
                {
                    hash *= 168431;
                    hash ^= Bytes[i];
                }
            }
            return hash;
        }

        public IEnumerator<byte> GetEnumerator()
        {
            foreach(var value in Bytes)
                yield return value;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public byte this[int index]
        {
            get
            {
                if (index < 0 && index >= Length) throw new IndexOutOfRangeException();
                return Bytes[index];
            }
        }

        public override string ToString()
        {
            return string.Format("[{0}]", string.Join(", ", Bytes));
        }
   }
}