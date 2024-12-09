using Confluent.Kafka;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IConsumer<Null, string>>(sp =>
{
    var config = new ConsumerConfig
    {
        BootstrapServers = builder.Configuration["Kafka:BootstrapServers"],
        GroupId = "payment-service-group",
        AutoOffsetReset = AutoOffsetReset.Earliest
    };
    return new ConsumerBuilder<Null, string>(config).Build();
});

builder.Services.AddSingleton<IProducer<Null, string>>(sp =>
{
    var config = new ProducerConfig
    {
        BootstrapServers = builder.Configuration["Kafka:BootstrapServers"]
    };
    return new ProducerBuilder<Null, string>(config).Build();
});

builder.Services.AddSingleton<PaymentService.PaymentService>();

var app = builder.Build();

var paymentService = app.Services.GetRequiredService<PaymentService.PaymentService>();
paymentService.Start();

app.Run("http://127.0.0.1:5007");