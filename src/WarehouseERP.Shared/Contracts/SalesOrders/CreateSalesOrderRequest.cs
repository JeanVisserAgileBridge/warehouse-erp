namespace WarehouseERP.Shared.Contracts.SalesOrders;

public sealed class CreateSalesOrderRequest
{
    public required Guid CustomerId { get; init; }
    public required string OrderNumber { get; init; }
    public required DateTime OrderDate { get; init; }
    public string? Notes { get; init; }
}
