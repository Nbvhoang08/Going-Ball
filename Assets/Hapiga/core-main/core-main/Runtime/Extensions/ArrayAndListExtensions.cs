using System;
using System.Collections.Generic;
using System.Text;

namespace Hapiga.Core.Runtime.Extensions
{
    public static class ArrayAndListExtensions
    {
        /// <summary>
        /// Shuffles the element order of the specified list.
        /// </summary>
        public static void Shuffle<T>(this IList<T> ts)
        {
            var count = ts.Count;
            var last = count - 1;
            for (var i = 0; i < last; ++i)
            {
                var r = UnityEngine.Random.Range(i, count);
                var tmp = ts[i];
                ts[i] = ts[r];
                ts[r] = tmp;
            }
        }

        public static T GetRandomItemInList<T>(this IList<T> itemList, int randomCountMax, Predicate<T> selectCondition)
        {
            if (itemList == null || itemList.Count <= 0)
            {
                return default(T);
            }

            int count = 0;
            int index;
            T selectItem = default(T);
            do
            {
                index = UnityEngine.Random.Range(0, itemList.Count);
                selectItem = itemList[index];
                count++;
            } while (!selectCondition(selectItem) && count < randomCountMax);

            return selectItem;
        }

        public static void AddRangeNoDuplicate<T>(this IList<T> itemList, IList<T> addedList)
        {
            if (itemList == null || addedList == null)
            {
                throw new ArgumentNullException();
            }

            for (int i = 0; i < addedList.Count; i++)
            {
                itemList.Add(addedList[i]);
            }
        }

        public static void AddRangeNoDuplicate2<T>(this IList<T> itemList, IList<T> addedList)
        {
            if (itemList == null || addedList == null)
            {
                throw new ArgumentNullException();
            }

            for (int i = 0; i < addedList.Count; i++)
            {
                if (!itemList.Contains(addedList[i]))
                {
                    itemList.Add(addedList[i]);
                }
            }
        }


        public static void AddValueToList(this List<int> itemList, int maximumListCount, bool isAddList, int value = 0)
        {
            if (!isAddList)
            {
                return;
            }

            if (itemList == null)
            {
                itemList = new List<int>(maximumListCount);
            }

            if (itemList.Count >= maximumListCount)
            {
                itemList.RemoveAt(0);
            }

            itemList.Add(value);
        }

        public static bool IsNullOrEmpty<T>(this IList<T> list)
        {
            return list == null || list.Count == 0;
        }

        /// <summary>
        /// Returns true if the array is null or empty.
        /// </summary>
        /// <typeparam name="T">Array Type.</typeparam>
        /// <param name="array">The array.</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this T[] array)
        {
            return ((array == null) || (array.Length == 0));
        }

        /// <summary>
        /// Returns true if the dictionary is null or empty.
        /// </summary>
        /// <typeparam name="TKey">Key Type.</typeparam>
        /// <typeparam name="TValue">Value Type.</typeparam>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<TKey, TValue>(this Dictionary<TKey, TValue> dict)
        {
            return ((dict == null) || (dict.Count == 0));
        }
        /// <summary>
        /// Returns a random element of the array. Does NOT check if the array is empty or null!
        /// </summary>
        /// <typeparam name="T">Array Type.</typeparam>
        /// <param name="array">The array.</param>
        /// <returns></returns>
        public static T GetRandomElement<T>(this T[] array)
        {
            return array[UnityEngine.Random.Range(0, array.Length)];
        }
        /// <summary>
        /// Returns a random element of the list. Does NOT check if the list is empty or null!
        /// </summary>
        /// <typeparam name="T">List Type.</typeparam>
        /// <param name="list">The list.</param>
        /// <returns>Radom element from the list</returns>
        public static T GetRandomElement<T>(this List<T> list)
        {
            return list[UnityEngine.Random.Range(0, list.Count)];
        }
        /// <summary>
        /// Shuffle the array.
        /// </summary>
        /// <typeparam name="T">Array Type.</typeparam>
        /// <param name="array">The array.</param>
        public static void ShuffleArray<T>(this T[] array)
        {
            T temp;
            for(int i = array.Length - 1; i > 0; i--)
            {
                // Get a random position lower than i
                int randomPosition = UnityEngine.Random.Range(0,i);
                // Swap values
                temp = array[i];
                array[i] = array[randomPosition];
                array[randomPosition] = temp;
            }
        }
        /// <summary>
        /// Shuffle the list.
        /// </summary>
        /// <typeparam name="T">List type.</typeparam>
        /// <param name="list">The list.</param>
        public static void ShuffleList<T>(this List<T> list)
        {
            T temp;
            for(int i = list.Count - 1; i > 0; i--)
            {
                // Get a random position lower than i
                int randomPosition = UnityEngine.Random.Range(0,i);
                // Swap values
                temp = list[i];
                list[i] = list[randomPosition];
                list[randomPosition] = temp;
            }
        }
        /// <summary>
        /// Joins all the elements of the array into a string separated by the given separator string.
        /// </summary>
        /// <typeparam name="T">Array Type.</typeparam>
        /// <param name="array">The array.</param>
        /// <param name="separator">String separator.</param>
        /// <returns></returns>
        public static string ToString<T>(this T[] array, string separator)
        {
            if(array.IsNullOrEmpty())
            {
                return string.Empty;
            }
            StringBuilder builder = new StringBuilder();
            for(int i = 0; i < array.Length - 1; i++)
            {
                builder.Append(array[i].ToString());
                builder.Append(separator);
            }
            builder.Append(array[array.Length - 1].ToString());
            return builder.ToString();
        }
        /// <summary>
        /// Joins all the elements of the list into a string separated by the given separator string.
        /// </summary>
        /// <typeparam name="T">List Type.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="separator">String separator.</param>
        /// <returns></returns>
        public static string ToString<T>(this List<T> list, string separator)
        {
            if(list.IsNullOrEmpty())
            {
                return string.Empty;
            }
            StringBuilder builder = new StringBuilder();
            for(int i = 0; i < list.Count - 1; i++)
            {
                builder.Append(list[i].ToString());
                builder.Append(separator);
            }
            builder.Append(list[list.Count - 1].ToString());
            return builder.ToString();
        }
		
		/// <summary>
        /// Joins all the elements of the array into a string separated by the given separator character.
        /// </summary>
        /// <typeparam name="T">Array Type.</typeparam>
        /// <param name="array">The array.</param>
        /// <param name="separator">String separator.</param>
        /// <returns></returns>
        public static string ToString<T>(this T[] array, char separator)
        {
            if(array.IsNullOrEmpty())
            {
                return string.Empty;
            }
            StringBuilder builder = new StringBuilder();
            for(int i = 0; i < array.Length - 1; i++)
            {
                builder.Append(array[i].ToString());
                builder.Append(separator);
            }
            builder.Append(array[array.Length - 1].ToString());
            return builder.ToString();
        }

        /// <summary>
        /// Joins all the elements of the list into a string separated by the given separator character.
        /// </summary>
        /// <typeparam name="T">List Type.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="separator">String separator.</param>
        /// <returns></returns>
        public static string ToString<T>(this List<T> list, char separator)
        {
            if(list.IsNullOrEmpty())
            {
                return string.Empty;
            }
            StringBuilder builder = new StringBuilder();
            for(int i = 0; i < list.Count - 1; i++)
            {
                builder.Append(list[i].ToString());
                builder.Append(separator);
            }
            builder.Append(list[list.Count - 1].ToString());
            return builder.ToString();
        }
    }
}