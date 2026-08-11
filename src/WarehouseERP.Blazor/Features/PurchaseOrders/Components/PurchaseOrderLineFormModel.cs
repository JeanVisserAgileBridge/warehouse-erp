using System.ComponentModel.DataAnnotations;

namespace WarehouseERP.Blazor.Features.PurchaseOrders.Components;

public sealed class PurchaseOrderLineFormModel
{
    [Required(ErrorMessage = "Product is required.")]
    public Guid? ProductId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Quantity ordered must be greater than zero.")]
    public int QuantityOrdered { get; set; }

    [Range(0, (double)decimal.MaxValue, ErrorMessage = "Unit price cannot be negative.")]
    public decimal UnitPrice { get; set; }
}
