using System;

namespace Oversight.Collector.Model
{
    public class OversightStatus
    {
        public string ServerName { get; set; }
        public string ServiceName { get; set; }
        public string Status { get; set; }
        public DateTime Timestamp { get; set; }
    }
}