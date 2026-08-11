using System.ComponentModel.DataAnnotations;

namespace WarehouseERP.Blazor.Features.Inventory.Components;

public sealed class IssueStockFormModel
{
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
    public int Quantity { get; set; }

    public string? Reference { get; set; }
}
