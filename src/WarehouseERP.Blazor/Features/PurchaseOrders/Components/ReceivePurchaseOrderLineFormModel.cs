using System.ComponentModel.DataAnnotations;

namespace WarehouseERP.Blazor.Features.PurchaseOrders.Components;

public sealed class ReceivePurchaseOrderLineFormModel
{
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
    public int Quantity { get; set; }

    [Required(ErrorMessage = "Storage location is required.")]
    public Guid? StorageLocationId { get; set; }

    public string? Reference { get; set; }
}
