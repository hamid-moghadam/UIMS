using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UIMS.Web.Extentions
{
    public static class StringExtentions
    {
        public static bool IsNumber(this string number)
        {
            return Regex.IsMatch(number, @"^[0-9]+$");
        }

    }
}
