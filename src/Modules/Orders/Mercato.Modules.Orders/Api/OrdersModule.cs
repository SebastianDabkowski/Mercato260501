using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mercato.Shared;
using Mercato.Modules.Orders.Application;
using Mercato.Modules.Orders.Application.DTOs;
using Mercato.Modules.Orders.Domain;
using Mercato.Modules.Orders.Infrastructure;

namespace Mercato.Modules.Orders.Api;

public class OrdersModule : IModule
{
    public void Register(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OrdersDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("OrdersDb")));

        services.AddScoped<IRepository<Order>, OrderRepository>();
        services.AddScoped<IOrderService, OrderService>();
    }

    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/orders").WithTags("Orders");

        group.MapGet("/", async (IOrderService svc, int page = 1, int pageSize = 10) =>
            Results.Ok(await svc.GetAllAsync(page, pageSize)));

        group.MapGet("/{id:guid}", async (Guid id, IOrderService svc) =>
        {
            var result = await svc.GetByIdAsync(id);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Error);
        });

        group.MapGet("/customer/{customerId:guid}", async (Guid customerId, IOrderService svc) =>
            Results.Ok(await svc.GetByCustomerIdAsync(customerId)));

        group.MapPost("/", async (CreateOrderRequest request, IOrderService svc) =>
        {
            var result = await svc.CreateAsync(request);
            return result.IsSuccess
                ? Results.Created($"/api/orders/{result.Value!.Id}", result.Value)
                : Results.BadRequest(result.Error);
        });

        group.MapPut("/{id:guid}/status", async (Guid id, UpdateOrderStatusRequest request, IOrderService svc) =>
        {
            var result = await svc.UpdateStatusAsync(id, request.Status);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Error);
        });

        group.MapPost("/{id:guid}/cancel", async (Guid id, IOrderService svc) =>
        {
            var result = await svc.CancelAsync(id);
            return result.IsSuccess ? Results.NoContent() : Results.BadRequest(result.Error);
        });
    }
}

public record UpdateOrderStatusRequest(OrderStatus Status);
