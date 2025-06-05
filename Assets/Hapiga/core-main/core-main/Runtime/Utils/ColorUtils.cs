using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

namespace Hapiga.Core.Runtime.Utils
{
	public static class ColorUtils
	{
		public static Color Yellow = Color.yellow;
		public static Color Orange = new Color(1f, 0.752941f, 0, 1);
		public static Color Red = Color.red;

		public static Color Faded = new Color(1, 1, 1, 0);
		public static Color Shown = new Color(1, 1, 1, 1);
		public static Color Shown2 = new Color(1, 1, 1, 0.5f);
		public static Color UIBg = new Color(0, 0, 0, 204 / 255f);

		public static Color DisableButton = new Color(0.5f, 0.5f, 0.5f, 1);
		public static Color ClickedButton = new Color(0.7f, 0.7f, 0.7f, 1);

		public static Color TextOrange = new Color(1f, 0.682353f, 0, 1);
		public static Color TextLightBlue = new Color(0.4f, 0.6784314f, 1f, 1);
		public static Color TextGreen = new Color(0.427451f, 0.6980392f, 0.1568628f, 1);
		public static Color TextBlue = new Color(65f / 255, 199 / 255f, 1f, 1f);


		public static Color TextUpgrade = new Color(191 / 255f, 114 / 255f, 0f, 1f);
		public static Color TextReset = new Color(70f / 255, 150 / 255f, 41 / 255f, 1f);

		public static Color ColorGreenLevelUp = new Color(131f / 255f, 221 / 255f, 37 / 255f, 1.0f);
		public static Color ColorPurpleVip = new Color(203 / 255f, 92 / 255f, 254 / 255f, 1.0f);
		public static Color ColorOrange = new Color(253 / 255f, 169 / 255f, 25 / 255f, 1.0f);

		public static Color ColorRainStart = new Color(45 / 255f, 193 / 255f, 179 / 255f, 78 / 255f);
		public static Color ColorRainEnd = new Color(37 / 255f, 173 / 255f, 221 / 255f, 78 / 255f);

		public static Color TextPink = new Color(180 / 255f, 30 / 255f, 127 / 255f, 1f);

		public static Color NormalLeague = new Color(32 / 255f, 161 / 255f, 56 / 255f, 1f);
		public static Color BronzeLeague = new Color(32 / 255f, 126 / 255f, 191 / 255f, 1f);
		public static Color SilverLeague = new Color(153 / 255f, 75 / 255f, 194 / 255f, 1f);
		public static Color GoldLeague = new Color(223 / 255f, 130 / 255f, 64 / 255f, 1f);

		public static Color CloudLock = new Color(228 / 255f, 241 / 255f, 248 / 255f, 1f);
		public static Color CloudHalloween = new Color(247 / 255f, 235 / 255f, 255 / 255f, 1f);

		public static Color CloudMorning = new Color(82 / 255f, 210 / 255f, 211 / 255f, 1f);
		public static Color CloudNight = new Color(81 / 255f, 113 / 255f, 186 / 255f, 1f);
		public static Color CloudButFloor = new Color(228 / 255f, 241 / 255f, 248 / 255f, 1f);

		public static Color BannerBottomWin = new Color(157 / 255f, 17 / 255f, 36 / 255f, 1f);
		public static Color BannerTopWin = new Color(217 / 255f, 28 / 255f, 64 / 255f, 1f);
		public static Color BarSpinWin = new Color(247 / 255f, 218 / 255f, 139 / 255f, 1f);
		public static Color XGoldWin = new Color(203 / 255f, 176 / 255f, 103 / 255f, 1f);


		public static Color BannerBottomLose = new Color(60 / 255f, 60 / 255f, 60 / 255f, 1f);
		public static Color BannerTopLose = new Color(60 / 255f, 60 / 255f, 60 / 255f, 1f);
		public static Color BarSpinLose = new Color(144 / 255f, 144 / 255f, 144 / 255f, 1f);
		public static Color XGoldLose = new Color(117 / 255f, 117 / 255f, 117 / 255f, 1f);


		public static Color DailyClose = new Color(117 / 255f, 117 / 255f, 117 / 255f, 1f);
		public static Color Daily = new Color(67 / 255f, 100 / 255f, 177 / 255f, 1f);
		public static Color Slot = new Color(108 / 255f, 93 / 255f, 64 / 255f, 1f);

		public static Color YellowLucky = new Color(1f, 204 / 255f, 0 / 255f, 1f);
		public static Color OrangeLucky = new Color(1f, 140 / 255f, 2 / 255f, 1f);
		public static Color RedLucky = Color.red;

		public static Color WinXGoldItem = new Color(251f / 255, 244f / 255f, 166f / 255f, 1);
		public static Color LoseXGoldItem = new Color(163f / 255, 163f / 255f, 160f / 255f, 1);
		public static Color BgLoseXGoldItem = new Color(86f / 255, 92 / 255f, 107 / 255f, 1);
	}
}