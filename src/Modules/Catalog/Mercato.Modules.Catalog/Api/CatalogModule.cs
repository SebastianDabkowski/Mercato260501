using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mercato.Shared;
using Mercato.Modules.Catalog.Application;
using Mercato.Modules.Catalog.Application.DTOs;
using Mercato.Modules.Catalog.Domain;
using Mercato.Modules.Catalog.Infrastructure;

namespace Mercato.Modules.Catalog.Api;

public class CatalogModule : IModule
{
    public void Register(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CatalogDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("CatalogDb")));

        services.AddScoped<IRepository<Product>, ProductRepository>();
        services.AddScoped<IRepository<Category>, CategoryRepository>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICategoryService, CategoryService>();
    }

    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/catalog").WithTags("Catalog");

        group.MapGet("/products", async (IProductService svc, int page = 1, int pageSize = 10) =>
            Results.Ok(await svc.GetAllAsync(page, pageSize)));

        group.MapGet("/products/{id:guid}", async (Guid id, IProductService svc) =>
        {
            var result = await svc.GetByIdAsync(id);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Error);
        });

        group.MapPost("/products", async (CreateProductRequest request, IProductService svc) =>
        {
            var result = await svc.CreateAsync(request);
            return result.IsSuccess
                ? Results.Created($"/api/catalog/products/{result.Value!.Id}", result.Value)
                : Results.BadRequest(result.Error);
        });

        group.MapPut("/products/{id:guid}", async (Guid id, UpdateProductRequest request, IProductService svc) =>
        {
            var result = await svc.UpdateAsync(id, request);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Error);
        });

        group.MapDelete("/products/{id:guid}", async (Guid id, IProductService svc) =>
        {
            var result = await svc.DeleteAsync(id);
            return result.IsSuccess ? Results.NoContent() : Results.NotFound(result.Error);
        });

        group.MapGet("/categories", async (ICategoryService svc) =>
            Results.Ok(await svc.GetAllAsync()));

        group.MapPost("/categories", async (CreateCategoryRequest request, ICategoryService svc) =>
        {
            var result = await svc.CreateAsync(request);
            return result.IsSuccess
                ? Results.Created($"/api/catalog/categories/{result.Value!.Id}", result.Value)
                : Results.BadRequest(result.Error);
        });
    }
}
