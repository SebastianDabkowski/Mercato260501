using Moq;
using Mercato.Shared;
using Mercato.Modules.Catalog.Application;
using Mercato.Modules.Catalog.Application.DTOs;
using Mercato.Modules.Catalog.Domain;

namespace Mercato.Modules.Catalog.Tests;

public class ProductServiceTests
{
    private readonly Mock<IRepository<Product>> _repoMock = new();
    private readonly IProductService _sut;

    public ProductServiceTests()
    {
        _sut = new ProductService(_repoMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsProduct_WhenExists()
    {
        var id = Guid.NewGuid();
        var product = new Product { Id = id, Name = "Test Shirt", Price = 19.99m, StockQuantity = 10 };
        _repoMock.Setup(r => r.GetByIdAsync(id, default)).ReturnsAsync(product);

        var result = await _sut.GetByIdAsync(id);

        Assert.True(result.IsSuccess);
        Assert.Equal(id, result.Value!.Id);
        Assert.Equal("Test Shirt", result.Value.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsFailure_WhenNotFound()
    {
        var id = Guid.NewGuid();
        _repoMock.Setup(r => r.GetByIdAsync(id, default)).ReturnsAsync((Product?)null);

        var result = await _sut.GetByIdAsync(id);

        Assert.False(result.IsSuccess);
        Assert.Contains(id.ToString(), result.Error);
    }

    [Fact]
    public async Task CreateAsync_ReturnsCreatedProduct()
    {
        var request = new CreateProductRequest("Blue Shirt", "A nice shirt", 29.99m, 100, Guid.NewGuid(), null);
        _repoMock.Setup(r => r.AddAsync(It.IsAny<Product>(), default)).ReturnsAsync((Product p, CancellationToken _) => p);

        var result = await _sut.CreateAsync(request);

        Assert.True(result.IsSuccess);
        Assert.Equal("Blue Shirt", result.Value!.Name);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFailure_WhenProductNotFound()
    {
        var id = Guid.NewGuid();
        _repoMock.Setup(r => r.GetByIdAsync(id, default)).ReturnsAsync((Product?)null);

        var result = await _sut.DeleteAsync(id);

        Assert.False(result.IsSuccess);
    }
}
