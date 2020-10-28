using System;
namespace RTL.CastAPI.Configuration
{
    public class SynchronizationSettings
    {
        public bool IsEnabled { get; set; }
        public TimeSpan Interval { get; set; }
    }
}
