using AutoMapper;
using ProductQrApi.DTOs;
using ProductQrApi.Entities;
using ProductQrApi.Interfaces;

namespace ProductQrApi.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    private readonly IMapper _mapper;

    public ProductService(
        IProductRepository repository,
        IMapper mapper
    )
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ProductResponseDto> CreateAsync(
        CreateProductDto dto
    )
    {
        string imageUrl = "";

        if (dto.Image != null)
        {
            var fileName = Guid.NewGuid() +
                           Path.GetExtension(dto.Image.FileName);

            var path = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot/uploads",
                fileName
            );

            using var stream = new FileStream(path, FileMode.Create);

            await dto.Image.CopyToAsync(stream);

            imageUrl = "/uploads/" + fileName;
        }

        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            CategoryId = dto.CategoryId,
            ImageUrl = imageUrl
        };

        var created = await _repository.CreateAsync(product);

        return _mapper.Map<ProductResponseDto>(created);
    }

    public async Task<List<ProductResponseDto>> GetAllAsync()
    {
        var products = await _repository.GetAllAsync();

        return _mapper.Map<List<ProductResponseDto>>(products);
    }

    public async Task<ProductResponseDto?> GetByIdAsync(int id)
    {
        var product = await _repository.GetByIdAsync(id);

        if (product == null)
        {
            return null;
        }

        return _mapper.Map<ProductResponseDto>(product);
    }
}