using System.Diagnostics;
using System.Text;
using MassTransit;
using PocWorker;
using PocWorker.Models;
using Shared.Messaging.Contracts;

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
        // string? parentActivityId = null;
        // if (context.Headers?.TryGetHeader("traceparent", out var parentActivityIdRaw) == true &&
        //     parentActivityIdRaw is byte[] traceParentBytes)
        //     parentActivityId = Encoding.UTF8.GetString(traceParentBytes);
        
        using var activity = TraceActivitySource.StartActivity(nameof(OrderMessage), kind: ActivityKind.Consumer);

        var message = context.Message;

        var order = new Order
        {
            OrderId = message.OrderId,
            OrderDate = message.OrderDate
        };

        activity.SetTag("orderId", order.OrderId);
        activity.SetTag("orderDate", order.OrderDate);

        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
    }
}