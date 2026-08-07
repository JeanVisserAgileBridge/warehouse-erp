using System.ComponentModel.DataAnnotations;

namespace WarehouseERP.Blazor.Features.Products.Components;

public sealed class ProductFormModel
{
    [Required(ErrorMessage = "SKU is required.")]
    public string Sku { get; set; } = string.Empty;

    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Required(ErrorMessage = "Category is required.")]
    public Guid? CategoryId { get; set; }

    public decimal UnitPrice { get; set; }
}
