using Confluent.Kafka;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day) // Логи в файл, по дням
    .CreateLogger();

builder.Services.AddSingleton<IConsumer<Null, string>>(sp =>
{
    var config = new ConsumerConfig
    {
        BootstrapServers = builder.Configuration["Kafka:BootstrapServers"],
        GroupId = "notification-service-group",
        AutoOffsetReset = AutoOffsetReset.Earliest
    };
    return new ConsumerBuilder<Null, string>(config).Build();
});

builder.Services.AddSingleton<NotificationService.NotificationService>();

var app = builder.Build();

var notificationService = app.Services.GetRequiredService<NotificationService.NotificationService>();
notificationService.Start();

app.Run("http://127.0.0.1:5006");