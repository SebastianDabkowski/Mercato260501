using Mercato.Shared;
using Mercato.Modules.Catalog.Application.DTOs;
using Mercato.Modules.Catalog.Domain;

namespace Mercato.Modules.Catalog.Application;

public class ProductService : IProductService
{
    private readonly IRepository<Product> _productRepository;

    public ProductService(IRepository<Product> productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<PagedResult<ProductDto>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var (items, totalCount) = await _productRepository.GetPagedAsync(page, pageSize, cancellationToken);
        return new PagedResult<ProductDto>
        {
            Items = items.Select(MapToDto).ToList(),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<Result<ProductDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(id, cancellationToken);
        if (product is null)
            return Result<ProductDto>.Failure($"Product with id {id} not found.");
        return Result<ProductDto>.Success(MapToDto(product));
    }

    public async Task<Result<ProductDto>> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default)
    {
        var product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            StockQuantity = request.StockQuantity,
            CategoryId = request.CategoryId,
            ImageUrl = request.ImageUrl,
            IsActive = true
        };
        await _productRepository.AddAsync(product, cancellationToken);
        return Result<ProductDto>.Success(MapToDto(product));
    }

    public async Task<Result<ProductDto>> UpdateAsync(Guid id, UpdateProductRequest request, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(id, cancellationToken);
        if (product is null)
            return Result<ProductDto>.Failure($"Product with id {id} not found.");

        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        product.StockQuantity = request.StockQuantity;
        product.CategoryId = request.CategoryId;
        product.ImageUrl = request.ImageUrl;
        product.IsActive = request.IsActive;
        product.UpdatedAt = DateTime.UtcNow;

        await _productRepository.UpdateAsync(product, cancellationToken);
        return Result<ProductDto>.Success(MapToDto(product));
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(id, cancellationToken);
        if (product is null)
            return Result.Failure($"Product with id {id} not found.");
        await _productRepository.DeleteAsync(product, cancellationToken);
        return Result.Success();
    }

    private static ProductDto MapToDto(Product p) => new(
        p.Id, p.Name, p.Description, p.Price, p.StockQuantity,
        p.CategoryId, p.Category?.Name, p.ImageUrl, p.IsActive,
        p.CreatedAt, p.UpdatedAt);
}
