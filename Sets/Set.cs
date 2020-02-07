using System;
using System.Linq;
using System.Text;

namespace Sets
{
    public abstract class Set
    {
        protected static T Union<T>(T s1, T s2, Action<T, T, T, int> applyUnion, Action<T, T, int> applyCopy) where T : Set
        {
            var minLength = Math.Min(s1._max, s2._max);
            var maxLength = Math.Max(s1._max, s2._max);
            var s3 = (T)Activator.CreateInstance(typeof(T), maxLength);
            for (var i = 0; i < minLength; i++)
            {
                applyUnion(s1, s2, s3, i);
            }

            var biggerSet = s1._max > s2._max ? s1 : s2;
            for (var i = minLength; i < maxLength; i++)
            {
                applyCopy(biggerSet, s3, i);
            }

            return s3;
        }

        protected static T Intersect<T>(T s1, T s2, Action<T, T, T, int> applyIntersect) where T : Set
        {
            var minLength = Math.Min(s1._max, s2._max);
            var s3 = (T)Activator.CreateInstance(typeof(T), minLength);
            for (var i = 0; i < minLength; i++)
            {
                applyIntersect(s1, s2, s3, i);
            }

            return s3;
        }

        public abstract void Add(int i);
        public abstract void Remove(int i);
        public abstract bool Contains(int i);

        protected readonly int _max;

        public Set(int max)
        {
            _max = max;
        }

        public void Append(string s)
        {
            var nums = s.Split(' ').Select(int.Parse).ToArray();
            Append(nums);
        }

        public void Append(int[] nums)
        {
            Array.ForEach(nums, Add);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("Set: [");
            for (var i = 1; i <= _max; ++i)
            {
                if (!Contains(i)) continue;
                builder.Append(i);
                if (i != _max)
                {
                    builder.Append(", ");
                }
            }
            builder.Append("]");
            return builder.ToString();
        }
    }
}
