using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Minilytics.Controllers
{
    [Route("api/")]
    public class EventsController : Controller
    {
        IEventStore eventStore;

        public EventsController(IEventStore eventStore)
        {
            this.eventStore = eventStore;
        }


        [HttpPost("events")]
        public async Task<string> PostEvent([FromBody]Event value)
        {
            value.EventId = Guid.NewGuid().ToString();
            if (Request.Headers.ContainsKey("x-forwarded-ip"))
                value.IpAddress = Request.Headers["x-forwarded-ip"];
            else
                value.IpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            value.ReceivedDateTime = DateTime.UtcNow;
            await eventStore.StoreEventAsync(value);
            return "OK";
        }

        [HttpPost("exceptions")]
        public async Task<string> PostException([FromBody]ClientException value)
        {
            value.ExceptionId = Guid.NewGuid().ToString();
            if (Request.Headers.ContainsKey("x-forwarded-ip"))
                value.IpAddress = Request.Headers["x-forwarded-ip"];
            else
                value.IpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            value.ReceivedDateTime = DateTime.UtcNow;
            await eventStore.StoreExceptionAsync(value);
            return "OK";
        }
    }
}
