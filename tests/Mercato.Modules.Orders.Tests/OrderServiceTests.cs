using Moq;
using Mercato.Shared;
using Mercato.Modules.Orders.Application;
using Mercato.Modules.Orders.Application.DTOs;
using Mercato.Modules.Orders.Domain;

namespace Mercato.Modules.Orders.Tests;

public class OrderServiceTests
{
    private readonly Mock<IRepository<Order>> _repoMock = new();
    private readonly IOrderService _sut;

    public OrderServiceTests()
    {
        _sut = new OrderService(_repoMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsOrder_WhenExists()
    {
        var id = Guid.NewGuid();
        var order = new Order { Id = id, CustomerId = Guid.NewGuid(), Status = OrderStatus.Pending, OrderItems = new List<OrderItem>() };
        _repoMock.Setup(r => r.GetByIdAsync(id, default)).ReturnsAsync(order);

        var result = await _sut.GetByIdAsync(id);

        Assert.True(result.IsSuccess);
        Assert.Equal(id, result.Value!.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsFailure_WhenNotFound()
    {
        var id = Guid.NewGuid();
        _repoMock.Setup(r => r.GetByIdAsync(id, default)).ReturnsAsync((Order?)null);

        var result = await _sut.GetByIdAsync(id);

        Assert.False(result.IsSuccess);
        Assert.Contains(id.ToString(), result.Error);
    }

    [Fact]
    public async Task CreateAsync_ReturnsCreatedOrder()
    {
        var customerId = Guid.NewGuid();
        var request = new CreateOrderRequest(customerId, new List<CreateOrderItemRequest>
        {
            new(Guid.NewGuid(), "Blue Shirt", 19.99m, 2)
        });
        _repoMock.Setup(r => r.AddAsync(It.IsAny<Order>(), default)).ReturnsAsync((Order o, CancellationToken _) => o);

        var result = await _sut.CreateAsync(request);

        Assert.True(result.IsSuccess);
        Assert.Equal(customerId, result.Value!.CustomerId);
        Assert.Equal(OrderStatus.Pending, result.Value.Status);
    }

    [Fact]
    public async Task CancelAsync_ReturnsFailure_WhenOrderNotFound()
    {
        var id = Guid.NewGuid();
        _repoMock.Setup(r => r.GetByIdAsync(id, default)).ReturnsAsync((Order?)null);

        var result = await _sut.CancelAsync(id);

        Assert.False(result.IsSuccess);
    }
}
