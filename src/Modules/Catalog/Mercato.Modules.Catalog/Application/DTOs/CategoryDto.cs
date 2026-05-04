namespace Mercato.Modules.Catalog.Application.DTOs;

public record CategoryDto(
    Guid Id,
    string Name,
    string Description,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record CreateCategoryRequest(string Name, string Description);
public record UpdateCategoryRequest(string Name, string Description);
