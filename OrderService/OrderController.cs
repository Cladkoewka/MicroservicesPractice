using System.Text.Json;
using MessageContracts;
using Microsoft.AspNetCore.Mvc;

namespace OrderService;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly KafkaProducerService _kafkaProducerService;
    private readonly string? _orderCreatedTopic;

    public OrderController(KafkaProducerService kafkaProducerService, IConfiguration configuration)
    {
        _kafkaProducerService = kafkaProducerService;
        _orderCreatedTopic = configuration["Kafka:OrderCreatedTopic"];
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {
        var orderId = Guid.NewGuid();

        var message = new OrderCreated
        {
            OrderId = orderId,
            CustomerName = request.CustomerName,
            Amount = request.Amount
        };

        string serializedMessage = JsonSerializer.Serialize(message);

        await _kafkaProducerService.ProduceMessageAsync(_orderCreatedTopic, serializedMessage);

        return Ok(new { OrderId = orderId });
    }
}