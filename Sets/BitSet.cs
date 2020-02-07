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
            var invertedMask = ~Bitmask(i);
            _arr[Elem(i)] &= invertedMask;
        }

        public override bool Contains(int i)
        {
            var bitmask = _arr[Elem(i)] & Bitmask(i);
            return bitmask != 0;
        }

        public static BitSet operator +(BitSet s1, BitSet s2)
        {
            return Union(s1, s2,
                // ReSharper disable once ArgumentsStyleAnonymousFunction
                applyUnion: (inSet1, inSet2, resultSet, i) => resultSet._arr[i] = inSet1._arr[i] | inSet2._arr[i],
                // ReSharper disable once ArgumentsStyleAnonymousFunction
                applyCopy: (biggerSet, resultSet, i) => resultSet._arr[i] = biggerSet._arr[i]);
        }

        public static BitSet operator *(BitSet s1, BitSet s2)
        {
            return Intersect(s1, s2,
                // ReSharper disable once ArgumentsStyleAnonymousFunction
                applyIntersect: (inSet1, inSet2, resultSet, i) => resultSet._arr[i] = inSet1._arr[i] & inSet2._arr[i]);
        }
    }
}
