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
        var message = context.Message;

        var order = new Order
        {
            OrderId = message.OrderId,
            OrderDate = message.OrderDate,
            OrderOrigin = message.OrderOrigin
        };

        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
    }
}