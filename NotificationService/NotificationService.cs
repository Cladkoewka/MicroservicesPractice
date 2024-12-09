using System.Text.Json;
using Confluent.Kafka;
using MessageContracts;

namespace NotificationService;

public class NotificationService
{
    private readonly IConsumer<Null, string> _kafkaConsumer;

    public NotificationService(IConsumer<Null, string> kafkaConsumer)
    {
        _kafkaConsumer = kafkaConsumer;
    }

    public void Start()
    {
        Task.Run(() =>
        {
            _kafkaConsumer.Subscribe("PaymentProcessed");

            Console.WriteLine("NotificationService is listening to PaymentProcessed...");
            
            while (true)
            {
                try
                {
                    var result = _kafkaConsumer.Consume();
                    var payment = JsonSerializer.Deserialize<PaymentProcessed>(result.Message.Value);

                    if (payment != null)
                    {
                        var notification = payment.IsSuccess
                            ? $"Order {payment.OrderId}: Payment approved."
                            : $"Order {payment.OrderId}: Payment failed. Reason: {payment.Reason}";

                        Console.WriteLine($"Notification: {notification}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error while consuming message: {ex.Message}");
                }
            }
        });
    }
}