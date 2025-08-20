using Gustov.Application.Mappers;
using Gustov.Domain.Interfaces.Repositories;
using Gustov.Infrastructure.DTOs;

namespace Gustov.Application.Services
{
    public class OrderItemService
    {
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly ILogger<OrderItemService> _logger;

        public OrderItemService(
            IOrderItemRepository orderItemRepository,
            ILogger<OrderItemService> logger)
        {
            _orderItemRepository = orderItemRepository;
            _logger = logger;
        }

        public async Task<List<OrderItemDto>> FindAll()
        {
            _logger.LogInformation("Obteniendo todos los items de órdenes");

            var orderItems = await _orderItemRepository.FindAll();
            var orderItemsDto = OrderItemMapper.ToDto(orderItems);

            _logger.LogInformation("Se encontraron {Count} items de órdenes", orderItemsDto.Count);
            return orderItemsDto;
        }

        public async Task<OrderItemDto?> FindOne(int orderItemId)
        {
            _logger.LogInformation("Obteniendo item de orden con ID: {OrderItemId}", orderItemId);

            var orderItem = await _orderItemRepository.FindOne(orderItemId);
            if (orderItem == null)
            {
                _logger.LogWarning("Item de orden no encontrado con ID: {OrderItemId}", orderItemId);
                return null;
            }

            var orderItemDto = OrderItemMapper.ToDto(orderItem);
            return orderItemDto;
        }

        public async Task<List<OrderItemDto>> FindBySaleId(int saleId)
        {
            _logger.LogInformation("Obteniendo items de orden para venta ID: {SaleId}", saleId);

            var orderItems = await _orderItemRepository.FindBySaleId(saleId);
            var orderItemsDto = OrderItemMapper.ToDto(orderItems);

            _logger.LogInformation("Se encontraron {Count} items para la venta {SaleId}", orderItemsDto.Count, saleId);
            return orderItemsDto;
        }

        public async Task<OrderItemDto> Create(CreateOrderItemDto createOrderItemDto)
        {
            _logger.LogInformation("Creando nuevo item de orden: {ItemName} para venta {SaleId}",
                createOrderItemDto.Name, createOrderItemDto.SaleId);

            var orderItem = OrderItemMapper.ToEntity(createOrderItemDto);
            var createdOrderItem = await _orderItemRepository.Create(orderItem);
            var orderItemDto = OrderItemMapper.ToDto(createdOrderItem);

            _logger.LogInformation("Item de orden creado exitosamente con ID: {OrderItemId}", createdOrderItem.Id);

            return orderItemDto;
        }

        public async Task<OrderItemDto> Update(int id, UpdateOrderItemDto updateOrderItemDto)
        {
            _logger.LogInformation("Actualizando item de orden con ID: {OrderItemId}", id);

            if (id <= 0)
            {
                _logger.LogWarning("ID de item de orden inválido: {OrderItemId}", id);
                throw new ArgumentException("El ID debe ser mayor a 0", nameof(id));
            }

            var orderItem = OrderItemMapper.ToEntity(updateOrderItemDto);
            orderItem.Id = id;

            var updatedOrderItem = await _orderItemRepository.Update(orderItem);
            var orderItemDto = OrderItemMapper.ToDto(updatedOrderItem);

            _logger.LogInformation("Item de orden actualizado exitosamente con ID: {OrderItemId}", id);

            return orderItemDto;
        }
    }
}