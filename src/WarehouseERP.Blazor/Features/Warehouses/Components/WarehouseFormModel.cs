using System.ComponentModel.DataAnnotations;

namespace WarehouseERP.Blazor.Features.Warehouses.Components;

public sealed class WarehouseFormModel
{
    [Required(ErrorMessage = "Code is required.")]
    [StringLength(20, ErrorMessage = "Code cannot exceed 20 characters.")]
    public string Code { get; set; } = string.Empty;

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Address cannot exceed 500 characters.")]
    public string? Address { get; set; }
}
