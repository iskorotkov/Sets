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
            if (i < 1 || i > _max)
            {
                throw new OutOfSetRangeException(i, _max);
            }
            _arr[i - 1] = true;
        }

        public override void Remove(int i)
        {
            _arr[i - 1] = false;
        }

        public override bool Contains(int i) => _arr[i - 1];

        public static SimpleSet operator +(SimpleSet s1, SimpleSet s2)
        {
            // var minLength = Math.Min(s1._max, s2._max);
            // var maxLength = Math.Max(s1._max, s2._max);
            // var s3 = new SimpleSet(maxLength);
            // for (var i = 0; i < minLength; i++)
            // {
            //     s3._arr[i] = s1._arr[i] || s2._arr[i];
            // }

            // var biggerSet = s1._max > s2._max ? s1 : s2;
            // for (var i = minLength; i < maxLength; i++)
            // {
            //     s3._arr[i] = biggerSet._arr[i];
            // }

            // return s3;
            return Union(s1, s2,
                applyUnion: (s1, s2, s3, i) => s3._arr[i] = s1._arr[i] || s2._arr[i],
                applyCopy: (biggerSet, s3, i) => s3._arr[i] = biggerSet._arr[i]);
        }

        public static SimpleSet operator *(SimpleSet s1, SimpleSet s2)
        {
            // var minLength = Math.Min(s1._max, s2._max);
            // var s3 = new SimpleSet(minLength);
            // for (var i = 0; i < minLength; i++)
            // {
            //     s3._arr[i] = s1._arr[i] && s2._arr[i];
            // }

            // return s3;
            return Intersect(s1, s2,
                applyIntersect: (s1, s2, s3, i) => s3._arr[i] = s1._arr[i] && s2._arr[i]);
        }
    }
}
