using Gustov.Application.Services;
using Gustov.Infrastructure.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Gustov.Controllers
{
    [ApiController]
    [Route("api/sale")]
    public class SaleController : ControllerBase
    {
        private readonly SaleService _saleService;
        private readonly ILogger<SaleController> _logger;

        public SaleController(SaleService saleService, ILogger<SaleController> logger)
        {
            _saleService = saleService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> FindAll()
        {
            try
            {
                var sales = await _saleService.FindAll();
                return Ok(sales);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener ventas");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("{saleId}")]
        public async Task<IActionResult> FindOne(int saleId)
        {
            try
            {
                var sale = await _saleService.FindOne(saleId);
                if (sale == null)
                    return NotFound("Venta no encontrada");

                return Ok(sale);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener venta {SaleId}", saleId);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSaleDto createSaleDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var sale = await _saleService.Create(createSaleDto);
                return Ok(sale);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear venta");
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}