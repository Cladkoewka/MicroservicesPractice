using Confluent.Kafka;

namespace OrderService;

public class KafkaProducerService
{
    private readonly string _bootstrapServers;

    public KafkaProducerService(string bootstrapServers)
    {
        _bootstrapServers = bootstrapServers;
    }

    public async Task ProduceMessageAsync(string topic, string message)
    {
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = _bootstrapServers
        };

        using var producer = new ProducerBuilder<Null, string>(producerConfig).Build();
        await producer.ProduceAsync(topic, new Message<Null, string> { Value = message });

        Console.WriteLine($"Published: {message}");
    }
}