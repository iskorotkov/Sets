using System;

namespace Sets
{
    public class SimpleSet : Set
    {
        private readonly bool[] _arr;

        public SimpleSet(int max) : base(max)
        {
            _arr = new bool[max];
        }

        public override void Add(int i)
        {
            if (i < 1 || i > Max)
            {
                throw new OutOfSetRangeException(i, Max);
            }
            _arr[i - 1] = true;
        }

        public override void Remove(int i)
        {
            if (i < 1 || i > Max) return;
            _arr[i - 1] = false;
        }

        public override bool Contains(int i) => i >= 1 && i <= Max && _arr[i - 1];

        public static SimpleSet operator +(SimpleSet lhs, SimpleSet rhs)
        {
            var minLength = Math.Min(lhs.Max, rhs.Max);
            var length = Math.Max(lhs.Max, rhs.Max);
            var result = new SimpleSet(length);
            for (var i = 0; i < minLength; i++)
            {
                result._arr[i] = lhs._arr[i] || rhs._arr[i];
            }

            var biggest = lhs.Max > rhs.Max ? lhs : rhs;
            for (var i = minLength; i < length; i++)
            {
                result._arr[i] = biggest._arr[i];
            }

            return result;
        }

        public static SimpleSet operator *(SimpleSet lhs, SimpleSet rhs)
        {
            var length = Math.Min(lhs.Max, rhs.Max);
            var result = new SimpleSet(length);
            for (var i = 0; i < length; i++)
            {
                result._arr[i] = lhs._arr[i] && rhs._arr[i];
            }

            return result;
        }
    }
}
