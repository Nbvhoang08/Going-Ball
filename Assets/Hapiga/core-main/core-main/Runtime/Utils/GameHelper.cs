using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hapiga.Core.Runtime.Utils
{
	public class GameHelper
	{
		public static List<string> MixStringList(List<string> strList)
		{
			int index = Random.Range(0, strList.Count);
			for (int i = 0; i < strList.Count; i++)
			{
				var temp = strList[i];
				strList[i] = strList[index];
				strList[index] = temp;
				index = Random.Range(0, strList.Count);
			}

			return strList;
		}

		public static List<string> MixTwoListString(List<string> prefix, List<string> suffixes)
		{
			List<string> listOutput = new List<string>();
			int count = prefix.Count;
			prefix = MixStringList(prefix);
			suffixes = MixStringList(suffixes);
			for (int i = 0; i < count; i++)
			{
				listOutput.Add(prefix[i] + " " + suffixes[i]);
			}

			return listOutput;
		}

		public static float Remap(float _value, float _from1, float _to1, float _from2, float _to2)
		{
			return _from2 + (_value - _from1) * (_to2 - _from2) / (_to1 - _from1);
		}

		public static string TrimEnd(string source, string value)
		{
			if (!source.EndsWith(value))
				return source;

			return source.Remove(source.LastIndexOf(value));
		}

		#region Json conver

		public static List<T> MixElementList<T>(List<T> param)
		{
			int index = Random.Range(0, param.Count);
			for (int i = 0; i < param.Count; i++)
			{
				var temp = param[i];
				param[i] = param[index];
				param[index] = temp;
				index = Random.Range(0, param.Count);
			}

			return param;
		}

		public static List<T> FromJson<T>(string json)
		{
			if (json == string.Empty)
				return new List<T>();
			Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
			return wrapper.Items;
		}

		public static string ToJson<T>(List<T> array)
		{
			Wrapper<T> wrapper = new Wrapper<T>();
			wrapper.Items = array;
			return JsonUtility.ToJson(wrapper);
		}

		public static string ToJson<T>(List<T> array, bool prettyPrint)
		{
			Wrapper<T> wrapper = new Wrapper<T>();
			wrapper.Items = array;
			return JsonUtility.ToJson(wrapper, prettyPrint);
		}

		[System.Serializable]
		private class Wrapper<T>
		{
			public List<T> Items;
		}

		#endregion
	}

//listItemTrans = contentItemReward.Cast<Transform>().Select(t => t.gameObject).ToList();
}