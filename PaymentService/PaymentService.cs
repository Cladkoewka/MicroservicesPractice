using System.Text.Json;
using Confluent.Kafka;
using MessageContracts;

namespace PaymentService;

public class PaymentService
{
    private readonly IConsumer<Null, string> _kafkaConsumer;
    private readonly IProducer<Null, string> _kafkaProducer;
    private readonly string? _paymentProcessedTopic;

    public PaymentService(IConsumer<Null, string> kafkaConsumer, IProducer<Null, string> kafkaProducer, IConfiguration configuration)
    {
        _kafkaConsumer = kafkaConsumer;
        _kafkaProducer = kafkaProducer;
        _paymentProcessedTopic = configuration["Kafka:PaymentProcessedTopic"];
    }

    public void Start()
    {
        Console.WriteLine("Start");
            
        Task.Run(() =>
        {
            _kafkaConsumer.Subscribe("OrderCreated");

            while (true)
            {
                var result = _kafkaConsumer.Consume();
                Console.WriteLine(result);
                var order = JsonSerializer.Deserialize<OrderCreated>(result.Message.Value);

                bool isSuccess = order.Amount < 10000; // Simple logic
                var paymentMessage = new PaymentProcessed
                {
                    OrderId = order.OrderId,
                    IsSuccess = isSuccess,
                    Reason = isSuccess ? "Approved" : "Insufficient funds"
                };
                
                _kafkaProducer.Produce(_paymentProcessedTopic, new Message<Null, string>
                {
                    Value = JsonSerializer.Serialize(paymentMessage)
                });
                
                Console.WriteLine($"Processed payment for order {order.OrderId}");
            }
        });
    }
}