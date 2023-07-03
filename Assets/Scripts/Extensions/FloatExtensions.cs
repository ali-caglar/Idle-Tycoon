using Helpers;
using UnityEngine;

namespace Extensions
{
    public static class FloatExtensions
    {
        public static bool IsExceeded(this float thisFloat, float floatToCompare)
        {
            if (thisFloat >= floatToCompare) return true;
            return Mathf.Abs(floatToCompare - thisFloat) < Constants.FloatingPointTolerance;
        }
    }
}