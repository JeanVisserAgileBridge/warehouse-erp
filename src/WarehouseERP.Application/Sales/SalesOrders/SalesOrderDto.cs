using WarehouseERP.Domain.Sales;

namespace WarehouseERP.Application.Sales.SalesOrders;

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

    public static SalesOrderDto FromDomain(SalesOrder salesOrder)
    {
        return new SalesOrderDto
        {
            Id = salesOrder.Id,
            CustomerId = salesOrder.CustomerId,
            OrderNumber = salesOrder.OrderNumber,
            OrderDate = salesOrder.OrderDate,
            Status = salesOrder.Status,
            Notes = salesOrder.Notes,
            CreatedAt = salesOrder.CreatedAt,
            UpdatedAt = salesOrder.UpdatedAt,
            Lines = salesOrder.Lines.Select(SalesOrderLineDto.FromDomain).ToList()
        };
    }
}
