using System.ComponentModel.DataAnnotations;

namespace WarehouseERP.Blazor.Features.PurchaseOrders.Components;

public sealed class PurchaseOrderFormModel
{
    [Required(ErrorMessage = "Supplier is required.")]
    public Guid? SupplierId { get; set; }

    [Required(ErrorMessage = "Order number is required.")]
    [StringLength(50, ErrorMessage = "Order number cannot exceed 50 characters.")]
    public string OrderNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Order date is required.")]
    public DateTime OrderDate { get; set; } = DateTime.UtcNow.Date;

    [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
    public string? Notes { get; set; }
}
