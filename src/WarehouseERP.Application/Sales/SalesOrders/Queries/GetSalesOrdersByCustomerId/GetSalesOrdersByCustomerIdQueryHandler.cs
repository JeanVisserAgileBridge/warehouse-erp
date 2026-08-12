using WarehouseERP.Application.Common;

namespace WarehouseERP.Application.Sales.SalesOrders.Queries.GetSalesOrdersByCustomerId;

public sealed class GetSalesOrdersByCustomerIdQueryHandler
    : IQueryHandler<GetSalesOrdersByCustomerIdQuery, IReadOnlyList<SalesOrderDto>>
{
    private readonly ISalesOrderRepository _salesOrderRepository;

    public GetSalesOrdersByCustomerIdQueryHandler(ISalesOrderRepository salesOrderRepository)
    {
        _salesOrderRepository = salesOrderRepository;
    }

    public async Task<IReadOnlyList<SalesOrderDto>> HandleAsync(
        GetSalesOrdersByCustomerIdQuery query, CancellationToken cancellationToken)
    {
        var salesOrders = await _salesOrderRepository.GetByCustomerIdAsync(query.CustomerId, cancellationToken);

        return salesOrders.Select(SalesOrderDto.FromDomain).ToList();
    }
}
