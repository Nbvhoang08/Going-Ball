using System.Collections;
using System.Numerics;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Hapiga.Core.Runtime.Utils
{
	public static class NumberConverter
	{
		private static StringBuilder unitBuilder = new StringBuilder();
		private static StringBuilder numberBuilder = new StringBuilder();
		private static int baseCount = 26;
		private static int offset = (int) 'A';

		public static string ConvertToString(int _number)
		{
			unitBuilder.Clear();
			int number = _number;
			int digitCount = number / baseCount;
			digitCount += 2;
			int digitInit = number % baseCount;
			char digitChar = (char) (offset + digitInit);
			for (int i = 0; i < digitCount; i++)
			{
				unitBuilder.Append(digitChar);
			}
			//int b = number % baseCount;
			//number = number / baseCount;
			//unitBuilder.Insert(0, (char)(offset + b));
			//if (number <= 0)
			//{
			//    unitBuilder.Insert(0, (char)(number + offset));
			//}
			//else
			//{
			//    while (number > 0)
			//    {
			//        b = number % baseCount;
			//        unitBuilder.Insert(0, (char)(offset + b));
			//        number = number / baseCount;
			//    }
			//}

			return unitBuilder.ToString();
		}

		public static string ToStringN0(this BigInteger bigInt, NumberLengthType lengthType)
		{
			string str = /*bigInt.ToString();*/bigInt + "";
			int maxLength = GetMaxLength(lengthType);
			int length = str.Length;
			if (length <= maxLength)
			{
				return FormatBigNumberWithComma(str);
			}
			else
			{
				return FormatBigNumberToWord(length, maxLength, bigInt);
			}
		}

		//public static string ToStringN0(this double inputNumber, NumberLengthType lengthType)
		//{
		//    string str = inputNumber.ToString("#.##");
		//    int maxLength = GetMaxLength(lengthType);
		//    int length = str.Length;
		//    if (length <= maxLength)
		//    {
		//        return FormatBigNumberWithComma(str);
		//    }
		//    else
		//    {
		//        return FormatBigNumberToWord(length, maxLength, inputNumber);
		//    }
		//}

		private static int GetMaxLength(NumberLengthType lengthType)
		{
			int maxLength = 3;
			switch (lengthType)
			{
				case NumberLengthType.Short:
					maxLength = 3;
					break;
				case NumberLengthType.Medium:
					maxLength = 6;
					break;
				case NumberLengthType.Long:
					maxLength = 9;
					break;
				case NumberLengthType.VeryLong:
					maxLength = 12;
					break;
			}

			return maxLength;
		}

		private static string FormatBigNumberToWord(int _length, int maxLength, BigInteger _number)
		{
			int a = _length / 3;
			if (_length % 3 == 0 && a > 1)
			{
				a -= 1;
			}

			numberBuilder.Clear();
			BigInteger number = _number / MathUtils.PowBigInt(10, a * 3 - 2);
			float numberFloat = (float) number / 100;
			numberBuilder.AppendFormat("{0:#,###,##0.##} ", numberFloat);
			numberBuilder.Append(GetUnit(a));
			//return numberBuilder.ToString();
			return numberBuilder + "";
		}

		private static string GetUnit(int a)
		{
			string result = string.Empty;
			if (a == 1)
			{
				result = "K";
			}
			else if (a == 2)
			{
				result = "M";
			}
			else if (a == 3)
			{
				result = "B";
			}
			else if (a == 4)
			{
				result = "T";
			}
			else
			{
				result = ConvertToString(a - 5);
			}

			return result;
		}

		private static string FormatBigNumberWithComma(string _numberStr)
		{
			int length = _numberStr.Length;
			if (length <= 3)
			{
				return _numberStr;
			}

			int offset = length % 3;
			if (offset == 0)
			{
				offset += 3;
			}

			_numberStr = _numberStr.Insert(offset, ",");
			length++;
			int i = _numberStr.LastIndexOf(',');
			while (i + 4 < length)
			{
				i = i + 4;
				_numberStr = _numberStr.Insert(i, ",");
				i = _numberStr.LastIndexOf(',');
				length++;
			}

			return _numberStr;
		}
	}

	public enum NumberLengthType : byte
	{
		Short = 1,
		Medium = 2,
		Long = 3,
		VeryLong = 4
	}
}