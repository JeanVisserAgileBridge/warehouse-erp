using WarehouseERP.Domain.Exceptions;

namespace WarehouseERP.Domain.ProductCatalog;

public class Category
{
    public const int MaxNameLength = 100;
    public const int MaxDescriptionLength = 500;

    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public bool IsActive { get; private set; }

    private Category()
    {
    }

    private Category(Guid id, string name, string? description)
    {
        Id = id;
        Name = name;
        Description = description;
        IsActive = true;
    }

    public static Category Create(string name, string? description = null)
    {
        ValidateName(name);
        ValidateDescription(description);

        return new Category(Guid.NewGuid(), name, description);
    }

    public void Rename(string name)
    {
        ValidateName(name);
        Name = name;
    }

    public void UpdateDescription(string? description)
    {
        ValidateDescription(description);
        Description = description;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Category name is required.");
        }

        if (name.Length > MaxNameLength)
        {
            throw new DomainException($"Category name cannot exceed {MaxNameLength} characters.");
        }
    }

    private static void ValidateDescription(string? description)
    {
        if (description is not null && description.Length > MaxDescriptionLength)
        {
            throw new DomainException($"Category description cannot exceed {MaxDescriptionLength} characters.");
        }
    }
}
