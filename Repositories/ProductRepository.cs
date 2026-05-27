using Microsoft.EntityFrameworkCore;
using ProductQrApi.Data;
using ProductQrApi.Entities;
using ProductQrApi.Interfaces;

namespace ProductQrApi.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Product> CreateAsync(Product product)
    {
        _context.Products.Add(product);

        await _context.SaveChangesAsync();

        return product;
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _context.Products
            .Include(x => x.Category)
            .ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products
            .Include(x => x.Category)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}