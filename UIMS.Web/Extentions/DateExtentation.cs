using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UIMS.Web.Extentions
{
    public static class DateExtentation
    {
        public static bool IsToday(this DateTime date1)
        {
            DateTime today = DateTime.Now;
            return date1.Day == today.Day && date1.Year == today.Year && date1.Month == today.Month;
        }


    }
}
