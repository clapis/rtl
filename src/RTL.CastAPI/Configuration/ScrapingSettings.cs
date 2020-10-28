using System;
namespace RTL.CastAPI.Configuration
{
    public class ScrapingSettings
    {
        public bool IsEnabled { get; set; }
        public TimeSpan Interval { get; set; }
    }
}
