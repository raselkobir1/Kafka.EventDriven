
namespace Consumer.Service
{
    public interface IKafkaConsumer
    {
        public Task StartConsuming(string topic);
    }
}
