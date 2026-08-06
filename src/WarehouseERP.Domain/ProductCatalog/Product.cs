using WarehouseERP.Domain.Exceptions;

namespace WarehouseERP.Domain.ProductCatalog;

public class Product
{
    public const int MaxSkuLength = 50;
    public const int MaxNameLength = 100;
    public const int MaxDescriptionLength = 500;

    public Guid Id { get; private set; }
    public string Sku { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public Guid CategoryId { get; private set; }
    public decimal UnitPrice { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private Product()
    {
    }

    private Product(Guid id, string sku, string name, string? description, Guid categoryId, decimal unitPrice)
    {
        Id = id;
        Sku = sku;
        Name = name;
        Description = description;
        CategoryId = categoryId;
        UnitPrice = unitPrice;
        IsActive = true;

        var now = DateTime.UtcNow;
        CreatedAt = now;
        UpdatedAt = now;
    }

    public static Product Create(string sku, string name, Guid categoryId, decimal unitPrice, string? description = null)
    {
        ValidateSku(sku);
        ValidateName(name);
        ValidateDescription(description);
        ValidateCategoryId(categoryId);
        ValidateUnitPrice(unitPrice);

        return new Product(Guid.NewGuid(), sku, name, description, categoryId, unitPrice);
    }

    public void Rename(string name)
    {
        ValidateName(name);
        Name = name;
        MarkUpdated();
    }

    public void ChangeSku(string sku)
    {
        ValidateSku(sku);
        Sku = sku;
        MarkUpdated();
    }

    public void ChangeDescription(string? description)
    {
        ValidateDescription(description);
        Description = description;
        MarkUpdated();
    }

    public void ChangePrice(decimal unitPrice)
    {
        ValidateUnitPrice(unitPrice);
        UnitPrice = unitPrice;
        MarkUpdated();
    }

    public void ChangeCategory(Guid categoryId)
    {
        ValidateCategoryId(categoryId);
        CategoryId = categoryId;
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

    private static void ValidateSku(string sku)
    {
        if (string.IsNullOrWhiteSpace(sku))
        {
            throw new DomainException("Product SKU is required.");
        }

        if (sku.Length > MaxSkuLength)
        {
            throw new DomainException($"Product SKU cannot exceed {MaxSkuLength} characters.");
        }
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Product name is required.");
        }

        if (name.Length > MaxNameLength)
        {
            throw new DomainException($"Product name cannot exceed {MaxNameLength} characters.");
        }
    }

    private static void ValidateDescription(string? description)
    {
        if (description is not null && description.Length > MaxDescriptionLength)
        {
            throw new DomainException($"Product description cannot exceed {MaxDescriptionLength} characters.");
        }
    }

    private static void ValidateCategoryId(Guid categoryId)
    {
        if (categoryId == Guid.Empty)
        {
            throw new DomainException("Product must be assigned to a valid category.");
        }
    }

    private static void ValidateUnitPrice(decimal unitPrice)
    {
        if (unitPrice < 0)
        {
            throw new DomainException("Product unit price cannot be negative.");
        }
    }
}
