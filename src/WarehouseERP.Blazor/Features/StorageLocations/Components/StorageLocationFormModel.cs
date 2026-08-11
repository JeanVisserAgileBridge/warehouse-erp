using System.ComponentModel.DataAnnotations;

namespace WarehouseERP.Blazor.Features.StorageLocations.Components;

public sealed class StorageLocationFormModel
{
    public Guid WarehouseId { get; set; }

    [Required(ErrorMessage = "Code is required.")]
    [StringLength(30, ErrorMessage = "Code cannot exceed 30 characters.")]
    public string Code { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
    public string? Description { get; set; }
}
