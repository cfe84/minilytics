using System;

namespace Contracts
{
    public class Event
    {
        public string EventId { get; set; }
        public DateTime ReceivedDateTime { get; set; }
        public DateTime SentDateTime { get; set; }
        public string EventName { get; set; }
        public string IpAddress { get; set; }
        public string IpLocation { get; set; }
        public string FullUrl { get; set; }
        public string Path { get; set; }
        public string Referrer { get; set; }
        public string UserId { get; set; }
    }
}
