using System.ComponentModel.DataAnnotations;

namespace Gustov.Infrastructure.DTOs
{
    public class OrderItemDto
    {
        public int Id { get; set; }
        public int SaleId { get; set; }
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [MaxLength(200, ErrorMessage = "El nombre no puede exceder los 200 caracteres")]
        public string Name { get; set; }

        [Required(ErrorMessage = "La cantidad es requerida")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "El precio unitario es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio unitario debe ser mayor a 0")]
        public decimal UnitPrice { get; set; }

        [Required(ErrorMessage = "El precio total es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio total debe ser mayor a 0")]
        public decimal TotalPrice { get; set; }

        public bool IsActive { get; set; }
        public DateOnly CreatedAt { get; set; }
        public DateOnly UpdatedAt { get; set; }
        public string? CategoryName { get; set; }
    }

    public class CreateOrderItemDto
    {
        [Required(ErrorMessage = "El ID de venta es requerido")]
        public int SaleId { get; set; }

        [Required(ErrorMessage = "El ID de categoría es requerido")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [MaxLength(200, ErrorMessage = "El nombre no puede exceder los 200 caracteres")]
        public string Name { get; set; }

        [Required(ErrorMessage = "La cantidad es requerida")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "El precio unitario es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio unitario debe ser mayor a 0")]
        public decimal UnitPrice { get; set; }

        [Required(ErrorMessage = "El precio total es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio total debe ser mayor a 0")]
        public decimal TotalPrice { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class UpdateOrderItemDto
    {
        [Required(ErrorMessage = "El ID de venta es requerido")]
        public int SaleId { get; set; }

        [Required(ErrorMessage = "El ID de categoría es requerido")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [MaxLength(200, ErrorMessage = "El nombre no puede exceder los 200 caracteres")]
        public string Name { get; set; }

        [Required(ErrorMessage = "La cantidad es requerida")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "El precio unitario es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio unitario debe ser mayor a 0")]
        public decimal UnitPrice { get; set; }

        [Required(ErrorMessage = "El precio total es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio total debe ser mayor a 0")]
        public decimal TotalPrice { get; set; }

        public bool IsActive { get; set; }
    }
}