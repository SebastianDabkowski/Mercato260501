using Mercato.Modules.Orders.Domain;

namespace Mercato.Modules.Orders.Application.DTOs;

public record OrderItemDto(
    Guid Id,
    Guid ProductId,
    string ProductName,
    decimal UnitPrice,
    int Quantity,
    decimal TotalPrice
);

public record OrderDto(
    Guid Id,
    Guid CustomerId,
    OrderStatus Status,
    IReadOnlyList<OrderItemDto> OrderItems,
    decimal TotalAmount,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record CreateOrderItemRequest(
    Guid ProductId,
    string ProductName,
    decimal UnitPrice,
    int Quantity
);

public record CreateOrderRequest(
    Guid CustomerId,
    IReadOnlyList<CreateOrderItemRequest> Items
);
