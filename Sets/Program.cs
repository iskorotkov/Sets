using System;

namespace Sets
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = new SimpleSet(100);
            s.Append(new[] { 1, 2, 3, 4, 10, 11, 100 });
            Console.WriteLine(s);

            var s2 = new BitSet(20);
            s2.Append("1 2 3 4 20 19 20 18");
            Console.WriteLine(s2);
        }
    }
}
