using Gustov.Domain.Entities;
using Gustov.Infrastructure.DTOs;

namespace Gustov.Application.Mappers
{
    public static class OrderItemMapper
    {
        public static OrderItemDto ToDto(OrderItem orderItem)
        {
            return new OrderItemDto
            {
                Id = orderItem.Id,
                SaleId = orderItem.SaleId,
                CategoryId = orderItem.CategoryId,
                Name = orderItem.Name,
                Quantity = orderItem.Quantity,
                UnitPrice = orderItem.UnitPrice,
                TotalPrice = orderItem.TotalPrice,
                IsActive = orderItem.IsActive,
                CreatedAt = orderItem.CreatedAt,
                UpdatedAt = orderItem.UpdatedAt,
                CategoryName = orderItem.Category?.Name
            };
        }

        public static List<OrderItemDto> ToDto(List<OrderItem> orderItems)
        {
            return orderItems.Select(ToDto).ToList();
        }

        public static OrderItem ToEntity(CreateOrderItemDto createOrderItemDto)
        {
            return new OrderItem
            {
                SaleId = createOrderItemDto.SaleId,
                CategoryId = createOrderItemDto.CategoryId,
                Name = createOrderItemDto.Name,
                Quantity = createOrderItemDto.Quantity,
                UnitPrice = createOrderItemDto.UnitPrice,
                TotalPrice = createOrderItemDto.TotalPrice,
                IsActive = createOrderItemDto.IsActive
            };
        }

        public static OrderItem ToEntity(UpdateOrderItemDto updateOrderItemDto)
        {
            return new OrderItem
            {
                SaleId = updateOrderItemDto.SaleId,
                CategoryId = updateOrderItemDto.CategoryId,
                Name = updateOrderItemDto.Name,
                Quantity = updateOrderItemDto.Quantity,
                UnitPrice = updateOrderItemDto.UnitPrice,
                TotalPrice = updateOrderItemDto.TotalPrice,
                IsActive = updateOrderItemDto.IsActive
            };
        }
    }
}