using System.Diagnostics;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace PocProducer;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IBus _bus;
    private readonly IServiceProvider _provider;
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
                var orders = await dbContext.Orders.ToListAsync(stoppingToken);
                
                foreach (var order in orders)
                {
                    await _bus.Publish(order, stoppingToken);
                    _logger.LogInformation($"Published order {order.OrderId}");
                }
            }
            await Task.Delay(10000, stoppingToken);
        }
    }
}