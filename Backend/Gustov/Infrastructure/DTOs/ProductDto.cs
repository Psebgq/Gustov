using System.ComponentModel.DataAnnotations;

namespace Gustov.Infrastructure.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [MaxLength(200, ErrorMessage = "El nombre no puede exceder los 200 caracteres")]
        public string Name { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El orden debe ser un número positivo")]
        public int SortOrder { get; set; }

        [Required(ErrorMessage = "La descripción es requerida")]
        [MaxLength(1000, ErrorMessage = "La descripción no puede exceder los 1000 caracteres")]
        public string Description { get; set; }

        [Required(ErrorMessage = "El precio es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "La imagen es requerida")]
        [MaxLength(500, ErrorMessage = "La URL de la imagen no puede exceder los 500 caracteres")]
        public string Image { get; set; }

        public bool IsActive { get; set; }
        public DateOnly CreatedAt { get; set; }
        public DateOnly UpdatedAt { get; set; }
    }

    public class CreateProductDto
    {
        [Required(ErrorMessage = "El ID de categoría es requerido")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [MaxLength(200, ErrorMessage = "El nombre no puede exceder los 200 caracteres")]
        public string Name { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El orden debe ser un número positivo")]
        public int SortOrder { get; set; }

        [Required(ErrorMessage = "La descripción es requerida")]
        [MaxLength(1000, ErrorMessage = "La descripción no puede exceder los 1000 caracteres")]
        public string Description { get; set; }

        [Required(ErrorMessage = "El precio es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "La imagen es requerida")]
        [MaxLength(500, ErrorMessage = "La URL de la imagen no puede exceder los 500 caracteres")]
        public string Image { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class UpdateProductDto
    {
        [Required(ErrorMessage = "El ID de categoría es requerido")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [MaxLength(200, ErrorMessage = "El nombre no puede exceder los 200 caracteres")]
        public string Name { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El orden debe ser un número positivo")]
        public int SortOrder { get; set; }

        [Required(ErrorMessage = "La descripción es requerida")]
        [MaxLength(1000, ErrorMessage = "La descripción no puede exceder los 1000 caracteres")]
        public string Description { get; set; }

        [Required(ErrorMessage = "El precio es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "La imagen es requerida")]
        [MaxLength(500, ErrorMessage = "La URL de la imagen no puede exceder los 500 caracteres")]
        public string Image { get; set; }

        public bool IsActive { get; set; }
    }
}
