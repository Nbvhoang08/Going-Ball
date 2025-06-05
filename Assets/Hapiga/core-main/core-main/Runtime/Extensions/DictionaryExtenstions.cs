using System.Collections.Generic;

namespace Hapiga.Core.Runtime.Extensions
{
    public static class DictionaryExtenstions
    {
        /// <summary>
        /// Adds the given key and value to the dictionary, if the key is not already present in the dictionary. Returns true if the key-value was added, false otherwise.
        /// </summary>
        /// <typeparam name="TKey">Key Type.</typeparam>
        /// <typeparam name="TValue">Value Type.</typeparam>
        /// <param name="dict">Dictionary.</param>
        /// <param name="key">The key to be added.</param>
        /// <param name="value">The value to be added.</param>
        /// <returns></returns>
        public static bool AddIfKeyNotPresent<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            try
            {
                dict.Add(key, value);
            }
            catch(System.Exception)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// If the key is not present in the dictionary, it is added with the specified value. Otherwise, its value is changed to the one specified here.
        /// </summary>
        /// <typeparam name="TKey">Key type.</typeparam>
        /// <typeparam name="TValue">Value type.</typeparam>
        /// <param name="dict">Dictionary.</param>
        /// <param name="key">The key to be added or updated.</param>
        /// <param name="value">The value for the key.</param>
        public static void AddOrUpdate<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            try
            {
                dict.Add(key, value);
            }
            catch(System.Exception)
            {
                dict[key] = value;
            }
        }
        /// <summary>
        /// Tries to add the key-value to the dictionary. Returns true if successful, false otherwise.
        /// <para>This method is just a wrapper for the <c>AddIfKeyNotPresent</c> renamed to match the native TryGetValue().</para>
        /// </summary>
        /// <typeparam name="TKey">Key Type.</typeparam>
        /// <typeparam name="TValue">Value Type.</typeparam>
        /// <param name="dict">Dictionary.</param>
        /// <param name="key">The key to be added.</param>
        /// <param name="value">The value to be added.</param>
        /// <returns></returns>
        public static bool TryAddKey<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            return dict.AddIfKeyNotPresent(key, value);
        }
    }
}