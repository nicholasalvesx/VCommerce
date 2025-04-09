using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VCommerce.Api.Models;

namespace VCommerce.Api.Data;

public class AppDbContext : IdentityDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
        // modelBuilder.Entity<Category>()
            // .HasKey(c => c.CategoryId);

        // modelBuilder.Entity<Category>()
            // .Property(c => c.Name)
            // .HasMaxLength(100)
            // .IsRequired();

        // modelBuilder.Entity<Product>()
            // .HasKey(p => p.Id);
        
        // modelBuilder.Entity<Product>().Property(p => p.Name)
            // .HasMaxLength(100)
            // .IsRequired();

        // modelBuilder.Entity<Product>()
            // .Property(p => p.Description)
            // .HasMaxLength(100)
            // .IsRequired();

        // modelBuilder.Entity<Product>()
            // .Property(p => p.ImageUrl)
            // .HasMaxLength(100)
            // .IsRequired();

        // modelBuilder.Entity<Product>()
            // .Property(p => p.Price)
            // .HasColumnType("decimal(10,2)")
            // .IsRequired();

        // modelBuilder.Entity<Category>()
            // .HasMany(c => c.Products)
            // .WithOne(p => p.Category)
            // .IsRequired()
            // .OnDelete(DeleteBehavior.Cascade);

        // modelBuilder.Entity<Category>().HasData(
            // new Category
            // {
                // CategoryId = 1,
                // Name = "Roupas"
            // },
            // new Category
            // {
                // CategoryId = 2,
                // Name = "Acessorios"
            // }
        // );
        
        // modelBuilder.Entity<Product>().HasData(
            // new Product{
                // Id = 1,
                // Name = "Blusa",
                // Price = 100,
                // ImageUrl = "blusa.jpg",
                // Stock = 5,
                // CategoryId = 1,
                // Description = "Blusa"
            // },
            
            // new Product{
                // Id = 2,
                // Name = "Short",
                // Price = 50,
                // ImageUrl = "short.jpg",
                // Stock = 8,
                // CategoryId = 1,
                // Description = "Short"
            // },
            
            // new Product{
                // Id = 3,
                // Name = "Cordao",
                // Price = 10,
                // ImageUrl = "cordao.jpg",
                // Stock = 1,
                // CategoryId = 2,
                // Description = "Cordao de ouro"
            // }
            // );
    // }
}