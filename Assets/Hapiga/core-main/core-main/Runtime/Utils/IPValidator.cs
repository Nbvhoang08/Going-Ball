using System.Text.RegularExpressions;

namespace Hapiga.Core.Runtime.Utils
{
    public static class IPValidator
    {
        /// <summary>
        /// Returns true if the string parameter is a valid IPv4 addrees, false otherwise.
        /// </summary>
        /// <param name="str">IP string to be checked.</param>
        /// <returns></returns>
        public static bool IsValidIPAddress(string str)
        {
            return Regex.IsMatch(str, @"\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b");
        }
    }
}