using Mercato.Shared;
using Mercato.Modules.Customers.Application.DTOs;
using Mercato.Modules.Customers.Domain;

namespace Mercato.Modules.Customers.Application;

public class CustomerService : ICustomerService
{
    private readonly IRepository<Customer> _customerRepository;

    public CustomerService(IRepository<Customer> customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<PagedResult<CustomerDto>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var (items, totalCount) = await _customerRepository.GetPagedAsync(page, pageSize, cancellationToken);
        return new PagedResult<CustomerDto>
        {
            Items = items.Select(MapToDto).ToList(),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<Result<CustomerDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var customer = await _customerRepository.GetByIdAsync(id, cancellationToken);
        if (customer is null)
            return Result<CustomerDto>.Failure($"Customer with id {id} not found.");
        return Result<CustomerDto>.Success(MapToDto(customer));
    }

    public async Task<Result<CustomerDto>> CreateAsync(CreateCustomerRequest request, CancellationToken cancellationToken = default)
    {
        var customer = new Customer
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Address = new Address
            {
                Street = request.Address.Street,
                City = request.Address.City,
                PostalCode = request.Address.PostalCode,
                Country = request.Address.Country
            }
        };
        await _customerRepository.AddAsync(customer, cancellationToken);
        return Result<CustomerDto>.Success(MapToDto(customer));
    }

    public async Task<Result<CustomerDto>> UpdateAsync(Guid id, UpdateCustomerRequest request, CancellationToken cancellationToken = default)
    {
        var customer = await _customerRepository.GetByIdAsync(id, cancellationToken);
        if (customer is null)
            return Result<CustomerDto>.Failure($"Customer with id {id} not found.");

        customer.FirstName = request.FirstName;
        customer.LastName = request.LastName;
        customer.Email = request.Email;
        customer.PhoneNumber = request.PhoneNumber;
        customer.Address.Street = request.Address.Street;
        customer.Address.City = request.Address.City;
        customer.Address.PostalCode = request.Address.PostalCode;
        customer.Address.Country = request.Address.Country;
        customer.UpdatedAt = DateTime.UtcNow;

        await _customerRepository.UpdateAsync(customer, cancellationToken);
        return Result<CustomerDto>.Success(MapToDto(customer));
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var customer = await _customerRepository.GetByIdAsync(id, cancellationToken);
        if (customer is null)
            return Result.Failure($"Customer with id {id} not found.");
        await _customerRepository.DeleteAsync(customer, cancellationToken);
        return Result.Success();
    }

    private static CustomerDto MapToDto(Customer c) => new(
        c.Id, c.FirstName, c.LastName, c.Email, c.PhoneNumber,
        new AddressDto(c.Address.Street, c.Address.City, c.Address.PostalCode, c.Address.Country),
        c.CreatedAt, c.UpdatedAt);
}
