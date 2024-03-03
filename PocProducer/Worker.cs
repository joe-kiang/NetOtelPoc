using System.Diagnostics;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace PocProducer;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly AppDbContext _dbContext;
    private readonly ActivitySource _activitySource;

    public Worker(ILogger<Worker> logger, IPublishEndpoint publishEndpoint, AppDbContext dbContext, ActivitySource activitySource)
    {
        _logger = logger;
        _publishEndpoint = publishEndpoint;
        _dbContext = dbContext;
        _activitySource = new ActivitySource("OrderProducer");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var span = _activitySource.StartActivity("Producing Orders");
            
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            var orders = await _dbContext.Orders.ToListAsync(stoppingToken);

            span.AddEvent(new ActivityEvent("Orders Retrived"));
            span.SetTag("Count", orders.Count);
            
            foreach (var order in orders)
            {
                await _publishEndpoint.Publish(order, stoppingToken);
                _logger.LogInformation($"Published order {order.OrderId}");
            }

            span.AddEvent(new ActivityEvent("Finish Sending"));

            await Task.Delay(10000, stoppingToken);
        }
    }
}