using System.ComponentModel.DataAnnotations;

namespace WarehouseERP.Blazor.Features.Admin.Users.Components;

public sealed class CreateUserFormModel
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; } = string.Empty;

    public HashSet<string> Roles { get; } = [];
}
