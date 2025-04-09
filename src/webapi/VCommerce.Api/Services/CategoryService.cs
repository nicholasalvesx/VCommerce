using AutoMapper;
using VCommerce.Api.DTOs;
using VCommerce.Api.Models;
using VCommerce.Api.Repositores;

namespace VCommerce.Api.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryService(IMapper mapper, ICategoryRepository categoryRepository)
    {
        _mapper = mapper;
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<CategoryDTO>> GetCategories()
    {
        var categoriesEntity = await _categoryRepository.GetCategories();
        return _mapper.Map<IEnumerable<CategoryDTO>>(categoriesEntity);
    }

    public async Task<IEnumerable<CategoryDTO>> GetCategoriesProduct()
    {
        var categoriesEntity = await _categoryRepository.GetCatgoriesProducts();
        return _mapper.Map<IEnumerable<CategoryDTO>>(categoriesEntity);
    }

    public async Task<CategoryDTO> GetCategoryById(int id)
    {
        var categoriesEntity = await _categoryRepository.GetCategoryById(id);
        return _mapper.Map<CategoryDTO>(categoriesEntity);
    }

    public async Task AddCategory(CategoryDTO category)
    {
        var categoryEntity = _mapper.Map<Category>(category);
        await _categoryRepository.CreateCategory(categoryEntity);
        category.CategoryId = categoryEntity.CategoryId;
    }

    public async Task UpdateCategory(CategoryDTO category)
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