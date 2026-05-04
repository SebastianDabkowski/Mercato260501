using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Mercato.Shared;
using Mercato.Modules.Customers.Domain;

namespace Mercato.Modules.Customers.Infrastructure;

public class CustomerRepository : IRepository<Customer>
{
    private readonly CustomersDbContext _context;

    public CustomerRepository(CustomersDbContext context)
    {
        _context = context;
    }

    public async Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Customers.FindAsync(new object[] { id }, cancellationToken);

    public async Task<IReadOnlyList<Customer>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.Customers.ToListAsync(cancellationToken);

    public async Task<(IReadOnlyList<Customer> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var totalCount = await _context.Customers.CountAsync(cancellationToken);
        var items = await _context.Customers.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
        return (items, totalCount);
    }

    public async Task<Customer> AddAsync(Customer entity, CancellationToken cancellationToken = default)
    {
        _context.Customers.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(Customer entity, CancellationToken cancellationToken = default)
    {
        _context.Customers.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Customer entity, CancellationToken cancellationToken = default)
    {
        _context.Customers.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Customer>> FindAsync(Expression<Func<Customer, bool>> predicate, CancellationToken cancellationToken = default)
        => await _context.Customers.Where(predicate).ToListAsync(cancellationToken);
}
