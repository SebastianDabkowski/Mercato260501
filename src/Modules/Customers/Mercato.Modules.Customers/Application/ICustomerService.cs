using Mercato.Shared;
using Mercato.Modules.Customers.Application.DTOs;

namespace Mercato.Modules.Customers.Application;

public interface ICustomerService
{
    Task<PagedResult<CustomerDto>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<Result<CustomerDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<CustomerDto>> CreateAsync(CreateCustomerRequest request, CancellationToken cancellationToken = default);
    Task<Result<CustomerDto>> UpdateAsync(Guid id, UpdateCustomerRequest request, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
