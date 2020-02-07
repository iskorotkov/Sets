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
            _arr[i - 1]++;
        }

        public override bool Contains(int i) => _arr[i - 1] > 0;

        public override void Remove(int i)
        {
            _arr[i - 1] = Math.Max(0, _arr[i - 1] - 1);
        }

        public static MultiSet operator +(MultiSet s1, MultiSet s2)
        {
            return Union(s1, s2,
                applyUnion: (s1, s2, s3, i) => s3._arr[i] = s1._arr[i] + s2._arr[i],
                applyCopy: (biggerSet, s3, i) => s3._arr[i] = biggerSet._arr[i]);
        }

        public static MultiSet operator *(MultiSet s1, MultiSet s2)
        {
            return Intersect(s1, s2,
                applyIntersect: (s1, s2, s3, i) => s3._arr[i] = Math.Min(s1._arr[i], s2._arr[2]));
        }
    }
}
