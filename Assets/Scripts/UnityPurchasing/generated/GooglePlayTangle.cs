// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("/uSgat2TN/5gMmjAQwPkZXgwe9+crxg+mmEC5KX9YhPN9X7LQilbPr1iz64Qs7OVdbBlqzevwuZEYW8wBrQ3FAY7MD8csH6wwTs3NzczNjUJ6Kwo4+a/E7iKhT+ZvGChZB0UqF/HqK2qUOY3T46Kcy6bJI5V9g6P/p0p4DAv4sCZ0XwVdMWLUBu40yK0Nzk2BrQ3PDS0Nzc25dOoSeTteY0UpsN+oo2ihnFBpyaV1n238ftelOr1TXX0GrUmphj2AP1Gbkf0G1KznRvvDaiRtSkgR+9Xe0Ph4EsyA2WyQzOBjWHw/6gHqUyks0YhoW2qdSRBI7AVVHr4u/Uxl15Aw5/9tQZyK5uca9zPK+zv0BchB5rtJnNq9HdBS3Y62bkbIzQ1NzY3");
        private static int[] order = new int[] { 2,9,13,13,8,5,12,9,9,9,12,13,12,13,14 };
        private static int key = 54;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
