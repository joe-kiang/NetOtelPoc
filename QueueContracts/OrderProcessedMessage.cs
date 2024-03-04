namespace QueueContracts
{
    public class OrderProcessedMessage
    {
        public Guid ProcessId = Guid.NewGuid();
        public DateTime ProcessDate = DateTime.Now;
        public Guid OrderId { get; set; }
    }
}