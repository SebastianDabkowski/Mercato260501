using Mercato.Shared;

namespace Mercato.Modules.Customers.Domain;

public class Customer : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public Address Address { get; set; } = new();
}
