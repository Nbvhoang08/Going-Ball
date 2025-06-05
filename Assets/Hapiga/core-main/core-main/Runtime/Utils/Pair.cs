using System;
using UnityEngine;
namespace Hapiga.Core.Runtime.Utils
{
	[Serializable]
	public class Pair<T1, T2>
	{
		[SerializeField]
		private T1 _key;
		[SerializeField]
		private T2 _value;

		public T1 Key { get { return _key; } set { _key = value; } }
		public T2 Value { get { return _value; } set { _value = value; } }

		public Pair(T1 _key, T2 _value)
		{
			Key = _key;
			Value = _value;
		}
		public Pair()
		{
			Key = default(T1);
			Value = default(T2);
		}
	}

	[Serializable]
	public class PairIntLong : Pair<int, long>
	{
		public PairIntLong(int _key, long _value) : base(_key, _value)
		{
			//		Key = _key;
			//		Value = _value;
		}

		public PairIntLong() : base()
		{ }
	} 
	
}