namespace PocProducer.Models
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string? OrderOrigin { get; set; }
    }
}