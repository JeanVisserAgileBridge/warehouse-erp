using WarehouseERP.Shared.Contracts.Inventory;
using ApplicationInventoryItemDto = WarehouseERP.Application.Inventory.InventoryItems.InventoryItemDto;

namespace WarehouseERP.Api.Contracts.Inventory;

internal static class InventoryItemDtoExtensions
{
    public static InventoryItemDto ToContract(this ApplicationInventoryItemDto dto)
    {
        return new InventoryItemDto
        {
            Id = dto.Id,
            ProductId = dto.ProductId,
            StorageLocationId = dto.StorageLocationId,
            QuantityOnHand = dto.QuantityOnHand,
            ReorderLevel = dto.ReorderLevel,
            UpdatedAt = dto.UpdatedAt
        };
    }

    public static IReadOnlyList<InventoryItemDto> ToContract(this IReadOnlyList<ApplicationInventoryItemDto> dtos)
    {
        return dtos.Select(ToContract).ToList();
    }
}
