using System;

namespace CustomersApi.Models
{
    public class RequestLogEntry
    {
        public DateTime Timestamp { get; set; }

        public string Method { get; set; }

        public string Url { get; set; }

        public string Body { get; set; }

        public int StatusCode { get; set; }

        public long DurationMs { get; set; }

        public string CorrelationId { get; set; }
    }
}
