namespace WarehouseERP.Shared.Contracts.SalesOrders;

public sealed class UpdateSalesOrderLineRequest
{
    public required int QuantityOrdered { get; init; }
    public required decimal UnitPrice { get; init; }
}
