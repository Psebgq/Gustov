namespace Gustov.Domain.Entities
{
    public class Sale
    {
        public int Id { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TipAmount { get; set; }
        public decimal Total { get; set; }
        public decimal CashRecieved { get; set; }
        public decimal CashChange { get; set; }
        public DateOnly CreatedAt { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
