using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame.Extensions
{
    public static class StringExtensions
    {
        public static bool EqualsIgnoreCase(this string val, string value)
        {
            return string.Equals(val, value, StringComparison.OrdinalIgnoreCase);
        }
    }
}
