using Kafka.EventDriven.Controllers.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Producer.Service;

namespace Kafka.EventDriven.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProducerController : ControllerBase
    {
        private readonly IKafkaProducer _kafkaProducer;
        public ProducerController(IKafkaProducer kafkaProducer)
        {
              _kafkaProducer = kafkaProducer;
        }

        [HttpPost("Place-Order")]
        public async Task<IActionResult> PlaceOrder([FromBody]OrderDetails orderDetails) 
        {
            var topic = "order-place";
            var key = "Key_1";
            await _kafkaProducer.SendMessage(topic,key,orderDetails);
            return Ok("Order placed successfully");
        }
    }
}
