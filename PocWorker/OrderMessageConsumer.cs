using MassTransit;
using PocWorker;
using PocWorker.Models;
using Shared.Messaging.Contracts;

public class OrderMessageConsumer : IConsumer<OrderMessage>
{
    private readonly AppDbContext _context;

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
            OrderDate = message.OrderDate
        };

        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
    }
}