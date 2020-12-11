using System;
using System.Collections.Generic;
using System.Text;

namespace Richviet.Tools.Utility
{
    public class TimeUtil
    {
        public static readonly DateTime UtcMinTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime LongSpanToUtcDateTime(long longSpan)
        {
            return UtcMinTime.Add(new TimeSpan(longSpan));
        }
    }
}
