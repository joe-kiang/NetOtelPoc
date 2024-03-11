namespace PocProducer.Models;

public class ProcessedOrder
{
    public Guid ProcessId { get; set; }
    public DateTime ProcessDate { get; set; }
    public Guid OrderId { get; set; }
}