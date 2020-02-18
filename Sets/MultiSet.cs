using System;

namespace Sets
{
    public class MultiSet : Set
    {
        private readonly int[] _arr;

        public MultiSet(int max) : base(max)
        {
            _arr = new int[max];
        }

        public override void Add(int i)
        {
            if (i < 1 || i > Max)
            {
                throw new OutOfSetRangeException(i, Max);
            }
            _arr[i - 1]++;
        }

        public override bool Contains(int i) => i >= 1 && i <= Max && _arr[i - 1] > 0;

        public override void Remove(int i)
        {
            if (i < 1 || i > Max) return;
            _arr[i - 1] = Math.Max(0, _arr[i - 1] - 1);
        }

        public static MultiSet operator +(MultiSet lhs, MultiSet rhs)
        {
            var minLength = Math.Min(lhs.Max, rhs.Max);
            var length = Math.Max(lhs.Max, rhs.Max);
            var result = new MultiSet(length);
            for (var i = 0; i < minLength; i++)
            {
                result._arr[i] = lhs._arr[i] + rhs._arr[i];
            }

            var biggest = lhs.Max > rhs.Max ? lhs : rhs;
            for (var i = minLength; i < length; i++)
            {
                result._arr[i] = biggest._arr[i];
            }

            return result;
        }

        public static MultiSet operator *(MultiSet lhs, MultiSet rhs)
        {
            var length = Math.Min(lhs.Max, rhs.Max);
            var result = new MultiSet(length);
            for (var i = 0; i < length; i++)
            {
                result._arr[i] = Math.Min(lhs._arr[i], rhs._arr[i]);
            }

            return result;
        }
    }
}
