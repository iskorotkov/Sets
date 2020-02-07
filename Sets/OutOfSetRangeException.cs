using System;

namespace Sets
{
    public class OutOfSetRangeException : ApplicationException
    {
        public OutOfSetRangeException(int invalidValue, int max) : base($"Can't add value to set because it's not in supported range. Invalid value is {invalidValue}, and set range is [1, {max}].")
        { }
    }
}
