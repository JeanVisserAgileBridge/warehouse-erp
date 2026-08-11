using WarehouseERP.Shared.Contracts.Suppliers;
using ApplicationSupplierDto = WarehouseERP.Application.Procurement.Suppliers.SupplierDto;

namespace WarehouseERP.Api.Contracts.Suppliers;

internal static class SupplierDtoExtensions
{
    public static SupplierDto ToContract(this ApplicationSupplierDto dto)
    {
        return new SupplierDto
        {
            Id = dto.Id,
            Name = dto.Name,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Address = dto.Address,
            IsActive = dto.IsActive
        };
    }

    public static IReadOnlyList<SupplierDto> ToContract(this IReadOnlyList<ApplicationSupplierDto> dtos)
    {
        return dtos.Select(ToContract).ToList();
    }
}
