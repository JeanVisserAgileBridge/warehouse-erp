using System.ComponentModel.DataAnnotations;

namespace WarehouseERP.Blazor.Features.Inventory.Components;

public sealed class AdjustStockFormModel
{
    [Range(0, int.MaxValue, ErrorMessage = "Quantity on hand cannot be negative.")]
    public int NewQuantityOnHand { get; set; }

    public string? Reference { get; set; }
}
