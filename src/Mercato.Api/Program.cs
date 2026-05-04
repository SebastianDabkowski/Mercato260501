using Mercato.Shared;
using Mercato.Modules.Catalog.Api;
using Mercato.Modules.Orders.Api;
using Mercato.Modules.Customers.Api;
using Microsoft.EntityFrameworkCore;
using Mercato.Modules.Catalog.Infrastructure;
using Mercato.Modules.Orders.Infrastructure;
using Mercato.Modules.Customers.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Mercato API", Version = "v1" });
});

var modules = new List<IModule>
{
    new CatalogModule(),
    new OrdersModule(),
    new CustomersModule()
};

foreach (var module in modules)
    module.Register(builder.Services, builder.Configuration);

var app = builder.Build();

// Auto-migrate databases
using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<CatalogDbContext>().Database.EnsureCreated();
    scope.ServiceProvider.GetRequiredService<OrdersDbContext>().Database.EnsureCreated();
    scope.ServiceProvider.GetRequiredService<CustomersDbContext>().Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

foreach (var module in modules)
    module.MapEndpoints(app);

app.Run();

