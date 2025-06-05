namespace Hapiga.Core.Runtime.Extensions
{
    public static class BoolExtensions
    {
        /// <summary>
        /// Checks whether the given boolean item is true.
        /// </summary>
        /// <param name="bool">The boolean value.</param>
        /// <returns></returns>
        public static bool IsTrue(this bool @bool) { return @bool; }
        /// <summary>
        /// Checks whether the given boolean item is false.
        /// </summary>
        /// <param name="bool">The boolean value.</param>
        /// <returns></returns>
        public static bool IsFalse(this bool @bool) { return !@bool; }
        /// <summary>
        /// Toggles the given boolean item and returns the toggled value.
        /// </summary>
        /// <param name="bool">The boolean value.</param>
        /// <returns></returns>
        public static bool Toggle(this bool @bool) { return !@bool; }
        /// <summary>
        /// Converts the given boolean value to integer.
        /// </summary>
        /// <param name="bool">The boolean value.</param>
        /// <returns></returns>
        public static int ToInt(this bool @bool) { return @bool ? 1 : 0; }
        /// <summary>
        /// Returns the lower string representation of boolean.
        /// </summary>
        /// <param name="bool">The boolean value.</param>
        /// <returns></returns>
        public static string ToLowerString(this bool @bool) { return @bool.ToString().ToLower(); }
        /// <summary>
        /// Returns the trueString or falseString based on the given boolean value.
        /// </summary>
        /// <param name="bool">The boolean value.</param>
        /// <param name="trueString">String returned if the bool is true.</param>
        /// <param name="falseString">String returned if the bool is false.</param>
        /// <returns></returns>
        public static string ToString(this bool @bool, string trueString, string falseString) { return @bool.ToType<string>(trueString, falseString); }
        /// <summary>
        /// Returns the trueValue or the falseValue based on the given boolean value.
        /// </summary>
        /// <typeparam name="T">Output type.</typeparam>
        /// <param name="bool">The boolean value.</param>
        /// <param name="trueValue">Value returned if the bool is true.</param>
        /// <param name="falseValue">Value returned if the bool is false.</param>
        /// <returns></returns>
        public static T ToType<T>(this bool @bool, T trueValue, T falseValue) { return @bool ? trueValue : falseValue; }
    }
}
