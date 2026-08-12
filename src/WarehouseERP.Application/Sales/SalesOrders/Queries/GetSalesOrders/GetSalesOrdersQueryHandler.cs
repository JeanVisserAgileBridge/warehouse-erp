using WarehouseERP.Application.Common;

namespace WarehouseERP.Application.Sales.SalesOrders.Queries.GetSalesOrders;

public sealed class GetSalesOrdersQueryHandler : IQueryHandler<GetSalesOrdersQuery, IReadOnlyList<SalesOrderDto>>
{
    private readonly ISalesOrderRepository _salesOrderRepository;

    public GetSalesOrdersQueryHandler(ISalesOrderRepository salesOrderRepository)
    {
        _salesOrderRepository = salesOrderRepository;
    }

    public async Task<IReadOnlyList<SalesOrderDto>> HandleAsync(GetSalesOrdersQuery query, CancellationToken cancellationToken)
    {
        var salesOrders = await _salesOrderRepository.GetAllAsync(cancellationToken);

        return salesOrders.Select(SalesOrderDto.FromDomain).ToList();
    }
}
