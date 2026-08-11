using System.ComponentModel.DataAnnotations;

namespace WarehouseERP.Blazor.Features.Inventory.Components;

public sealed class ReorderLevelFormModel
{
    [Range(0, int.MaxValue, ErrorMessage = "Reorder level cannot be negative.")]
    public int ReorderLevel { get; set; }
}
