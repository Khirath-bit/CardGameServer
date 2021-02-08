using System;
using System.Collections.Generic;
using System.Text;

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
