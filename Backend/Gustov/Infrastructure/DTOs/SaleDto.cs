using System.ComponentModel.DataAnnotations;

namespace Gustov.Infrastructure.DTOs
{
    public class SaleDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El subtotal es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El subtotal debe ser mayor a 0")]
        public decimal SubTotal { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "La propina debe ser mayor o igual a 0")]
        public decimal TipAmount { get; set; }

        [Required(ErrorMessage = "El total es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El total debe ser mayor a 0")]
        public decimal Total { get; set; }

        [Required(ErrorMessage = "El efectivo recibido es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El efectivo recibido debe ser mayor a 0")]
        public decimal CashRecieved { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El cambio debe ser mayor o igual a 0")]
        public decimal CashChange { get; set; }

        public DateOnly CreatedAt { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
    }

    public class CreateSaleDto
    {
        [Required(ErrorMessage = "El subtotal es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El subtotal debe ser mayor a 0")]
        public decimal SubTotal { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "La propina debe ser mayor o igual a 0")]
        public decimal TipAmount { get; set; } = 0;

        [Required(ErrorMessage = "El total es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El total debe ser mayor a 0")]
        public decimal Total { get; set; }

        [Required(ErrorMessage = "El efectivo recibido es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El efectivo recibido debe ser mayor a 0")]
        public decimal CashRecieved { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El cambio debe ser mayor o igual a 0")]
        public decimal CashChange { get; set; } = 0;

        public List<CreateOrderItemDto> OrderItems { get; set; } = new List<CreateOrderItemDto>();
    }

    public class UpdateSaleDto
    {
        [Required(ErrorMessage = "El subtotal es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El subtotal debe ser mayor a 0")]
        public decimal SubTotal { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "La propina debe ser mayor o igual a 0")]
        public decimal TipAmount { get; set; }

        [Required(ErrorMessage = "El total es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El total debe ser mayor a 0")]
        public decimal Total { get; set; }

        [Required(ErrorMessage = "El efectivo recibido es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El efectivo recibido debe ser mayor a 0")]
        public decimal CashRecieved { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El cambio debe ser mayor o igual a 0")]
        public decimal CashChange { get; set; }

        public List<UpdateOrderItemDto> OrderItems { get; set; } = new List<UpdateOrderItemDto>();
    }
}