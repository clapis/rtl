using System;

namespace RTL.CastAPI.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToDateFormat(this DateTime? datetime)
            => datetime.HasValue ? datetime.Value.ToString("yyyy-MM-dd") : null;

    }
}
