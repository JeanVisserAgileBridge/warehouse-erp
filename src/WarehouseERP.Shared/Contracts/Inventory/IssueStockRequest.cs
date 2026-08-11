namespace WarehouseERP.Shared.Contracts.Inventory;

public sealed class IssueStockRequest
{
    public required int Quantity { get; init; }
    public string? Reference { get; init; }
}
