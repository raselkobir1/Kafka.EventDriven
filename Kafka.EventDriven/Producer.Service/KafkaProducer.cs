using Confluent.Kafka;
using Newtonsoft.Json;

namespace Producer.Service
{
    public class KafkaProducer : IKafkaProducer
    {
        private readonly IProducer<string, string> kafkaProducer;
        public KafkaProducer(string bootstrapServers, string clientId)
        {
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = bootstrapServers,
                ClientId = clientId,   
            };

            kafkaProducer = new ProducerBuilder<string, string>(producerConfig).Build();
        }
        public async Task Producer<T>(string topic, string key, T message) 
        {
            var serialized_message = JsonConvert.SerializeObject(message);
            await kafkaProducer.ProduceAsync(topic, new Message<string, string> { Key = key, Value = serialized_message });
        }
        public void ProducerDispose()
        {
            kafkaProducer?.Dispose();
        }
    }
}
