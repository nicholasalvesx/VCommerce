using Microsoft.EntityFrameworkCore;
using VCommerce.Api.Data;
using VCommerce.Api.Models;

namespace VCommerce.Api.Repositores;

public class CategoryRepository  : ICategoryRepository
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Category>> GetCategories()
    {
        return await _context.Categories.ToListAsync();
    }

    public async Task<IEnumerable<Category>> GetCatgoriesProducts()
    {
        return await _context.Categories.Include(c => c.Products)
            .ToListAsync();
    }

    public async Task<Category?> GetCategoryById(int id)
    {
        return await _context.Categories.Where(c => c.CategoryId == id).FirstOrDefaultAsync();
    }

    public async Task<Category> CreateCategory(Category category)
    {
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task<Category> UpdateCategory(Category category)
    {
        _context.Entry(category).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task<Category> DeleteCategory(int id)
    {
        var category = await GetCategoryById(id);
        _context.Categories.Remove(category!);
        await _context.SaveChangesAsync();
        return category!;
    }
}