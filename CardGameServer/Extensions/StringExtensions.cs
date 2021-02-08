using System;

namespace CardGameServer.Extensions
{
    public static class StringExtensions
    {
        public static bool EqualsIgnoreCase(this string val, string value)
        {
            return string.Equals(val, value, StringComparison.OrdinalIgnoreCase);
        }
    }
}
