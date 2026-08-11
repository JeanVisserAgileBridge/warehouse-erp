using WarehouseERP.Shared.Contracts.Inventory;
using ApplicationStockMovementDto = WarehouseERP.Application.Inventory.StockMovements.StockMovementDto;
using DomainStockMovementType = WarehouseERP.Domain.Inventory.StockMovementType;

namespace WarehouseERP.Api.Contracts.Inventory;

internal static class StockMovementDtoExtensions
{
    public static StockMovementDto ToContract(this ApplicationStockMovementDto dto)
    {
        return new StockMovementDto
        {
            Id = dto.Id,
            InventoryItemId = dto.InventoryItemId,
            MovementType = dto.MovementType.ToContract(),
            Quantity = dto.Quantity,
            Reference = dto.Reference,
            OccurredAt = dto.OccurredAt
        };
    }

    public static IReadOnlyList<StockMovementDto> ToContract(this IReadOnlyList<ApplicationStockMovementDto> dtos)
    {
        return dtos.Select(ToContract).ToList();
    }

    private static StockMovementType ToContract(this DomainStockMovementType movementType) => movementType switch
    {
        DomainStockMovementType.Receipt => StockMovementType.Receipt,
        DomainStockMovementType.Issue => StockMovementType.Issue,
        DomainStockMovementType.Adjustment => StockMovementType.Adjustment,
        DomainStockMovementType.Transfer => StockMovementType.Transfer,
        DomainStockMovementType.Return => StockMovementType.Return,
        _ => throw new ArgumentOutOfRangeException(nameof(movementType), movementType, "Unknown stock movement type.")
    };
}
