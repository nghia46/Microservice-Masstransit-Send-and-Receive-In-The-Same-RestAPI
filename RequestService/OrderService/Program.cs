using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Configure MassTransit and RabbitMQ
var bus = Bus.Factory.CreateUsingRabbitMq(config =>
{
    // Configure RabbitMQ host
    config.Host(new Uri("rabbitmq://guest:guest@localhost:5672"));
});

builder.Services.AddMassTransit(config =>
{
    // Configure MassTransit with RabbitMQ
    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("rabbitmq://guest:guest@localhost:5672");
    });
});
// Add MassTransit hosted service
builder.Services.AddHostedService<MassTransitHostedService>();
builder.Services.AddSingleton(bus);
var app = builder.Build();
// Configure MassTransit bus control
var busControl = app.Services.GetRequiredService<IBusControl>();
await busControl.StartAsync();
// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
