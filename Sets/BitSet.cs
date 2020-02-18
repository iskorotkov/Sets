using System;

namespace Sets
{
    public class BitSet : Set
    {
        private const int BitsPerCell = 32;
        
        private readonly int[] _arr;

        public BitSet(int max) : base(max)
        {
            var length = (int)Math.Ceiling((double)max / BitsPerCell);
            _arr = new int[length];
        }

        public override void Add(int i)
        {
            if (i < 1 || i > Max)
            {
                throw new OutOfSetRangeException(i, Max);
            }
            _arr[Cell(i)] |= Bitmask(i);
        }

        private static int Cell(int i) => (i - 1) / BitsPerCell;

        private static int Bitmask(int i)
        {
            var bit = (i - 1) % BitsPerCell;
            return 1 << bit;
        }

        public override void Remove(int i)
        {
            if (i < 1 || i > Max) return;
            var invertedMask = ~Bitmask(i);
            _arr[Cell(i)] &= invertedMask;
        }

        public override bool Contains(int i)
        {
            if (i < 1 || i > Max) return false;
            var bitmask = _arr[Cell(i)] & Bitmask(i);
            return bitmask != 0;
        }

        public static BitSet operator +(BitSet lhs, BitSet rhs)
        {
            var minLength = Math.Min(lhs.Max, rhs.Max);
            var length = Math.Max(lhs.Max, rhs.Max);
            var result = new BitSet(length);
            for (var i = 0; i <= Cell(minLength); i++)
            {
                result._arr[i] = lhs._arr[i] | rhs._arr[i];
            }

            var biggest = lhs.Max > rhs.Max ? lhs : rhs;
            for (var i = Cell(minLength) + 1; i <= Cell(length); i++)
            {
                result._arr[i] = biggest._arr[i];
            }

            return result;
        }

        public static BitSet operator *(BitSet lhs, BitSet rhs)
        {
            var length = Math.Min(lhs.Max, rhs.Max);
            var result = new BitSet(length);
            for (var i = 0; i <= Cell(length); i++)
            {
                result._arr[i] = lhs._arr[i] & rhs._arr[i];
            }

            return result;
        }
    }
}
