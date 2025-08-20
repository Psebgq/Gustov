using Gustov.Application.Mappers;
using Gustov.Domain.Entities;
using Gustov.Domain.Interfaces.Repositories;
using Gustov.Infrastructure.DTOs;

namespace Gustov.Application.Services
{
    public class SaleService
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly ILogger<SaleService> _logger;

        public SaleService(
            ISaleRepository saleRepository,
            IOrderItemRepository orderItemRepository,
            ILogger<SaleService> logger)
        {
            _saleRepository = saleRepository;
            _orderItemRepository = orderItemRepository;
            _logger = logger;
        }

        public async Task<List<SaleDto>> FindAll()
        {
            _logger.LogInformation("Obteniendo todas las ventas");

            var sales = await _saleRepository.FindAll();
            var salesDto = SaleMapper.ToDto(sales);

            _logger.LogInformation("Se encontraron {Count} ventas", salesDto.Count);
            return salesDto;
        }

        public async Task<SaleDto?> FindOne(int saleId)
        {
            _logger.LogInformation("Obteniendo venta con ID: {SaleId}", saleId);

            var sale = await _saleRepository.FindOne(saleId);
            if (sale == null)
            {
                _logger.LogWarning("Venta no encontrada con ID: {SaleId}", saleId);
                return null;
            }

            var saleDto = SaleMapper.ToDto(sale);
            return saleDto;
        }

        public async Task<SaleDto> Create(CreateSaleDto createSaleDto)
        {
            _logger.LogInformation("Creando nueva venta con total: {Total}", createSaleDto.Total);

            var sale = new Sale
            {
                SubTotal = createSaleDto.SubTotal,
                TipAmount = createSaleDto.TipAmount,
                Total = createSaleDto.Total,
                CashRecieved = createSaleDto.CashRecieved,
                CashChange = createSaleDto.CashChange
            };

            var createdSale = await _saleRepository.Create(sale);
            List<OrderItem> orderItems = [];
            if (createSaleDto.OrderItems?.Any() == true)
            {
                _logger.LogInformation("Creando {Count} items para la venta {SaleId}",
                    createSaleDto.OrderItems.Count, createdSale.Id);

                foreach (var orderItemDto in createSaleDto.OrderItems)
                {
                    var orderItem = OrderItemMapper.ToEntity(orderItemDto);
                    orderItem.SaleId = createdSale.Id;
                    orderItem.CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
                    orderItem.UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
                    orderItems.Add(orderItem);
                }

                createdSale = await _saleRepository.FindOne(createdSale.Id) ?? createdSale;
            }
            await _orderItemRepository.CreateRange(orderItems);
            var saleDto = SaleMapper.ToDto(createdSale);

            _logger.LogInformation("Venta creada exitosamente con ID: {SaleId} y {ItemCount} items",
                createdSale.Id, createdSale.OrderItems?.Count ?? 0);

            return saleDto;
        }
    }
}