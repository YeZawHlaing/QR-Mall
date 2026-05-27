using Microsoft.AspNetCore.Mvc;
using ProductQrApi.Interfaces;
using ProductQrApi.Responses;
using ProductQrApi.Services;

namespace ProductQrApi.Controllers;

[ApiController]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly IProductService _productService;

    public AdminController(IProductService productService)
    {
        _productService = productService;
    }

    // GET: api/admin/products
    [HttpGet("products")]
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await _productService.GetAllAsync();

        return Ok(new ApiResponse<object>(
            true,
            "Admin - All products",
            products
        ));
    }

    // GET: api/admin/products/{id}
    [HttpGet("products/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _productService.GetByIdAsync(id);

        if (product == null)
        {
            return NotFound(new ApiResponse<object>(
                false,
                "Product not found",
                null
            ));
        }

        return Ok(new ApiResponse<object>(
            true,
            "Admin - product detail",
            product
        ));
    }

    [HttpDelete("products/{id}")]
public async Task<IActionResult> Delete(int id)
{
    await _productService.DeleteAsync(id);

    return Ok(new ApiResponse<object>(
        true,
        "Product deleted",
        null
    ));
}


[HttpGet("products/{id}/qr/pdf")]
public async Task<IActionResult> DownloadQrPdf(
    int id,
    [FromServices] PdfService pdfService)
{
    var product = await _productService.GetByIdAsync(id);

    if (product == null)
        return NotFound();

    var qrPath = Path.Combine(
        Directory.GetCurrentDirectory(),
        "wwwroot",
        product.QrUrl.TrimStart('/')
    );

    var pdf = pdfService.GenerateQrPdf(qrPath, product.Name);

    return File(pdf, "application/pdf", $"qr-{id}.pdf");
}

}