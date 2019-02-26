using System;

namespace Contracts
{
    public class ClientException
    {
        public string ExceptionId { get; set; }
        public DateTime ReceivedDateTime { get; set; }
        public string IpAddress { get; set; }
        public string IpLocation { get; set; }
        public string Browser { get; set; }
        public string Error { get; set; }
        public string LocalStorageStatus { get; set; }
    }
}
