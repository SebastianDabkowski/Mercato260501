using Microsoft.EntityFrameworkCore;
using Mercato.Modules.Customers.Domain;

namespace Mercato.Modules.Customers.Infrastructure;

public class CustomersDbContext : DbContext
{
    public CustomersDbContext(DbContextOptions<CustomersDbContext> options) : base(options) { }

    public DbSet<Customer> Customers => Set<Customer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.OwnsOne(e => e.Address, addr =>
            {
                addr.Property(a => a.Street).HasMaxLength(200);
                addr.Property(a => a.City).HasMaxLength(100);
                addr.Property(a => a.PostalCode).HasMaxLength(20);
                addr.Property(a => a.Country).HasMaxLength(100);
            });
        });
    }
}
