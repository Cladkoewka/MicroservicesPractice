using Confluent.Kafka;

namespace PaymentService;

public class PaymentService
{
    private readonly string _bootstrapServers;
    private readonly string _topicToConsume;
    private readonly string _topicToProduce;

    public PaymentService(string bootstrapServers, string topicToConsume, string topicToProduce)
    {
        _bootstrapServers = bootstrapServers;
        _topicToConsume = topicToConsume;
        _topicToProduce = topicToProduce;
    }

    public void StartConsuming()
    {
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = _bootstrapServers,
            GroupId = "payment-service-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        Task.Run(() =>
        {
            using var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
            consumer.Subscribe(_topicToConsume);

            Console.WriteLine($"PaymentService is listening to {_topicToConsume}...");

            while (true)
            {
                var message = consumer.Consume();
                Console.WriteLine($"Processing payment for: {message.Value}");

                var paymentMessage = message.Value.Replace("created", "payment successful");

                using var producer = new ProducerBuilder<Null, string>(new ProducerConfig { BootstrapServers = _bootstrapServers }).Build();
                producer.Produce(_topicToProduce, new Message<Null, string> { Value = paymentMessage });

                Console.WriteLine($"Published: {paymentMessage}");
            }
        });
    }
}