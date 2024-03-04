using MassTransit;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using PocProducer;
using Repository;
using Serilog;
using Serilog.Sinks.OpenTelemetry;

var builder = Host.CreateApplicationBuilder(args);

const string serviceName = "PocProducer";

Log.Logger = new LoggerConfiguration()
    .WriteTo.OpenTelemetry(options =>
    {
        options.Endpoint = "http://otel-collector:4317";
        options.Protocol = OtlpProtocol.Grpc;
    })
    .CreateLogger();

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource
        .AddService(serviceName))
    .WithTracing(tracer => tracer
        .AddSource("*")
        .AddAspNetCoreInstrumentation()
        .AddEntityFrameworkCoreInstrumentation(options =>
        {
            options.SetDbStatementForText = true;
        })
        .AddOtlpExporter(ops =>
        {
            ops.Endpoint = new Uri("http://otel-collector:4317");
            ops.Protocol = OtlpExportProtocol.Grpc;
        }))
    .WithMetrics(metrics => metrics
        .AddAspNetCoreInstrumentation()
        .AddOtlpExporter(ops =>
        {
            ops.Endpoint = new Uri("http://otel-collector:4317");
            ops.Protocol = OtlpExportProtocol.Grpc;
        }));

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.EnableSensitiveDataLogging();
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

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

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();