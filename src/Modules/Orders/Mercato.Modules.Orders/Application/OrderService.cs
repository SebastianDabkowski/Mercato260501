using Mercato.Shared;
using Mercato.Modules.Orders.Application.DTOs;
using Mercato.Modules.Orders.Domain;

namespace Mercato.Modules.Orders.Application;

public class OrderService : IOrderService
{
    private readonly IRepository<Order> _orderRepository;

    public OrderService(IRepository<Order> orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<PagedResult<OrderDto>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var (items, totalCount) = await _orderRepository.GetPagedAsync(page, pageSize, cancellationToken);
        return new PagedResult<OrderDto>
        {
            Items = items.Select(MapToDto).ToList(),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<Result<OrderDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(id, cancellationToken);
        if (order is null)
            return Result<OrderDto>.Failure($"Order with id {id} not found.");
        return Result<OrderDto>.Success(MapToDto(order));
    }

    public async Task<IReadOnlyList<OrderDto>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        var orders = await _orderRepository.FindAsync(o => o.CustomerId == customerId, cancellationToken);
        return orders.Select(MapToDto).ToList();
    }

    public async Task<Result<OrderDto>> CreateAsync(CreateOrderRequest request, CancellationToken cancellationToken = default)
    {
        var order = new Order
        {
            CustomerId = request.CustomerId,
            Status = OrderStatus.Pending,
            OrderItems = request.Items.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                UnitPrice = i.UnitPrice,
                Quantity = i.Quantity
            }).ToList()
        };
        order.TotalAmount = order.OrderItems.Sum(i => i.UnitPrice * i.Quantity);
        await _orderRepository.AddAsync(order, cancellationToken);
        return Result<OrderDto>.Success(MapToDto(order));
    }

    public async Task<Result<OrderDto>> UpdateStatusAsync(Guid id, OrderStatus status, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(id, cancellationToken);
        if (order is null)
            return Result<OrderDto>.Failure($"Order with id {id} not found.");

        order.Status = status;
        order.UpdatedAt = DateTime.UtcNow;
        await _orderRepository.UpdateAsync(order, cancellationToken);
        return Result<OrderDto>.Success(MapToDto(order));
    }

    public async Task<Result> CancelAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(id, cancellationToken);
        if (order is null)
            return Result.Failure($"Order with id {id} not found.");

        if (order.Status == OrderStatus.Delivered)
            return Result.Failure("Cannot cancel a delivered order.");

        order.Status = OrderStatus.Cancelled;
        order.UpdatedAt = DateTime.UtcNow;
        await _orderRepository.UpdateAsync(order, cancellationToken);
        return Result.Success();
    }

    private static OrderDto MapToDto(Order o) => new(
        o.Id, o.CustomerId, o.Status,
        o.OrderItems.Select(i => new OrderItemDto(i.Id, i.ProductId, i.ProductName, i.UnitPrice, i.Quantity, i.UnitPrice * i.Quantity)).ToList(),
        o.TotalAmount, o.CreatedAt, o.UpdatedAt);
}
