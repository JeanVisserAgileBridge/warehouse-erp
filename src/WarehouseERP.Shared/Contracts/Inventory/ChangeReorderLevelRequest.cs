namespace WarehouseERP.Shared.Contracts.Inventory;

public sealed class ChangeReorderLevelRequest
{
    public required int ReorderLevel { get; init; }
}
