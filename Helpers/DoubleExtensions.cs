using System;

namespace Helpers
{
    public static class DoubleExtensions
    {
        public static long ToLong(this double value)
        {
            return Convert.ToInt64(value);
        }
    }
}