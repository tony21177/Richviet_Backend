using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Richviet.IoC.Configuration.AutoMapper.Converter
{
    public class UnixTimestampDateTimeConverter : ITypeConverter<DateTime, long>
    {
        public long Convert(DateTime source, long destination, ResolutionContext context)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            var unixdatetime = (source.ToUniversalTime() - epoch).TotalSeconds;
            return (long)unixdatetime;
        }
    }

    public class UnixTimestampDateTimeOffsetConverter : ITypeConverter<DateTimeOffset, long>
    {
       

        public long Convert(DateTimeOffset source, long destination, ResolutionContext context)
        {
            return source.ToUnixTimeSeconds();
        }
    }
}
