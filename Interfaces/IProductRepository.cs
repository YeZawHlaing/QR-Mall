using ProductQrApi.Entities;

namespace ProductQrApi.Interfaces;

public interface IProductRepository
{
    Task<Product> CreateAsync(Product product);

    Task<List<Product>> GetAllAsync();

    Task<Product?> GetByIdAsync(int id);

    Task<Product> UpdateAsync(Product product);

    Task<Product?> GetByCodeAsync(string code);
}