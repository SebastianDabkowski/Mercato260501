using Mercato.Shared;

namespace Mercato.Modules.Catalog.Domain;

public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; } = true;
}
