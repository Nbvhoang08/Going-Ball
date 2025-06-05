using UnityEngine;

namespace Hapiga.Core.Runtime.Utils
{
    public static class Vector3Utils
    {
        public static Vector3 Zero = Vector3.zero;
        public static Vector3 One = Vector3.one;
        public static Vector3 One1 = new Vector3(-1, 1, 1);
        public static Vector3 OneP1 = new Vector3(1.1f, 1.1f, 1);
        public static Vector3 One2 = new Vector3(1, -1, 1);
        public static Vector3 One3 = new Vector3(-1, -1, 1);
        public static Vector3 OneX = new Vector3(1, 0, 0);
        public static Vector3 OneY = new Vector3(0, 1, 0);
        public static Vector3 OneP5Y = new Vector3(0, 1.5f, 0);
        public static Vector3 ZeroYP5 = new Vector3(0, 0.5f, 0);
        public static Vector3 OneYP8 = new Vector3(1, 1.8f, 1);
        public static Vector3 ZeroYP2 = new Vector3(0, 0.2f, 0);
        public static Vector3 ZeroYP3 = new Vector3(0, 0.3f, 0);
        public static Vector3 ZeroYP4 = new Vector3(0, 0.4f, 0);
        public static Vector3 OneP2 = new Vector3(1.2f, 1.2f, 1);
        public static Vector3 OneP25 = new Vector3(1.25f, 1.25f, 1);
        public static Vector3 OneP3 = new Vector3(1.3f, 1.3f, 1);
        public static Vector3 OneP5 = new Vector3(1.5f, 1.5f, 1);
        public static Vector3 OneP6 = new Vector3(1.6f, 1.6f, 1);
        public static Vector3 OneP8 = new Vector3(1.8f, 1.8f, 1);
        public static Vector3 ZeroP1 = new Vector3(0.1f, 0.1f, 1);
        public static Vector3 ZeroP2 = new Vector3(0.2f, 0.2f, 1);
        public static Vector3 ZeroP3 = new Vector3(0.3f, 0.3f, 1);
        public static Vector3 ZeroP35 = new Vector3(0.35f, 0.35f, 1);
        public static Vector3 ZeroP4 = new Vector3(0.4f, 0.4f, 1);
        public static Vector3 ZeroP5 = new Vector3(0.5f, 0.5f, 1);
        public static Vector3 ZeroP6 = new Vector3(0.6f, 0.6f, 1);
        public static Vector3 ZeroP7 = new Vector3(0.7f, 0.7f, 1);
        public static Vector3 ZeroP8 = new Vector3(0.8f, 0.8f, 1);
        public static Vector3 ZeroP9 = new Vector3(0.9f, 0.9f, 1);
        public static Vector3 ZeroP30 = new Vector3(0, 0, 30);
        public static Vector3 P360 = new Vector3(0, 0, 360);
        public static Vector3 Two = Vector3.one * 2;
        public static Vector3 TwoP2 = new Vector3(2.2f, 2.2f, 1);
        public static Vector3 TwoP5 = new Vector3(2.5f, 2.5f, 1);
        public static Vector3 Three = Vector3.one * 3;
        public static Vector3 Other = new Vector3(-0.15f, 0, 0);
        public static Vector3 Other2 = new Vector3(-0.25f, 0, 0);

        public static float FastDistance(ref Vector3 start, ref Vector3 end)
        {
            return Mathf.Sqrt((end.x - start.x) * (end.x - start.x) + (end.y - start.y) * (end.y - start.y) +
                              (end.z - start.z) * (end.z - start.z));
        }

        public static void FastLerp(ref Vector3 start, ref Vector3 end, float percent, ref Vector3 result)
        {
            result.x = start.x + percent * (end.x - start.x);
            result.y = start.y + percent * (end.y - start.y);
            result.z = start.z + percent * (end.z - start.z);
        }
    }

    public static class Vector2Utils
    {
        public static Vector2 Zero = Vector2.zero;
    }
}