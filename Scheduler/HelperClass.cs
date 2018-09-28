using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scheduler
{
    public static class HelperClass
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            
            int daydiff = dt.DayOfWeek - startOfWeek;
            int diff = (7 + (daydiff)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
        public static DateTime EndOfWeek(this DateTime dt, DayOfWeek endOfWeek)
        {
            int daydiff = dt.DayOfWeek - endOfWeek;
            int diff = (7 + (daydiff)) % 7;
            return dt.AddDays(diff).Date;
        }
    }
}