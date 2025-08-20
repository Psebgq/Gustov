using Gustov.Application.Services;
using Gustov.Infrastructure.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Gustov.Controllers
{
    [ApiController]
    [Route("api/product")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(
            ProductService productService,
            ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> FindAll()
        {
            try
            {
                var products = await _productService.FindAll();
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener productos");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> FindByCategory(int categoryId)
        {
            try
            {
                var products = await _productService.FindByCategory(categoryId);
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener productos");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> FindOne(int productId)
        {
            try
            {
                var product = await _productService.FindOne(productId);
                return Ok(product);
            }
            catch
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateProductDto createProductDto, IFormFile? image)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Manejar la imagen si se proporciona
                if (image != null)
                {
                    var imageValidation = ValidateImageFile(image);
                    if (!imageValidation.IsValid)
                        return BadRequest(imageValidation.Error);

                    var imagePath = await SaveImageAsync(image);
                    createProductDto.Image = imagePath;
                }

                var product = await _productService.Create(createProductDto);
                return Ok(product);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear producto");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateProductDto updateProductDto, IFormFile? image)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Obtener el producto actual para comparar la imagen
                var currentProduct = await _productService.FindOne(id);
                if (currentProduct == null)
                    return NotFound("Producto no encontrado");

                string oldImagePath = currentProduct.Image;

                // Manejar la imagen si se proporciona una nueva
                if (image != null)
                {
                    var imageValidation = ValidateImageFile(image);
                    if (!imageValidation.IsValid)
                        return BadRequest(imageValidation.Error);

                    var newImagePath = await SaveImageAsync(image);
                    updateProductDto.Image = newImagePath;

                    // Eliminar la imagen anterior si es diferente y no es la imagen por defecto
                    if (!string.IsNullOrEmpty(oldImagePath) &&
                        oldImagePath != newImagePath &&
                        oldImagePath != "assets/images/default.jpg")
                    {
                        DeleteImageFile(oldImagePath);
                    }
                }
                else
                {
                    // Si no se proporciona imagen nueva, mantener la actual
                    updateProductDto.Image = oldImagePath;
                }

                var product = await _productService.Update(id, updateProductDto);
                return Ok(product);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar producto {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        private (bool IsValid, string? Error) ValidateImageFile(IFormFile file)
        {
            const int maxSizeInBytes = 5 * 1024 * 1024; // 5MB
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var allowedMimeTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif", "image/webp" };

            if (!allowedMimeTypes.Contains(file.ContentType.ToLower()))
            {
                return (false, "Tipo de archivo no válido. Solo se permiten: JPG, PNG, GIF, WebP");
            }

            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExtension))
            {
                return (false, "Extensión de archivo no válida. Solo se permiten: .jpg, .jpeg, .png, .gif, .webp");
            }

            if (file.Length > maxSizeInBytes)
            {
                return (false, "El archivo es demasiado grande. Máximo 5MB");
            }

            if (file.Length == 0)
            {
                return (false, "El archivo está vacío");
            }

            return (true, null);
        }

        private async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            try
            {
                // Crear el directorio si no existe
                var uploadsFolder = Path.Combine("assets", "images");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Generar nombre único para la imagen
                var fileExtension = Path.GetExtension(imageFile.FileName);
                var fileName = $"{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                // Guardar el archivo
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                // Retornar la ruta relativa para guardar en la base de datos
                return $"assets/images/{fileName}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar la imagen");
                throw new Exception("Error al guardar la imagen");
            }
        }

        private void DeleteImageFile(string imagePath)
        {
            try
            {
                if (string.IsNullOrEmpty(imagePath) || imagePath == "assets/images/default.jpg")
                    return;

                var fullPath = Path.Combine(imagePath);

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                    _logger.LogInformation("Imagen eliminada: {ImagePath}", imagePath);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error al eliminar la imagen: {ImagePath}", imagePath);
                // No lanzamos excepción para no interrumpir el flujo principal
            }
        }
    }
}