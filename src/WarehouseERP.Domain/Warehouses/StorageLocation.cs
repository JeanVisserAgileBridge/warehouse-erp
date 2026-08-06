using WarehouseERP.Domain.Exceptions;

namespace WarehouseERP.Domain.Warehouses;

public class StorageLocation
{
    public const int MaxCodeLength = 30;
    public const int MaxDescriptionLength = 500;

    public Guid Id { get; private set; }
    public Guid WarehouseId { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private StorageLocation()
    {
    }

    private StorageLocation(Guid id, Guid warehouseId, string code, string? description)
    {
        Id = id;
        WarehouseId = warehouseId;
        Code = code;
        Description = description;
        IsActive = true;

        var now = DateTime.UtcNow;
        CreatedAt = now;
        UpdatedAt = now;
    }

    public static StorageLocation Create(Guid warehouseId, string code, string? description = null)
    {
        ValidateWarehouseId(warehouseId);
        ValidateCode(code);
        ValidateDescription(description);

        return new StorageLocation(Guid.NewGuid(), warehouseId, code, description);
    }

    public void ChangeCode(string code)
    {
        ValidateCode(code);
        Code = code;
        MarkUpdated();
    }

    public void ChangeDescription(string? description)
    {
        ValidateDescription(description);
        Description = description;
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

    private static void ValidateWarehouseId(Guid warehouseId)
    {
        if (warehouseId == Guid.Empty)
        {
            throw new DomainException("Storage location must be assigned to a valid warehouse.");
        }
    }

    private static void ValidateCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new DomainException("Storage location code is required.");
        }

        if (code.Length > MaxCodeLength)
        {
            throw new DomainException($"Storage location code cannot exceed {MaxCodeLength} characters.");
        }
    }

    private static void ValidateDescription(string? description)
    {
        if (description is not null && description.Length > MaxDescriptionLength)
        {
            throw new DomainException($"Storage location description cannot exceed {MaxDescriptionLength} characters.");
        }
    }
}
