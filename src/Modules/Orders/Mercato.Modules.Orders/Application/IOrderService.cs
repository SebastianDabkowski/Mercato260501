using Mercato.Shared;
using Mercato.Modules.Orders.Application.DTOs;
using Mercato.Modules.Orders.Domain;

namespace Mercato.Modules.Orders.Application;

public interface IOrderService
{
    Task<PagedResult<OrderDto>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<Result<OrderDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<OrderDto>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default);
    Task<Result<OrderDto>> CreateAsync(CreateOrderRequest request, CancellationToken cancellationToken = default);
    Task<Result<OrderDto>> UpdateStatusAsync(Guid id, OrderStatus status, CancellationToken cancellationToken = default);
    Task<Result> CancelAsync(Guid id, CancellationToken cancellationToken = default);
}
