using AutoMapper;
using VCommerce.Modules.Core.Application.DTOs;
using VCommerce.Modules.Core.Application.Interfaces;
using VCommerce.Modules.Core.Domain.Entities;
using VCommerce.Modules.Core.Infra.Repositories;

namespace VCommerce.Modules.Core.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryService(IMapper mapper, ICategoryRepository categoryRepository)
    {
        _mapper = mapper;
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<CategoryDto>> GetCategories()
    {
        var categoriesEntity = await _categoryRepository.GetCategories();
        return _mapper.Map<IEnumerable<CategoryDto>>(categoriesEntity);
    }

    public async Task<IEnumerable<CategoryDto>> GetCategoriesProduct()
    {
        var categoriesEntity = await _categoryRepository.GetCatgoriesProducts();
        return _mapper.Map<IEnumerable<CategoryDto>>(categoriesEntity);
    }

    public async Task<CategoryDto> GetCategoryById(int id)
    {
        var categoriesEntity = await _categoryRepository.GetCategoryById(id);
        return _mapper.Map<CategoryDto>(categoriesEntity);
    }

    public async Task AddCategory(CategoryDto category)
    {
        var categoryEntity = _mapper.Map<Category>(category);
        await _categoryRepository.CreateCategory(categoryEntity);
        category.CategoryId = categoryEntity.CategoryId;
    }

    public async Task UpdateCategory(CategoryDto category)
    {
        var categoryEntity = _mapper.Map<Category>(category);
        await _categoryRepository.UpdateCategory(categoryEntity);
    }

    public async Task DeleteCategory(int id)
    {
        var category = _categoryRepository.GetCategoryById(id).Result;
        await _categoryRepository.DeleteCategory(category!.CategoryId);
    }
}