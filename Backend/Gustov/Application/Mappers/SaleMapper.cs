using Gustov.Domain.Entities;
using Gustov.Infrastructure.DTOs;

namespace Gustov.Application.Mappers
{
    public static class SaleMapper
    {
        public static SaleDto ToDto(Sale sale)
        {
            return new SaleDto
            {
                Id = sale.Id,
                SubTotal = sale.SubTotal,
                TipAmount = sale.TipAmount,
                Total = sale.Total,
                CashRecieved = sale.CashRecieved,
                CashChange = sale.CashChange,
                CreatedAt = sale.CreatedAt,
                OrderItems = sale.OrderItems?.Select(OrderItemMapper.ToDto).ToList() ?? new List<OrderItemDto>()
            };
        }

        public static List<SaleDto> ToDto(List<Sale> sales)
        {
            return sales.Select(ToDto).ToList();
        }

        public static Sale ToEntity(CreateSaleDto createSaleDto)
        {
            return new Sale
            {
                SubTotal = createSaleDto.SubTotal,
                TipAmount = createSaleDto.TipAmount,
                Total = createSaleDto.Total,
                CashRecieved = createSaleDto.CashRecieved,
                CashChange = createSaleDto.CashChange,
                OrderItems = createSaleDto.OrderItems?.Select(OrderItemMapper.ToEntity).ToList() ?? new List<OrderItem>()
            };
        }

        public static Sale ToEntity(UpdateSaleDto updateSaleDto)
        {
            return new Sale
            {
                SubTotal = updateSaleDto.SubTotal,
                TipAmount = updateSaleDto.TipAmount,
                Total = updateSaleDto.Total,
                CashRecieved = updateSaleDto.CashRecieved,
                CashChange = updateSaleDto.CashChange,
                OrderItems = updateSaleDto.OrderItems?.Select(OrderItemMapper.ToEntity).ToList() ?? new List<OrderItem>()
            };
        }
    }
}