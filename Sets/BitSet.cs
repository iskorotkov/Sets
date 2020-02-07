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
            // var minLength = Math.Min(s1._max, s2._max);
            // var maxLength = Math.Max(s1._max, s2._max);
            // var s3 = new BitSet(maxLength);
            // for (var i = 0; i < minLength; i++)
            // {
            //     s3._arr[i] = s1._arr[i] | s2._arr[i];
            // }

            // var biggerSet = s1._max > s2._max ? s1 : s2;
            // for (var i = minLength; i < maxLength; i++)
            // {
            //     s3._arr[i] = biggerSet._arr[i];
            // }

            // return s3;

            return Union(s1, s2,
                applyUnion: (s1, s2, s3, i) => s3._arr[i] = s1._arr[i] | s2._arr[i],
                applyCopy: (biggerSet, s3, i) => s3._arr[i] = biggerSet._arr[i]);
        }

        public static BitSet operator *(BitSet s1, BitSet s2)
        {
            // var minLength = Math.Min(s1._max, s2._max);
            // var s3 = new BitSet(minLength);
            // for (var i = 0; i < minLength; i++)
            // {
            //     s3._arr[i] = s1._arr[i] & s2._arr[i];
            // }

            // return s3;

            return Intersect(s1, s2,
                applyIntersect: (s1, s2, s3, i) => s3._arr[i] = s1._arr[i] & s2._arr[i]);
        }
    }
}
