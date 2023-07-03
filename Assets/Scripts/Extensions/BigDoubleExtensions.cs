using BreakInfinity;
using Helpers;

namespace Extensions
{
    public static class BigDoubleExtensions
    {
        public static string ToLetterNotation(this BigDouble value)
        {
            return LetterNotation.FormatNumber(value);
        }
    }
}