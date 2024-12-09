using Confluent.Kafka;

namespace NotificationService;

public class NotificationService
{
    private readonly string _bootstrapServers;
    private readonly string _topicToConsume;

    public NotificationService(string bootstrapServers, string topicToConsume)
    {
        _bootstrapServers = bootstrapServers;
        _topicToConsume = topicToConsume;
    }

    public void StartConsuming()
    {
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = _bootstrapServers,
            GroupId = "notification-service-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        Task.Run(() =>
        {
            using var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
            consumer.Subscribe(_topicToConsume);

            Console.WriteLine($"NotificationService is listening to {_topicToConsume}...");

            while (true)
            {
                var message = consumer.Consume();
                Console.WriteLine($"Sending notification: {message.Value}");
            }
        });
    }
}