using MassTransit;
using Shared.Messaging.Contracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapPost("/order", async (OrderMessage order, IPublishEndpoint publishEndpoint) =>
    {
        await publishEndpoint.Publish(order);
        return Results.Ok();
    })
    .WithName("CreateOrder")
    .WithOpenApi();

app.Run();