namespace Shared.Messaging.Contracts
{
    public class OrderMessage
    {
        public Guid OrderId = new Guid();
        public DateTime OrderDate = DateTime.Now;
        public string? OrderOrigin { get; set; }
    }
}
