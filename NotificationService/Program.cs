var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<NotificationService.NotificationService>(sp =>
    new NotificationService.NotificationService("localhost:9092", "PaymentProcessed"));

var app = builder.Build();

var notificationService = app.Services.GetRequiredService<NotificationService.NotificationService>();
notificationService.StartConsuming();

app.Run();
