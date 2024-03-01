using System.Diagnostics;
using MassTransit;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Sinks.OpenTelemetry;
using Shared.Messaging.Contracts;

var builder = WebApplication.CreateBuilder(args);


const string serviceName = "PocApi";

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.AddOpenTelemetry(options =>
{
    options.SetResourceBuilder(
            ResourceBuilder.CreateDefault()
                .AddService(serviceName))
        .AddOtlpExporter(ops =>
        {
            ops.Endpoint = new Uri("http://otel-collector:4317");
            ops.Protocol = OtlpExportProtocol.Grpc;
        });
});
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource
        .AddService(serviceName))
    .WithTracing(tracer => tracer
        .AddSource("*")
        .AddHttpClientInstrumentation()
        .AddAspNetCoreInstrumentation()
        .AddOtlpExporter(ops =>
        {
            ops.Endpoint = new Uri("http://otel-collector:4317");
            ops.Protocol = OtlpExportProtocol.Grpc;
        }))
    .WithMetrics(metrics => metrics
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddOtlpExporter(ops =>
        {
            ops.Endpoint = new Uri("http://otel-collector:4317");
            ops.Protocol = OtlpExportProtocol.Grpc;
        }));

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