using System;
using System.Linq;
using System.Text;

namespace Sets
{
    public abstract class Set
    {
        public static Set operator +(Set lhs, Set rhs)
        {
            var length = Math.Max(lhs.Max, rhs.Max);
            var result = (Set) Activator.CreateInstance(lhs.GetType(), length);
            for (var i = 1; i <= length; i++)
            {
                if (lhs.Contains(i) || rhs.Contains(i))
                {
                    result.Add(i);
                }
            }
            return result;
        }

        public static Set operator *(Set lhs, Set rhs)
        {
            var length = Math.Min(lhs.Max, rhs.Max);
            var result = (Set) Activator.CreateInstance(lhs.GetType(), length);
            for (var i = 1; i <= length; i++)
            {
                if (lhs.Contains(i) && rhs.Contains(i))
                {
                    result.Add(i);
                }
            }

            return result;
        }

        public abstract void Add(int i);
        public abstract void Remove(int i);
        public abstract bool Contains(int i);

        protected readonly int Max;

        protected Set(int max)
        {
            Max = max;
        }

        public void Append(string s)
        {
            var nums = s.Split(' ', '\t', '\r', '\n')
                .Where(str => str.Any())
                .Select(int.Parse)
                .ToArray();
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
            var elemAdded = false;
            for (var i = 1; i <= Max; ++i)
            {
                if (!Contains(i)) continue;
                if (elemAdded)
                {
                    builder.Append(", ");
                }

                builder.Append(i);
                elemAdded = true;
            }

            builder.Append("]");
            return builder.ToString();
        }
    }
}
