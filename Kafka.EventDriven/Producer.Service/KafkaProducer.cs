using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Producer.Service
{
    public class KafkaProducer : IKafkaProducer
    {
        private readonly IProducer<string, string> kafkaProducer;
        private readonly ILogger<KafkaProducer> _logger;
        public KafkaProducer(string bootstrapServers, string clientId, ILogger<KafkaProducer> logger = null)
        {
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = bootstrapServers,
                ClientId = clientId,   
            };
            _logger = logger;
            kafkaProducer = new ProducerBuilder<string, string>(producerConfig).Build();
        }
        public async Task Producer<T>(string topic, string key, T message) 
        {
            try
            {
                var serialized_message = JsonConvert.SerializeObject(message);
                var kafkaResult = await kafkaProducer.ProduceAsync(topic, new Message<string, string> { Key = key, Value = serialized_message });
                if (kafkaResult != null)
                {
                    //send notifcation
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error,ex.Message);
                throw;
            }
        }

        public void ProducerDispose()
        {
            kafkaProducer?.Dispose();
        }
    }
}
