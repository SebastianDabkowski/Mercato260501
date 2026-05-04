namespace Mercato.Modules.Catalog.Application.DTOs;

public record ProductDto(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    int StockQuantity,
    Guid CategoryId,
    string? CategoryName,
    string? ImageUrl,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record CreateProductRequest(
    string Name,
    string Description,
    decimal Price,
    int StockQuantity,
    Guid CategoryId,
    string? ImageUrl
);

public record UpdateProductRequest(
    string Name,
    string Description,
    decimal Price,
    int StockQuantity,
    Guid CategoryId,
    string? ImageUrl,
    bool IsActive
);
