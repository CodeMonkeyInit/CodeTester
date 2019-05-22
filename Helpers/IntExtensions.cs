using System;
using System.Linq;

namespace Helpers
{
    public static class IntExtensions
    {
        public static int ToIntOrDefault(this string numberString) => 
            int.TryParse(numberString, out int parsedNumber) ? parsedNumber : default;
    }
    
    public static class StringExtensions
    {
        public static string FirstCharToUpper(this string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }
    }
}