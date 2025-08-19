using Gustov.Application.Mappers;
using Gustov.Domain.Interfaces.Repositories;
using Gustov.Infrastructure.DTOs;

namespace Gustov.Application.Services
{
    public class CategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(
            ICategoryRepository categoryRepository,
            ILogger<CategoryService> logger)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
        }

        public async Task<List<CategoryDto>> GetAllCategoriesAsync()
        {
            _logger.LogInformation("Obteniendo todas las categorías");

            var categories = await _categoryRepository.FindAll();
            var categoriesDto = CategoryMapper.ToDto(categories);

            _logger.LogInformation("Se encontraron {Count} categorías", categoriesDto.Count);
            return categoriesDto;
        }

        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createCategoryDto)
        {
            _logger.LogInformation("Creando nueva categoría: {CategoryName}", createCategoryDto.Name);

            var category = CategoryMapper.ToEntity(createCategoryDto);
            var createdCategory = await _categoryRepository.Create(category);
            var categoryDto = CategoryMapper.ToDto(createdCategory);

            _logger.LogInformation("Categoría creada exitosamente con ID: {CategoryId}", createdCategory.Id);

            return categoryDto;
        }

        public async Task<CategoryDto> UpdateCategoryAsync(int id, UpdateCategoryDto updateCategoryDto)
        {
            _logger.LogInformation("Actualizando categoría con ID: {CategoryId}", id);

            if (id <= 0)
            {
                _logger.LogWarning("ID de categoría inválido: {CategoryId}", id);
                throw new ArgumentException("El ID debe ser mayor a 0", nameof(id));
            }

            var category = CategoryMapper.ToEntity(updateCategoryDto);
            category.Id = id;

            var updatedCategory = await _categoryRepository.Update(category);
            var categoryDto = CategoryMapper.ToDto(updatedCategory);

            _logger.LogInformation("Categoría actualizada exitosamente con ID: {CategoryId}", id);

            return categoryDto;
        }
    }
}