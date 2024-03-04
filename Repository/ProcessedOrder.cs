namespace Repository;

public class ProcessedOrder
{
    public Guid ProcessId = Guid.NewGuid();
    public DateTime ProcessDate = DateTime.Now;
    public Guid OrderId { get; set; }
}