using System;

namespace Sets
{
    public class BitSet : Set
    {
        private readonly int[] _arr;

        public BitSet(int max) : base(max)
        {
            max = (int)Math.Ceiling((double)max / sizeof(int));
            _arr = new int[max];
        }

        public override void Add(int i)
        {
            if (i < 1 || i > Max)
            {
                throw new OutOfSetRangeException(i, Max);
            }
            _arr[Elem(i)] |= Bitmask(i);
        }

        private static int Elem(int i) => (i - 1) / sizeof(int);

        private static int Bitmask(int i)
        {
            var bit = (i - 1) % sizeof(int);
            return 1 << bit;
        }

        public override void Remove(int i)
        {
            if (i < 1 || i > Max) return;
            var invertedMask = ~Bitmask(i);
            _arr[Elem(i)] &= invertedMask;
        }

        public override bool Contains(int i)
        {
            if (i < 1 || i > Max) return false;
            var bitmask = _arr[Elem(i)] & Bitmask(i);
            return bitmask != 0;
        }

        public static BitSet operator +(BitSet lhs, BitSet rhs)
        {
            var minLength = Math.Min(lhs.Max, rhs.Max);
            var length = Math.Max(lhs.Max, rhs.Max);
            var result = new BitSet(length);
            for (var i = 0; i < minLength; i++)
            {
                result._arr[i] = lhs._arr[i] | rhs._arr[i];
            }

            var biggest = lhs.Max > rhs.Max ? lhs : rhs;
            for (var i = minLength; i < length; i++)
            {
                result._arr[i] = biggest._arr[i];
            }

            return result;
        }

        public static BitSet operator *(BitSet lhs, BitSet rhs)
        {
            var length = Math.Min(lhs.Max, rhs.Max);
            var result = new BitSet(length);
            for (var i = 0; i < length; i++)
            {
                result._arr[i] = lhs._arr[i] & rhs._arr[i];
            }

            return result;
        }
    }
}
