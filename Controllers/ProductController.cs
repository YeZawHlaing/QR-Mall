using Microsoft.AspNetCore.Mvc;
using ProductQrApi.DTOs;
using ProductQrApi.Interfaces;
using ProductQrApi.Responses;

namespace ProductQrApi.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private readonly IProductService _service;

    public ProductController(IProductService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromForm] CreateProductDto dto
    )
    {
        var product = await _service.CreateAsync(dto);

        return Ok(
            new ApiResponse<object>(
                true,
                "Product created successfully",
                product
            )
        );
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _service.GetAllAsync();

        return Ok(
            new ApiResponse<object>(
                true,
                "Products fetched successfully",
                products
            )
        );
    }

    [HttpGet("/p/{code}")]
public async Task<IActionResult> GetPublicProduct(string code)
{
    var product = await _service.GetByCodeAsync(code);

    if (product == null)
    {
        return NotFound();
    }

    return Ok(product);
}

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _service.GetByIdAsync(id);

        if (product == null)
        {
            return NotFound(
                new ApiResponse<object>(
                    false,
                    "Product not found",
                    null
                )
            );
        }

        return Ok(
            new ApiResponse<object>(
                true,
                "Product fetched successfully",
                product
            )
        );
    }
}