using System.Diagnostics;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using QueueContracts;
using Repository;

namespace PocProducer;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IBus _bus;
    private readonly IServiceProvider _provider;
    
    public static readonly string TraceActivityName = typeof(Worker).FullName!;
    private static readonly ActivitySource TraceActivitySource = new (TraceActivityName);

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
            using var activity = TraceActivitySource.StartActivity("OrderSender", kind: ActivityKind.Producer);

            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            activity.AddEvent(new ActivityEvent("Worker running"));
            
            using (var scope = _provider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var orders = await dbContext.Orders.ToListAsync(stoppingToken);
                
                activity.AddEvent(new ActivityEvent("Orders Retrived"));

                foreach (var order in orders)
                {
                    var msg = new OrderProcessedMessage()
                    {
                        OrderId = order.OrderId
                    };
                    
                    activity.SetTag("orderId", order.OrderId);
                    
                    await _bus.Publish(msg, stoppingToken);
                    _logger.LogInformation($"Published order {order.OrderId}");
                }
            }
            activity.AddEvent(new ActivityEvent("Finish Sending"));

            await Task.Delay(10000, stoppingToken);
        }
    }
}