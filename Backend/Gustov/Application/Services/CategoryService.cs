using AutoMapper;
using Gustov.Domain.Entities;
using Gustov.Domain.Interfaces.Repositories;
using Gustov.Infrastructure.DTOs;

namespace Gustov.Application.Services
{
    public class CategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(
            ICategoryRepository categoryRepository,
            IMapper mapper,
            ILogger<CategoryService> logger)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<CategoryDto>> GetAllCategoriesAsync()
        {
            _logger.LogInformation("Obteniendo todas las categorías");

            var categories = await _categoryRepository.FindAll();
            var categoriesDto = _mapper.Map<List<CategoryDto>>(categories);

            _logger.LogInformation("Se encontraron {Count} categorías", categoriesDto.Count);
            return categoriesDto;
        }

        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createCategoryDto)
        {
            _logger.LogInformation("Creando nueva categoría: {CategoryName}", createCategoryDto.Name);

            var category = _mapper.Map<Category>(createCategoryDto);
            var createdCategory = await _categoryRepository.Create(category);
            var categoryDto = _mapper.Map<CategoryDto>(createdCategory);

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

            var category = _mapper.Map<Category>(updateCategoryDto);
            category.Id = id;

            var updatedCategory = await _categoryRepository.Update(category);
            var categoryDto = _mapper.Map<CategoryDto>(updatedCategory);

            _logger.LogInformation("Categoría actualizada exitosamente con ID: {CategoryId}", id);

            return categoryDto;
        }
    }
}