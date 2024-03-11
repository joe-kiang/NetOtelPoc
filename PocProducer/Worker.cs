using System.Diagnostics;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using PocProducer.Models;

namespace PocProducer;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IBus _bus;
    private readonly IServiceProvider _provider;
    private static readonly ActivitySource TraceActivitySource = new ("Poc.Producer.Process");
    public Worker(ILogger<Worker> logger, IBus bus, IServiceProvider provider)
    {
        _logger = logger;
        _bus = bus;
        _provider = provider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            using (var scope = _provider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var orders = await dbContext.Orders.Where( order => order.ProcessedDate == null).ToListAsync(stoppingToken);
                
                foreach (var order in orders)
                {
                    var tracer = ActivityTraceId.CreateFromString(order.OrderTraceId);
                    var context = new ActivityContext(tracer, ActivitySpanId.CreateRandom(), ActivityTraceFlags.Recorded);
                    
                    using var activity = TraceActivitySource.StartActivity(nameof(ExecuteAsync), kind: ActivityKind.Producer, context);

                    var process = new ProcessedOrder
                    {
                        OrderId = order.OrderId,
                        ProcessId = new Guid(),
                        ProcessDate = DateTime.Now
                    };
                    
                    activity?.SetTag("process", process);
                    
                    _logger.LogInformation($"Published order {order.OrderId}");
                    
                    await _bus.Publish(process, stoppingToken);
                    
                    order.ProcessedDate = DateTime.Now;

                    dbContext.Orders.Update(order);
                    await dbContext.SaveChangesAsync();
                }            
            }
            await Task.Delay(10000, stoppingToken);
        }
    }
}