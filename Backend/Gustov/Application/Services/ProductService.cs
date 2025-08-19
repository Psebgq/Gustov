using Gustov.Application.Mappers;
using Gustov.Domain.Interfaces.Repositories;
using Gustov.Infrastructure.DTOs;

namespace Gustov.Application.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductService> _logger;

        public ProductService(
            IProductRepository productRepository,
            ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task<List<ProductDto>> GetAllProductsAsync()
        {
            _logger.LogInformation("Obteniendo todos los productos");

            var products = await _productRepository.FindAll();
            var productsDto = ProductMapper.ToDto(products);

            _logger.LogInformation("Se encontraron {Count} productos", productsDto.Count);
            return productsDto;
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto)
        {
            _logger.LogInformation("Creando nuevo producto: {ProductName}", createProductDto.Name);

            var product = ProductMapper.ToEntity(createProductDto);
            var createdProduct = await _productRepository.Create(product);
            var productDto = ProductMapper.ToDto(createdProduct);

            _logger.LogInformation("Producto creado exitosamente con ID: {ProductId}", createdProduct.Id);

            return productDto;
        }

        public async Task<ProductDto> UpdateProductAsync(int id, UpdateProductDto updateProductDto)
        {
            _logger.LogInformation("Actualizando producto con ID: {ProductId}", id);

            if (id <= 0)
            {
                _logger.LogWarning("ID de producto inválido: {ProductId}", id);
                throw new ArgumentException("El ID debe ser mayor a 0", nameof(id));
            }

            var product = ProductMapper.ToEntity(updateProductDto);
            product.Id = id;

            var updatedProduct = await _productRepository.Update(product);
            var productDto = ProductMapper.ToDto(updatedProduct);

            _logger.LogInformation("Producto actualizado exitosamente con ID: {ProductId}", id);

            return productDto;
        }
    }
}