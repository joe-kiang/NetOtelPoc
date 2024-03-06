using System.Diagnostics;
using MassTransit;
using PocWorker.Models;
using Shared.Messaging.Contracts;

namespace PocWorker;

public class OrderMessageConsumer : IConsumer<OrderMessage>
{
    private readonly AppDbContext _context;
    public static readonly string TraceActivityName = typeof(OrderMessageConsumer).FullName!;
    private static readonly ActivitySource TraceActivitySource = new (TraceActivityName);

    public OrderMessageConsumer(AppDbContext context)
    {
        _context = context;
    }

    public async Task Consume(ConsumeContext<OrderMessage> context)
    {
        using var activity = TraceActivitySource.StartActivity(nameof(OrderMessage), ActivityKind.Consumer);

        var message = context.Message;

        var order = new Order
        {
            OrderId = message.OrderId,
            OrderDate = message.OrderDate,
            OrderOrigin = message.OrderOrigin
        };
        
        activity.SetTag("orderId", message.OrderId);

        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
    }
}