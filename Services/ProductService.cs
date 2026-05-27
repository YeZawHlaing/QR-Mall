using AutoMapper;
using ProductQrApi.DTOs;
using ProductQrApi.Entities;
using ProductQrApi.Interfaces;
using ProductQrApi.Helpers;

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

    // public async Task<ProductResponseDto> CreateAsync(
    //     CreateProductDto dto
    // )
    // {
    //     string imageUrl = "";

    //     if (dto.Image != null)
    //     {
    //         var fileName = Guid.NewGuid() +
    //                        Path.GetExtension(dto.Image.FileName);

    //         var path = Path.Combine(
    //             Directory.GetCurrentDirectory(),
    //             "wwwroot/uploads",
    //             fileName
    //         );

    //         using var stream = new FileStream(path, FileMode.Create);

    //         await dto.Image.CopyToAsync(stream);

    //         imageUrl = "/uploads/" + fileName;
    //     }

    //     var product = new Product
    //     {
    //         Name = dto.Name,
    //         Description = dto.Description,
    //         Price = dto.Price,
    //         CategoryId = dto.CategoryId,
    //         ImageUrl = imageUrl
    //     };

    //     var created = await _repository.CreateAsync(product);

    //     return _mapper.Map<ProductResponseDto>(created);
    // }

    public async Task<ProductResponseDto> CreateAsync(
    CreateProductDto dto
)
{
    string imageUrl = "";
    var publicCode = Guid.NewGuid().ToString("N")[..8];


    // 1. Save product image
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

    // 2. Create product (SAVE TO DB FIRST)
    var product = new Product
    {
        Name = dto.Name,
        Description = dto.Description,
        Price = dto.Price,
        CategoryId = dto.CategoryId,
        ImageUrl = imageUrl,
        PublicCode = publicCode

    };

    var created = await _repository.CreateAsync(product);

    // ================================
    // 3. QR CODE PART (ADD HERE)
    // ================================

    string qrContent =
    $"http://localhost:3000/p/{created.PublicCode}";

    string qrFileName = $"qr_{created.Id}.png";

    string qrPath = Path.Combine(
        Directory.GetCurrentDirectory(),
        "wwwroot/qrcodes",
        qrFileName
    );

    QrCodeGeneratorHelper.GenerateQrCode(qrContent, qrPath);

    // 4. Save QR URL into product
    created.QrUrl = "/qrcodes/" + qrFileName;

    await _repository.UpdateAsync(created);

    // 5. Return response
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

    public async Task DeleteAsync(int id)
{
    await _repository.DeleteAsync(id);
}


    public async Task<ProductResponseDto?> GetByCodeAsync(string code)
{
    var product = await _repository.GetByCodeAsync(code);

    if (product == null)
        return null;

    return _mapper.Map<ProductResponseDto>(product);
}

private string GeneratePublicCode()
{
    return Guid.NewGuid().ToString("N")[..8];
}

}