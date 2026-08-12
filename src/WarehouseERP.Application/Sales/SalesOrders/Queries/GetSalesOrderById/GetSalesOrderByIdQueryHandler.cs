using WarehouseERP.Application.Common;
using WarehouseERP.Application.Common.Exceptions;

namespace WarehouseERP.Application.Sales.SalesOrders.Queries.GetSalesOrderById;

public sealed class GetSalesOrderByIdQueryHandler : IQueryHandler<GetSalesOrderByIdQuery, SalesOrderDto>
{
    private readonly ISalesOrderRepository _salesOrderRepository;

    public GetSalesOrderByIdQueryHandler(ISalesOrderRepository salesOrderRepository)
    {
        _salesOrderRepository = salesOrderRepository;
    }

    public async Task<SalesOrderDto> HandleAsync(GetSalesOrderByIdQuery query, CancellationToken cancellationToken)
    {
        var salesOrder = await _salesOrderRepository.GetByIdAsync(query.Id, cancellationToken)
            ?? throw new NotFoundException($"Sales order with id '{query.Id}' was not found.");

        return SalesOrderDto.FromDomain(salesOrder);
    }
}
