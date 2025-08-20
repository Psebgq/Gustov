using Gustov.Application.Mappers;
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

            var sale = SaleMapper.ToEntity(createSaleDto);
            var createdSale = await _saleRepository.Create(sale);

            if (createSaleDto.OrderItems?.Any() == true)
            {
                foreach (var orderItemDto in createSaleDto.OrderItems)
                {
                    var orderItem = OrderItemMapper.ToEntity(orderItemDto);
                    orderItem.SaleId = createdSale.Id;
                    await _orderItemRepository.Create(orderItem);
                }

                createdSale = await _saleRepository.FindOne(createdSale.Id) ?? createdSale;
            }

            var saleDto = SaleMapper.ToDto(createdSale);

            _logger.LogInformation("Venta creada exitosamente con ID: {SaleId}", createdSale.Id);

            return saleDto;
        }

        public async Task<SaleDto> Update(int id, UpdateSaleDto updateSaleDto)
        {
            _logger.LogInformation("Actualizando venta con ID: {SaleId}", id);

            if (id <= 0)
            {
                _logger.LogWarning("ID de venta inválido: {SaleId}", id);
                throw new ArgumentException("El ID debe ser mayor a 0", nameof(id));
            }

            var sale = SaleMapper.ToEntity(updateSaleDto);
            sale.Id = id;

            var updatedSale = await _saleRepository.Update(sale);

            // Actualizar los items de la orden si existen
            if (updateSaleDto.OrderItems?.Any() == true)
            {
                // Nota: Aquí podrías implementar lógica más compleja para
                // manejar la actualización, eliminación e inserción de items
                _logger.LogInformation("Actualizando {Count} items de la venta", updateSaleDto.OrderItems.Count);
            }

            var saleDto = SaleMapper.ToDto(updatedSale);

            _logger.LogInformation("Venta actualizada exitosamente con ID: {SaleId}", id);

            return saleDto;
        }
    }
}