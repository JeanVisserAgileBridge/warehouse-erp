using WarehouseERP.Shared.Contracts.Warehouses;
using ApplicationWarehouseDto = WarehouseERP.Application.Warehouses.Warehouses.WarehouseDto;

namespace WarehouseERP.Api.Contracts.Warehouses;

internal static class WarehouseDtoExtensions
{
    public static WarehouseDto ToContract(this ApplicationWarehouseDto dto)
    {
        return new WarehouseDto
        {
            Id = dto.Id,
            Code = dto.Code,
            Name = dto.Name,
            Address = dto.Address,
            IsActive = dto.IsActive
        };
    }

    public static IReadOnlyList<WarehouseDto> ToContract(this IReadOnlyList<ApplicationWarehouseDto> dtos)
    {
        return dtos.Select(ToContract).ToList();
    }
}
