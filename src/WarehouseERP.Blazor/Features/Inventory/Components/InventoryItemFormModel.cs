using System.ComponentModel.DataAnnotations;

namespace WarehouseERP.Blazor.Features.Inventory.Components;

public sealed class InventoryItemFormModel
{
    [Required(ErrorMessage = "Product is required.")]
    public Guid? ProductId { get; set; }

    [Required(ErrorMessage = "Storage location is required.")]
    public Guid? StorageLocationId { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Quantity on hand cannot be negative.")]
    public int QuantityOnHand { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Reorder level cannot be negative.")]
    public int ReorderLevel { get; set; }
}
