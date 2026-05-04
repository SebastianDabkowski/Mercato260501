using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mercato.Shared;
using Mercato.Modules.Customers.Application;
using Mercato.Modules.Customers.Application.DTOs;
using Mercato.Modules.Customers.Domain;
using Mercato.Modules.Customers.Infrastructure;

namespace Mercato.Modules.Customers.Api;

public class CustomersModule : IModule
{
    public void Register(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CustomersDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("CustomersDb")));

        services.AddScoped<IRepository<Customer>, CustomerRepository>();
        services.AddScoped<ICustomerService, CustomerService>();
    }

    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/customers").WithTags("Customers");

        group.MapGet("/", async (ICustomerService svc, int page = 1, int pageSize = 10) =>
            Results.Ok(await svc.GetAllAsync(page, pageSize)));

        group.MapGet("/{id:guid}", async (Guid id, ICustomerService svc) =>
        {
            var result = await svc.GetByIdAsync(id);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Error);
        });

        group.MapPost("/", async (CreateCustomerRequest request, ICustomerService svc) =>
        {
            var result = await svc.CreateAsync(request);
            return result.IsSuccess
                ? Results.Created($"/api/customers/{result.Value!.Id}", result.Value)
                : Results.BadRequest(result.Error);
        });

        group.MapPut("/{id:guid}", async (Guid id, UpdateCustomerRequest request, ICustomerService svc) =>
        {
            var result = await svc.UpdateAsync(id, request);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Error);
        });

        group.MapDelete("/{id:guid}", async (Guid id, ICustomerService svc) =>
        {
            var result = await svc.DeleteAsync(id);
            return result.IsSuccess ? Results.NoContent() : Results.NotFound(result.Error);
        });
    }
}
