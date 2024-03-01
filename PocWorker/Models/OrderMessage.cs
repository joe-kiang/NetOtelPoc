namespace Shared.Messaging.Contracts
{
    public class OrderMessage
    {
        public Guid OrderId { get; set; }
        public DateTime OrderDate { get; set; }
    }
}