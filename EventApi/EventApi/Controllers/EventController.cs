using EventLib.Services;
using Microsoft.AspNetCore.Mvc;
using QueueLib;

namespace EventApi.Controllers
{
    [Route("api/")]
    public class EventController : ControllerBase
    {
        private readonly IDispatcher _dispatcher;
        private readonly IConsumer _consumer;

        public EventController(IDispatcher dispatcher, IConsumer consumer)
        {
            _dispatcher = dispatcher;
            _consumer = consumer;
        }

        [HttpPost("SendError")]
        public string SendError([FromBody]ErrorMessage message)
        {
            _dispatcher.Send(message, _consumer.GetResponceProperties());
            return _consumer.Consume();
        }

        [HttpPost("SendWarn")]
        public string SendWarn([FromBody]WarnMessage message)
        {
            _dispatcher.Send(message, _consumer.GetResponceProperties());
            return _consumer.Consume();
        }

        [HttpPost("SendInfo")]
        public string SendInfo([FromBody]InfoMessage message)
        {
            _dispatcher.Send(message, _consumer.GetResponceProperties());
            return _consumer.Consume();
        }
    }
}
