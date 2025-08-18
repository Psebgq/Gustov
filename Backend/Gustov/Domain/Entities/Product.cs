namespace Gustov.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; }
        public DateOnly CreatedAt { get; set; }
        public DateOnly UpdatedAt { get; set; }
        public Category Category { get; set; }
    }
}
