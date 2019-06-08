using System;
using System.Linq;

namespace Helpers
{
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

        public static string EndWithNewLine(this string text)
        {
            if (!text.EndsWith(Environment.NewLine))
            {
                return text + Environment.NewLine;
            }

            return text;
        }
    }
}