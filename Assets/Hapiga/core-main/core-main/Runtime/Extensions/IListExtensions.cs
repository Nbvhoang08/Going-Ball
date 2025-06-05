using System;
using System.Collections.Generic;

namespace Hapiga.Core.Runtime.Extensions
{
    public static class IListExtensions
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

        public static bool IsNullOrEmpty<T>(this List<T> list)
        {
            return list == null || list.Count == 0;
        }
    }
}