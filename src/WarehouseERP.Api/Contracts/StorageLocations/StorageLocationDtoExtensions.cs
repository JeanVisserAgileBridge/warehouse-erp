using WarehouseERP.Shared.Contracts.StorageLocations;
using ApplicationStorageLocationDto = WarehouseERP.Application.Warehouses.StorageLocations.StorageLocationDto;

namespace WarehouseERP.Api.Contracts.StorageLocations;

internal static class StorageLocationDtoExtensions
{
    public static StorageLocationDto ToContract(this ApplicationStorageLocationDto dto)
    {
        return new StorageLocationDto
        {
            Id = dto.Id,
            WarehouseId = dto.WarehouseId,
            Code = dto.Code,
            Description = dto.Description,
            IsActive = dto.IsActive
        };
    }

    public static IReadOnlyList<StorageLocationDto> ToContract(this IReadOnlyList<ApplicationStorageLocationDto> dtos)
    {
        return dtos.Select(ToContract).ToList();
    }
}
