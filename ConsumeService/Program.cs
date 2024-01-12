using InventoryService;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<RequestConsumer>();

    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("rabbitmq://guest:guest@localhost:5672");

        cfg.ReceiveEndpoint("request-queue", c =>
        {
            c.ConfigureConsumer<RequestConsumer>(ctx);
        });
    });
});

builder.Services.AddHostedService<MassTransitHostedService>();
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

var busControl = app.Services.GetRequiredService<IBusControl>();
await busControl.StartAsync();

app.MapControllers();
app.Run();
