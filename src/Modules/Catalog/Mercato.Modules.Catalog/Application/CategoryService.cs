using Mercato.Shared;
using Mercato.Modules.Catalog.Application.DTOs;
using Mercato.Modules.Catalog.Domain;

namespace Mercato.Modules.Catalog.Application;

public class CategoryService : ICategoryService
{
    private readonly IRepository<Category> _categoryRepository;

    public CategoryService(IRepository<Category> categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IReadOnlyList<CategoryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var categories = await _categoryRepository.GetAllAsync(cancellationToken);
        return categories.Select(MapToDto).ToList();
    }

    public async Task<Result<CategoryDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var category = await _categoryRepository.GetByIdAsync(id, cancellationToken);
        if (category is null)
            return Result<CategoryDto>.Failure($"Category with id {id} not found.");
        return Result<CategoryDto>.Success(MapToDto(category));
    }

    public async Task<Result<CategoryDto>> CreateAsync(CreateCategoryRequest request, CancellationToken cancellationToken = default)
    {
        var category = new Category { Name = request.Name, Description = request.Description };
        await _categoryRepository.AddAsync(category, cancellationToken);
        return Result<CategoryDto>.Success(MapToDto(category));
    }

    public async Task<Result<CategoryDto>> UpdateAsync(Guid id, UpdateCategoryRequest request, CancellationToken cancellationToken = default)
    {
        var category = await _categoryRepository.GetByIdAsync(id, cancellationToken);
        if (category is null)
            return Result<CategoryDto>.Failure($"Category with id {id} not found.");

        category.Name = request.Name;
        category.Description = request.Description;
        category.UpdatedAt = DateTime.UtcNow;

        await _categoryRepository.UpdateAsync(category, cancellationToken);
        return Result<CategoryDto>.Success(MapToDto(category));
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var category = await _categoryRepository.GetByIdAsync(id, cancellationToken);
        if (category is null)
            return Result.Failure($"Category with id {id} not found.");
        await _categoryRepository.DeleteAsync(category, cancellationToken);
        return Result.Success();
    }

    private static CategoryDto MapToDto(Category c) => new(c.Id, c.Name, c.Description, c.CreatedAt, c.UpdatedAt);
}
