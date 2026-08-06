using System.Text.RegularExpressions;
using WarehouseERP.Domain.Exceptions;

namespace WarehouseERP.Domain.Sales;

public class Customer
{
    public const int MaxNameLength = 100;
    public const int MaxPhoneNumberLength = 30;
    public const int MaxAddressLength = 500;

    private static readonly Regex EmailPattern = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Email { get; private set; }
    public string? PhoneNumber { get; private set; }
    public string? Address { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private Customer()
    {
    }

    private Customer(Guid id, string name, string? email, string? phoneNumber, string? address)
    {
        Id = id;
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
        Address = address;
        IsActive = true;

        var now = DateTime.UtcNow;
        CreatedAt = now;
        UpdatedAt = now;
    }

    public static Customer Create(string name, string? email = null, string? phoneNumber = null, string? address = null)
    {
        ValidateName(name);
        ValidateEmail(email);
        ValidatePhoneNumber(phoneNumber);
        ValidateAddress(address);

        return new Customer(Guid.NewGuid(), name, email, phoneNumber, address);
    }

    public void Rename(string name)
    {
        ValidateName(name);
        Name = name;
        MarkUpdated();
    }

    public void ChangeEmail(string? email)
    {
        ValidateEmail(email);
        Email = email;
        MarkUpdated();
    }

    public void ChangePhoneNumber(string? phoneNumber)
    {
        ValidatePhoneNumber(phoneNumber);
        PhoneNumber = phoneNumber;
        MarkUpdated();
    }

    public void ChangeAddress(string? address)
    {
        ValidateAddress(address);
        Address = address;
        MarkUpdated();
    }

    public void Activate()
    {
        IsActive = true;
        MarkUpdated();
    }

    public void Deactivate()
    {
        IsActive = false;
        MarkUpdated();
    }

    private void MarkUpdated()
    {
        UpdatedAt = DateTime.UtcNow;
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Customer name is required.");
        }

        if (name.Length > MaxNameLength)
        {
            throw new DomainException($"Customer name cannot exceed {MaxNameLength} characters.");
        }
    }

    private static void ValidateEmail(string? email)
    {
        if (email is not null && !EmailPattern.IsMatch(email))
        {
            throw new DomainException("Customer email is not a valid email address.");
        }
    }

    private static void ValidatePhoneNumber(string? phoneNumber)
    {
        if (phoneNumber is not null && phoneNumber.Length > MaxPhoneNumberLength)
        {
            throw new DomainException($"Customer phone number cannot exceed {MaxPhoneNumberLength} characters.");
        }
    }

    private static void ValidateAddress(string? address)
    {
        if (address is not null && address.Length > MaxAddressLength)
        {
            throw new DomainException($"Customer address cannot exceed {MaxAddressLength} characters.");
        }
    }
}
