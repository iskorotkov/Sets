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
            _arr[i - 1] = false;
        }

        public override bool Contains(int i) => _arr[i - 1];

        public static SimpleSet operator +(SimpleSet s1, SimpleSet s2)
        {
            return Union(s1, s2,
                // ReSharper disable once ArgumentsStyleAnonymousFunction
                applyUnion: (inSet1, inSet2, resultSet, i) => resultSet._arr[i] = inSet1._arr[i] || inSet2._arr[i],
                // ReSharper disable once ArgumentsStyleAnonymousFunction
                applyCopy: (biggerSet, resultSet, i) => resultSet._arr[i] = biggerSet._arr[i]);
        }

        public static SimpleSet operator *(SimpleSet s1, SimpleSet s2)
        {
            return Intersect(s1, s2,
                // ReSharper disable once ArgumentsStyleAnonymousFunction
                applyIntersect: (inSet1, inSet2, resultSet, i) => resultSet._arr[i] = inSet1._arr[i] && inSet2._arr[i]);
        }
    }
}
