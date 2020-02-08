using System;

namespace Sets
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = new SimpleSet(100);
            s.Append(new[] { 1, 2, 3, 4, 10, 11, 100 });
            s.Remove(10);
            Console.WriteLine(s);

            var s2 = new BitSet(20);
            s2.Append("1 2 3 4 20 19 20 18");
            s2.Remove(18);
            Console.WriteLine(s2);

            var s3 = new MultiSet(10);
            s3.Append(new[] { 1, 1, 2, 2, 10, 9, 9 });
            s3.Remove(1);
            s3.Remove(9);
            Console.WriteLine(s3);

            s.Add(101);
        }
    }
}
