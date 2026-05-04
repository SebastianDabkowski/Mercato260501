using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Mercato.Shared;
using Mercato.Modules.Orders.Domain;

namespace Mercato.Modules.Orders.Infrastructure;

public class OrderRepository : IRepository<Order>
{
    private readonly OrdersDbContext _context;

    public OrderRepository(OrdersDbContext context)
    {
        _context = context;
    }

    public async Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Order>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.Orders.Include(o => o.OrderItems).ToListAsync(cancellationToken);

    public async Task<(IReadOnlyList<Order> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.Orders.Include(o => o.OrderItems);
        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
        return (items, totalCount);
    }

    public async Task<Order> AddAsync(Order entity, CancellationToken cancellationToken = default)
    {
        _context.Orders.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(Order entity, CancellationToken cancellationToken = default)
    {
        _context.Orders.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Order entity, CancellationToken cancellationToken = default)
    {
        _context.Orders.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Order>> FindAsync(Expression<Func<Order, bool>> predicate, CancellationToken cancellationToken = default)
        => await _context.Orders.Include(o => o.OrderItems).Where(predicate).ToListAsync(cancellationToken);
}
