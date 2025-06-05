using System;
using UnityEngine;

namespace Hapiga.Core.Runtime.Extensions
{
    public static class FloatExtensions
    {
        /// <summary>
        /// Returns a float rounded up to the set number of decimals.
        /// </summary>
        public static float Round(this float f, int decimals = 1) { return (float)Math.Round(f, decimals); }
        /// <summary>
        /// Returns the float rounded to the nearest integer.
        /// </summary>
        public static float Round(this float f) { return Mathf.Round(f); }
    }
}
