using System.Diagnostics;
using MassTransit;
using QueueContracts;
using Repository;

namespace PocWorker;

public class OrderProcessedConsumer : IConsumer<OrderProcessedMessage>
{
    private readonly AppDbContext _context;
    public static readonly string TraceActivityName = typeof(OrderProcessedConsumer).FullName!;
    private static readonly ActivitySource TraceActivitySource = new (TraceActivityName);

    public OrderProcessedConsumer(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task Consume(ConsumeContext<OrderProcessedMessage> context)
    {
        using var activity = TraceActivitySource.StartActivity(nameof(OrderProcessedMessage), ActivityKind.Consumer);

        var message = context.Message;

        var order = new Order
        {
            OrderId = message.OrderId
        };
        
        activity.SetTag("orderId", message.OrderId);

        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
    }
}