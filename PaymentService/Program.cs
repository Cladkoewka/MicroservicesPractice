var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<PaymentService.PaymentService>(sp =>
    new PaymentService.PaymentService("localhost:9092", "OrderCreated", "PaymentProcessed"));

var app = builder.Build();

var paymentService = app.Services.GetRequiredService<PaymentService.PaymentService>();
paymentService.StartConsuming();

app.Run();
