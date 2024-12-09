using OrderService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<KafkaProducerService>(sp => 
    new KafkaProducerService("localhost:9092"));

var app = builder.Build();

app.MapPost("/orders", async (KafkaProducerService kafkaProducer) =>
{
    var orderId = Guid.NewGuid().ToString();
    var message = $"Order {orderId} created";

    await kafkaProducer.ProduceMessageAsync("OrderCreated", message);

    return Results.Ok($"Order {orderId} created and published to Kafka.");
});

app.Run();
