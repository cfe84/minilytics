using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Minilytics.Controllers
{
    [Route("api/[controller]")]
    public class EventsController : Controller
    {
        IEventStore eventStore;

        public EventsController(IEventStore eventStore)
        {
            this.eventStore = eventStore;
        }


        // POST api/values
        [HttpPost]
        public async Task Post([FromBody]Event value)
        {
            value.EventId = Guid.NewGuid().ToString();
            value.IpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            value.ReceivedDateTime = DateTime.UtcNow;
            value.SentDateTime = DateTime.UtcNow;
            await eventStore.StoreEventAsync(value);
        }
        
    }
}
