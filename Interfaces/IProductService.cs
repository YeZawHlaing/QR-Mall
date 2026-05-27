using ProductQrApi.DTOs;

namespace ProductQrApi.Interfaces;

public interface IProductService
{
    Task<ProductResponseDto> CreateAsync(
        CreateProductDto dto
    );

    Task<List<ProductResponseDto>> GetAllAsync();

    Task<ProductResponseDto?> GetByIdAsync(int id);
}