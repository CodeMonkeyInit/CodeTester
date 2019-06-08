using System;

namespace Helpers
{
    public static class IntExtensions
    {
        public static int ToIntOrDefault(this string numberString) => 
            int.TryParse(numberString, out int parsedNumber) ? parsedNumber : default;

        public static double ToDouble(this int integer) => Convert.ToDouble(integer);
    }
}