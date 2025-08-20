using Gustov.Application.Services;
using Gustov.Infrastructure.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Gustov.Controllers
{
    [ApiController]
    [Route("api/orderitem")]
    public class OrderItemController : ControllerBase
    {
        private readonly OrderItemService _orderItemService;
        private readonly ILogger<OrderItemController> _logger;

        public OrderItemController(OrderItemService orderItemService, ILogger<OrderItemController> logger)
        {
            _orderItemService = orderItemService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> FindAll()
        {
            try
            {
                var orderItems = await _orderItemService.FindAll();
                return Ok(orderItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener items de órdenes");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("{orderItemId}")]
        public async Task<IActionResult> FindOne(int orderItemId)
        {
            try
            {
                var orderItem = await _orderItemService.FindOne(orderItemId);
                if (orderItem == null)
                    return NotFound("Item de orden no encontrado");

                return Ok(orderItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener item de orden {OrderItemId}", orderItemId);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("sale/{saleId}")]
        public async Task<IActionResult> FindBySaleId(int saleId)
        {
            try
            {
                var orderItems = await _orderItemService.FindBySaleId(saleId);
                return Ok(orderItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener items de orden para venta {SaleId}", saleId);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderItemDto createOrderItemDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var orderItem = await _orderItemService.Create(createOrderItemDto);
                return Ok(orderItem);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear item de orden");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateOrderItemDto updateOrderItemDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var orderItem = await _orderItemService.Update(id, updateOrderItemDto);
                return Ok(orderItem);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar item de orden {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}