using Confluent.Kafka;
using Newtonsoft.Json;

namespace Producer.Service
{
    public class KafkaProducer : IKafkaProducer
    {
        private readonly IProducer<string, string> kafkaProducer;
        public KafkaProducer(string bootstrapServers)
        {
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = bootstrapServers,
            };

            kafkaProducer = new ProducerBuilder<string, string>(producerConfig).Build();
        }
        public async Task SendMessage<T>(string topic, string key, T message)
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
