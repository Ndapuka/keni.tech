using BusinessLogicLayer.Entities;
using BusinessLogicLayer.RepositoryContracts;
using DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ProductsDbContext _context;

    public ProductRepository(ProductsDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
    }

    public void Delete(Product product)
    {
        _context.Products.Remove(product);
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Images)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Product>> GetFeaturedProductsAsync()
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Images)
            .Where(p => p.IsFeatured && p.IsAvailable)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(Guid categoryId)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Images)
            .Where(p => p.CategoryId == categoryId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> SearchProductsAsync(string keyword)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Images)
            .Where(p =>
                p.Name.Contains(keyword) ||
                p.Description.Contains(keyword))
            .AsNoTracking()
            .ToListAsync();
    }

    public void Update(Product product)
    {
        _context.Products.Update(product);
    }
}