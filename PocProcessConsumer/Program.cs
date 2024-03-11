using MassTransit;
using PocProcessConsumer;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<ProcessMessageConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq", "/", h => {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("process-queue", e =>
        {
            e.ConfigureConsumer<ProcessMessageConsumer>(context);
        });
    });
});

var host = builder.Build();
host.Run();