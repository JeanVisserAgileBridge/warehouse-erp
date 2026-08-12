namespace WarehouseERP.Shared.Contracts.SalesOrders;

public sealed class AddSalesOrderLineRequest
{
    public required Guid ProductId { get; init; }
    public required int QuantityOrdered { get; init; }
    public required decimal UnitPrice { get; init; }
}
