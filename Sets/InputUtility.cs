using System.Collections.Generic;
using System.Linq;

namespace Sets
{
    public static class InputUtility
    {
        public static IEnumerable<string> RemoveInsignificantWhitespaces(this string input)
        {
            return input.Trim()
                .Split(' ', '\t', '\n', '\r')
                .Where(s => s.Any());
        }

        public static IEnumerable<string> RemoveInsignificantWhitespaces(this string input, int substrings)
        {
            return input.Trim()
                .Split(new[] {' ', '\t', '\n', '\r'}, substrings)
                .Where(s => s.Any());
        }
    }
}
