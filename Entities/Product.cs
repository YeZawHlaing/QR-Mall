namespace ProductQrApi.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public string ImageUrl { get; set; } = string.Empty;

    public string QrUrl { get; set; } = string.Empty; // NEW

    public int CategoryId { get; set; }

    public Category? Category { get; set; }

    public string PublicCode { get; set; } = string.Empty;
}