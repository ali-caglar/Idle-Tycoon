using System;
using System.Collections.Generic;
using BreakInfinity;

namespace Helpers
{
    public static class LetterNotation
    {
        private static readonly int CharA = Convert.ToInt32('a');

        private static readonly Dictionary<int, string> Units = new()
        {
            { 0, "" },
            { 1, "K" },
            { 2, "M" },
            { 3, "B" },
            { 4, "T" }
        };

        public static string FormatNumber(BigDouble value)
        {
            if (value < 1d)
            {
                return "0";
            }

            var n = (int)BigDouble.Log(value, 1000);
            var m = value / BigDouble.Pow(1000, n);
            var unit = "";

            if (n < Units.Count)
            {
                unit = Units[n];
            }
            else
            {
                var unitInt = n - Units.Count;
                var secondUnit = unitInt % 26;
                var firstUnit = unitInt / 26;
                unit = Convert.ToChar(firstUnit + CharA) + Convert.ToChar(secondUnit + CharA).ToString();
            }

            // Floor(m * 100) / 100) fixes rounding errors
            return $"{(BigDouble.Floor(m * 100) / 100).ToString("G0")}{unit}"; // "0.##"
        }
    }
}