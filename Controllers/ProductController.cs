using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductQrApi.Data;
using ProductQrApi.DTOs;
using ProductQrApi.Entities;

namespace ProductQrApi.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromForm] CreateProductDto dto
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

        _context.Products.Add(product);

        await _context.SaveChangesAsync();

        return Ok(product);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _context.Products
            .Include(x => x.Category)
            .ToListAsync();

        return Ok(products);
    }
}