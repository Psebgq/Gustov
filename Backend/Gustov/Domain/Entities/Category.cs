namespace Gustov.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateOnly CreatedAt { get; set; }
        public DateOnly UpdatedAt { get; set; }

        //Relationships
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
