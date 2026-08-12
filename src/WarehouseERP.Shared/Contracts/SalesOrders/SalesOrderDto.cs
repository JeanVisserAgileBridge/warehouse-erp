namespace WarehouseERP.Shared.Contracts.SalesOrders;

public sealed class SalesOrderDto
{
    public required Guid Id { get; init; }
    public required Guid CustomerId { get; init; }
    public required string OrderNumber { get; init; }
    public required DateTime OrderDate { get; init; }
    public required SalesOrderStatus Status { get; init; }
    public string? Notes { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required DateTime UpdatedAt { get; init; }
    public required IReadOnlyList<SalesOrderLineDto> Lines { get; init; }
}
