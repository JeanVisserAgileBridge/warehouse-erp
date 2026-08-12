namespace WarehouseERP.Shared.Contracts.SalesOrders;

public sealed class FulfilSalesOrderLineRequest
{
    public required int Quantity { get; init; }
    public required Guid StorageLocationId { get; init; }
    public string? Reference { get; init; }
}
