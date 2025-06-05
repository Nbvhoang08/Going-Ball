using System;
using System.Text;
using UnityEngine;

namespace Hapiga.Core.Runtime.Utils
{
	public class TimeUtils
	{
		private static readonly DateTime Jan1st2015 = new DateTime(2015, 1, 1, 0, 0, 0, DateTimeKind.Local);

		public static readonly string dateFormat = "dd/MMM/yyyy";
		public static readonly string timeFormat = "HH:mm:ss";

		public static readonly int secondsPerDay = 86400;
		public static readonly int secondsPerMinute = 60;
		public static readonly int secondsPerHour = 60 * 60;
		public static readonly int secondPerWeek = secondsPerDay * 7;
		private static StringBuilder strBuilder = new StringBuilder();


		static int deltaSecondsWithServer { get; set; }

		public static void Setup(int currentTimeServer)
		{
			int currentTimeClient = (int) ((DateTime.Now - Jan1st2015).TotalSeconds);

			deltaSecondsWithServer = currentTimeServer - currentTimeClient;

#if UNITY_EDITOR
			Debug.Log("Delta seconds with server: " + TimeUtils.deltaSecondsWithServer);
#endif
		}

		#region Convert

		public static ulong ConvertToTimeInSecond(DateTime time)
		{
			ulong timeInSecond = (ulong) ((time - Jan1st2015).TotalSeconds);

			return timeInSecond;
		}

		public static DateTime ConvertToDateTime(int timeSecond)
		{
			DateTime time = Jan1st2015.Add(new TimeSpan(timeSecond * TimeSpan.TicksPerSecond));

			return time;
		}

		public static DateTime ConvertToDateTimeLong(long timeSecond)
		{
			DateTime time = Jan1st2015.Add(new TimeSpan(timeSecond * TimeSpan.TicksPerSecond));

			return time;
		}

		public static long ConvertToTimeInSecondLong(DateTime time)
		{
			long timeInSecond = (long) ((time - Jan1st2015).TotalSeconds);

			return timeInSecond;
		}

		#endregion

		#region Get Time

		public static int GetCurrentTimeInSecond()
		{
			int now = (int) ((DateTime.Now - Jan1st2015).TotalSeconds);
			now += deltaSecondsWithServer;

			return now;
		}

		public static long GetCurrentTimeInMilisecond()
		{
			long now = (long) ((DateTime.Now - Jan1st2015).TotalMilliseconds);
			now += deltaSecondsWithServer * 1000;

			return now;
		}

		public static DateTime GetCurrentDateTime()
		{
			DateTime now = DateTime.Now;
			now = now.AddSeconds(deltaSecondsWithServer);

			return now;
		}

		public static ulong GetTodayMidnightTimeInSecond()
		{
			DateTime now = DateTime.Now.AddSeconds(deltaSecondsWithServer);
			DateTime midNightTime = (new DateTime(now.Year, now.Month, now.Day, 0, 0, 0));

			ulong midNightTimeInSecond = ConvertToTimeInSecond(midNightTime);

			return midNightTimeInSecond;
		}

		public static ulong GetMidnightTimeOfDate(DateTime date)
		{
			DateTime midNightTime = (new DateTime(date.Year, date.Month, date.Day, 0, 0, 0));

			ulong midNightTimeInSecond = ConvertToTimeInSecond(midNightTime);

			return midNightTimeInSecond;
		}

		#endregion

		public static string GetDateTimeFormat()
		{
			return TimeUtils.dateFormat + " " + TimeUtils.timeFormat;
		}

		public static DateTime NextDayOfWeek(DateTime from, DayOfWeek dayOfWeek)
		{
			int start = (int) from.DayOfWeek;
			int target = (int) dayOfWeek;

			if (target <= start)
				target += 7;

			return from.AddDays(target - start);
		}

		public static string GetDayOfWeekName(int day)
		{
			string result = string.Empty;

			switch (day)
			{
				case 0:
					result = "Sunday";
					break;

				case 1:
					result = "Monday";
					break;

				case 2:
					result = "Tuesday";
					break;

				case 3:
					result = "Wednesday";
					break;

				case 4:
					result = "Thursday";
					break;

				case 5:
					result = "Friday";
					break;

				case 6:
					result = "Saturday";
					break;
			}

			return result;
		}

		public static string FormatTime(long timeInSeconds)
		{
			strBuilder.Clear();
			int meaningfulIndex = 0;
			if (timeInSeconds > 0)
			{
				TimeSpan timeSpan = TimeSpan.FromSeconds(timeInSeconds);

				if (timeSpan.Days > 0)
				{
					strBuilder.AppendFormat("{0:0}d:", timeSpan.Days);
					meaningfulIndex++;
				}

				if (timeSpan.Hours > 0)
				{
					meaningfulIndex++;
					strBuilder.AppendFormat("{0:0}h", timeSpan.Hours);
					if (meaningfulIndex < 2)
					{
						strBuilder.AppendFormat(":", timeSpan.Hours);
					}
				}

				if (meaningfulIndex < 2)
				{
					if (timeSpan.Minutes > 0)
					{
						meaningfulIndex++;
						strBuilder.AppendFormat("{0:0}m", timeSpan.Minutes);
						if (meaningfulIndex < 2)
						{
							strBuilder.AppendFormat(" ", timeSpan.Hours);
						}
					}
				}

				if (meaningfulIndex < 2)
				{
					if (timeSpan.Seconds > 0)
					{
						meaningfulIndex++;
						strBuilder.AppendFormat("{0}s", timeSpan.Seconds);
					}
				}

				//strBuilder.AppendFormat("{0}s", timeSpan.Seconds);
			}
			else
			{
				strBuilder.Append("0s");
			}

			return strBuilder.ToString();
		}

		public static DateTime GetThisWeekDay(DateTime currentDate, DayOfWeek dow)
		{
			int currentDay = (int) currentDate.DayOfWeek, gotoDay = (int) dow;
			currentDay = currentDate.DayOfWeek == DayOfWeek.Sunday ? 7 : (int) currentDate.DayOfWeek;
			return currentDate.AddDays(0).AddDays(gotoDay - currentDay);
		}

		public static DateTime GetNextWeekDay(DateTime currentDate, DayOfWeek dow)
		{
			int currentDay = (int) currentDate.DayOfWeek, gotoDay = (int) dow;
			currentDay = currentDate.DayOfWeek == DayOfWeek.Sunday ? 7 : (int) currentDate.DayOfWeek;
			return currentDate.AddDays(7).AddDays(gotoDay - currentDay);
		}

		public static DateTime GetPreviousWeekDay(DateTime currentDate, DayOfWeek dow)
		{
			int currentDay = (int) currentDate.DayOfWeek, gotoDay = (int) dow;
			currentDay = currentDate.DayOfWeek == DayOfWeek.Sunday ? 7 : (int) currentDate.DayOfWeek;
			return currentDate.AddDays(-7).AddDays(gotoDay - currentDay);
		}

		public static DateTime StartOfWeek(DateTime dt, DayOfWeek startOfWeek)
		{
			int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
			return dt.AddDays(-1 * diff).Date;
		}
	}
}