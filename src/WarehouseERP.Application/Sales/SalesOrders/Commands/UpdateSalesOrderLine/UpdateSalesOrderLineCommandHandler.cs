using WarehouseERP.Application.Common;
using WarehouseERP.Application.Common.Exceptions;

namespace WarehouseERP.Application.Sales.SalesOrders.Commands.UpdateSalesOrderLine;

// There is no Domain "UpdateLine" behaviour. A line is edited by removing and re-adding it for
// the same ProductId, both of which already require the order to be Draft and re-validate
// quantity/price. Since a line can only be edited while Draft, QuantityFulfilled is always zero
// at this point, so nothing is lost by replacing it.
public sealed class UpdateSalesOrderLineCommandHandler : ICommandHandler<UpdateSalesOrderLineCommand, SalesOrderDto>
{
    private readonly ISalesOrderRepository _salesOrderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateSalesOrderLineCommandHandler(ISalesOrderRepository salesOrderRepository, IUnitOfWork unitOfWork)
    {
        _salesOrderRepository = salesOrderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<SalesOrderDto> HandleAsync(UpdateSalesOrderLineCommand command, CancellationToken cancellationToken)
    {
        var salesOrder = await _salesOrderRepository.GetByIdAsync(command.SalesOrderId, cancellationToken)
            ?? throw new NotFoundException($"Sales order with id '{command.SalesOrderId}' was not found.");

        salesOrder.RemoveLine(command.ProductId);
        salesOrder.AddLine(command.ProductId, command.QuantityOrdered, command.UnitPrice);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return SalesOrderDto.FromDomain(salesOrder);
    }
}
