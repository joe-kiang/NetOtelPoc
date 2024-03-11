using System.Diagnostics;
using System.Text.Json;
using MassTransit;
using PocProducer.Models;
using StackExchange.Redis;

namespace PocProcessConsumer;

public class ProcessMessageConsumer : IConsumer<ProcessedOrder>
{
    private static readonly ActivitySource TraceActivitySource = new ("Poc.Consumer.Process");
    private static readonly IDatabase Database = RedisConnectorHelper.Connection.GetDatabase();

    public async Task Consume(ConsumeContext<ProcessedOrder> context)
    {
        using var activity = TraceActivitySource.StartActivity(nameof(Consume), kind: ActivityKind.Consumer);

        var message = context.Message;

        var processedOrder = new ProcessedOrder()
        {
            OrderId = message.OrderId,
            ProcessId = new Guid(),
            ProcessDate = DateTime.Now
        };
        
        var processedOrderJson = JsonSerializer.Serialize(processedOrder);
        var redisKey = $"processedOrder:{processedOrder.ProcessId}";
        await Database.StringSetAsync(redisKey, processedOrderJson);

        activity?.SetTag("Process", processedOrder);
    }
}