using Moq;
using Mercato.Shared;
using Mercato.Modules.Customers.Application;
using Mercato.Modules.Customers.Application.DTOs;
using Mercato.Modules.Customers.Domain;

namespace Mercato.Modules.Customers.Tests;

public class CustomerServiceTests
{
    private readonly Mock<IRepository<Customer>> _repoMock = new();
    private readonly ICustomerService _sut;

    public CustomerServiceTests()
    {
        _sut = new CustomerService(_repoMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsCustomer_WhenExists()
    {
        var id = Guid.NewGuid();
        var customer = new Customer
        {
            Id = id, FirstName = "John", LastName = "Doe",
            Email = "john@example.com", Address = new Address()
        };
        _repoMock.Setup(r => r.GetByIdAsync(id, default)).ReturnsAsync(customer);

        var result = await _sut.GetByIdAsync(id);

        Assert.True(result.IsSuccess);
        Assert.Equal("John", result.Value!.FirstName);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsFailure_WhenNotFound()
    {
        var id = Guid.NewGuid();
        _repoMock.Setup(r => r.GetByIdAsync(id, default)).ReturnsAsync((Customer?)null);

        var result = await _sut.GetByIdAsync(id);

        Assert.False(result.IsSuccess);
        Assert.Contains(id.ToString(), result.Error);
    }

    [Fact]
    public async Task CreateAsync_ReturnsCreatedCustomer()
    {
        var request = new CreateCustomerRequest("Jane", "Smith", "jane@example.com", null,
            new AddressDto("123 Main St", "Springfield", "12345", "US"));
        _repoMock.Setup(r => r.AddAsync(It.IsAny<Customer>(), default)).ReturnsAsync((Customer c, CancellationToken _) => c);

        var result = await _sut.CreateAsync(request);

        Assert.True(result.IsSuccess);
        Assert.Equal("Jane", result.Value!.FirstName);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFailure_WhenNotFound()
    {
        var id = Guid.NewGuid();
        _repoMock.Setup(r => r.GetByIdAsync(id, default)).ReturnsAsync((Customer?)null);

        var result = await _sut.DeleteAsync(id);

        Assert.False(result.IsSuccess);
    }
}
