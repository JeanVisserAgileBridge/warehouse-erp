using WarehouseERP.Domain.Exceptions;

namespace WarehouseERP.Domain.Warehouses;

public class Warehouse
{
    public const int MaxCodeLength = 20;
    public const int MaxNameLength = 100;
    public const int MaxAddressLength = 500;

    public Guid Id { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string? Address { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private Warehouse()
    {
    }

    private Warehouse(Guid id, string code, string name, string? address)
    {
        Id = id;
        Code = code;
        Name = name;
        Address = address;
        IsActive = true;

        var now = DateTime.UtcNow;
        CreatedAt = now;
        UpdatedAt = now;
    }

    public static Warehouse Create(string code, string name, string? address = null)
    {
        ValidateCode(code);
        ValidateName(name);
        ValidateAddress(address);

        return new Warehouse(Guid.NewGuid(), code, name, address);
    }

    public void Rename(string name)
    {
        ValidateName(name);
        Name = name;
        MarkUpdated();
    }

    public void ChangeCode(string code)
    {
        ValidateCode(code);
        Code = code;
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

    private static void ValidateCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new DomainException("Warehouse code is required.");
        }

        if (code.Length > MaxCodeLength)
        {
            throw new DomainException($"Warehouse code cannot exceed {MaxCodeLength} characters.");
        }
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Warehouse name is required.");
        }

        if (name.Length > MaxNameLength)
        {
            throw new DomainException($"Warehouse name cannot exceed {MaxNameLength} characters.");
        }
    }

    private static void ValidateAddress(string? address)
    {
        if (address is not null && address.Length > MaxAddressLength)
        {
            throw new DomainException($"Warehouse address cannot exceed {MaxAddressLength} characters.");
        }
    }
}
