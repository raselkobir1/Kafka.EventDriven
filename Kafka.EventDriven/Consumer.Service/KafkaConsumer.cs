
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using System.Threading;

namespace Consumer.Service
{
    public class KafkaConsumer: IHostedService
    {
        private readonly IConsumer<string, string> kafkaConsumer;

        public KafkaConsumer(string bootstrapServers, string groupId, string autoOffsetRest, string enableAutoOffsetStore)
        {
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = bootstrapServers,
                GroupId = groupId,
                AutoOffsetReset = ConvertToAutoOffsetReset(autoOffsetRest),// AutoOffsetReset.Earliest, 
                EnableAutoOffsetStore = Convert.ToBoolean(enableAutoOffsetStore),
            };

            this.kafkaConsumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
        }

        static AutoOffsetReset ConvertToAutoOffsetReset(string value)
        {
            return value.ToLower() switch
            {
                "earliest" => AutoOffsetReset.Earliest,
                "latest" => AutoOffsetReset.Latest,
                "error" => AutoOffsetReset.Error,
                _ => throw new ArgumentException($"Invalid AutoOffsetReset value: {value}")
            };
        }

        public void ConsumerDispose()
        {
            // Properly dispose of the Kafka consumer
            kafkaConsumer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var topic = "order-place";
            kafkaConsumer.Subscribe(topic);
            // Start an infinite loop with background service to consume messages
            Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        // Consume messages from the Kafka topic
                        var consumeResult = kafkaConsumer.Consume(CancellationToken.None);
                        if (consumeResult is null)
                        {
                            return;
                        }

                        // Process the consumed message (implement your logic here)
                        Console.WriteLine($"Received message: {consumeResult.Offset} \n");
                        Console.WriteLine($"Received message: {consumeResult.Message.Key} \n");
                        Console.WriteLine($"Received message: {consumeResult.Message.Value} \n\n\n");
                    }
                    catch (ConsumeException ex)
                    {
                        // Handle any errors that occur during message consumption
                        Console.WriteLine($"Error consuming message: {ex.Error.Reason}");
                    }
                }
            }, cancellationToken);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
