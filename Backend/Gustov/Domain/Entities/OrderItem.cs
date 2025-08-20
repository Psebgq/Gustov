namespace Gustov.Domain.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int SaleId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public bool IsActive { get; set; }
        public DateOnly CreatedAt { get; set; }
        public DateOnly UpdatedAt { get; set; }

        public Sale Sale { get; set; }
        public Category Category { get; set; }
    }
}
