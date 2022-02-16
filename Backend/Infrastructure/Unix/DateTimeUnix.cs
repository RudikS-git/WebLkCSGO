using System;

namespace Infrastructure.Unix
{
    static public class DateTimeUnix
    {
        static public DateTime UnixTimeStampToDateTime(long timestamp)
        {
            return new DateTime(1970, 1, 1).AddSeconds(timestamp);
        }

    }
}
