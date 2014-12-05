using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OurEDA.DianShangYun.Management.Comment
{
    public static class ConvertHelp
    {
        public static long DatetimeToUnixTime(this DateTime dt)
        {
            //DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            //return (long)(dt - startTime).TotalSeconds;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (dt.Ticks - startTime.Ticks) / 10000;            //除10000调整为13位
            return t;
        }
        public static DateTime UnixTimeToDatetime(this long dt)
        {
            System.DateTime time = System.DateTime.MinValue;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            time = startTime.AddMilliseconds(dt);
            return time;
        }
    }
}