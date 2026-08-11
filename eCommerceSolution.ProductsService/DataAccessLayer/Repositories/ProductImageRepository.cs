using BusinessLogicLayer.Entities;
using BusinessLogicLayer.RepositoryContracts;
using DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories;

public class ProductImageRepository : IProductImageRepository
{
    private readonly ProductsDbContext _context;

    public ProductImageRepository(ProductsDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ProductImage image)
    {
        await _context.ProductImages.AddAsync(image);
    }

    public void Delete(ProductImage image)
    {
        _context.ProductImages.Remove(image);
    }

    public async Task<IEnumerable<ProductImage>> GetImagesByProductIdAsync(Guid productId)
    {
        return await _context.ProductImages
            .Where(i => i.ProductId == productId)
            .OrderBy(i => i.DisplayOrder)
            .AsNoTracking()
            .ToListAsync();
    }
    public async Task<ProductImage?> GetByIdAsync(Guid imageId)
    {
        return await _context.ProductImages
            .FirstOrDefaultAsync(i => i.Id == imageId);
    }
}