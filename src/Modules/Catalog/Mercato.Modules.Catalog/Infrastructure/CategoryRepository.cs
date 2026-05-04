using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Mercato.Shared;
using Mercato.Modules.Catalog.Domain;

namespace Mercato.Modules.Catalog.Infrastructure;

public class CategoryRepository : IRepository<Category>
{
    private readonly CatalogDbContext _context;

    public CategoryRepository(CatalogDbContext context)
    {
        _context = context;
    }

    public async Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Categories.FindAsync(new object[] { id }, cancellationToken);

    public async Task<IReadOnlyList<Category>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.Categories.ToListAsync(cancellationToken);

    public async Task<(IReadOnlyList<Category> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var totalCount = await _context.Categories.CountAsync(cancellationToken);
        var items = await _context.Categories.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
        return (items, totalCount);
    }

    public async Task<Category> AddAsync(Category entity, CancellationToken cancellationToken = default)
    {
        _context.Categories.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(Category entity, CancellationToken cancellationToken = default)
    {
        _context.Categories.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Category entity, CancellationToken cancellationToken = default)
    {
        _context.Categories.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Category>> FindAsync(Expression<Func<Category, bool>> predicate, CancellationToken cancellationToken = default)
        => await _context.Categories.Where(predicate).ToListAsync(cancellationToken);
}
