namespace Mercato.Modules.Customers.Application.DTOs;

public record AddressDto(string Street, string City, string PostalCode, string Country);

public record CustomerDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string? PhoneNumber,
    AddressDto Address,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record CreateCustomerRequest(
    string FirstName,
    string LastName,
    string Email,
    string? PhoneNumber,
    AddressDto Address
);

public record UpdateCustomerRequest(
    string FirstName,
    string LastName,
    string Email,
    string? PhoneNumber,
    AddressDto Address
);
