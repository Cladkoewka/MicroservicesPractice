using Confluent.Kafka;
using Serilog;

namespace OrderService;

public class KafkaProducerService
{
    private readonly string? _bootstrapServers;

    public KafkaProducerService(ConfigurationManager configuration)
    {
        _bootstrapServers = configuration["Kafka:BootstrapServers"];
    }

    public async Task ProduceMessageAsync(string topic, string message)
    {
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = _bootstrapServers
        };

        using var producer = new ProducerBuilder<Null, string>(producerConfig).Build();
        await producer.ProduceAsync(topic, new Message<Null, string> { Value = message });

        Console.WriteLine($"Published message to topic {topic}: {message}");
    }
}