using Microsoft.EntityFrameworkCore;
using VCommerce.Api.Data;
using VCommerce.Api.Models;

namespace VCommerce.Api.Repositores;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetProducts()
    {
        return await _context.Products.Include(c => c.CategoryName).ToListAsync();
    }
    
    public async Task<Product?> GetProductById(int id)
    {
        return await _context.Products.Include(c => c.CategoryName)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Product> CreateProduct(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<Product> UpdateProduct(Product product)
    {
        _context.Entry(product).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<Product?> DeleteProduct(int id)
    {
        var product = await GetProductById(id);
        _context.Products.Remove(product!);
        await _context.SaveChangesAsync();
        return product!;
    }
}