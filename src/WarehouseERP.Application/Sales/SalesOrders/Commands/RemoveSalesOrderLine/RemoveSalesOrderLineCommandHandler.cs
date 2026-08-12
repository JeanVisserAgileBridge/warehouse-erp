using WarehouseERP.Application.Common;
using WarehouseERP.Application.Common.Exceptions;

namespace WarehouseERP.Application.Sales.SalesOrders.Commands.RemoveSalesOrderLine;

public sealed class RemoveSalesOrderLineCommandHandler : ICommandHandler<RemoveSalesOrderLineCommand, SalesOrderDto>
{
    private readonly ISalesOrderRepository _salesOrderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveSalesOrderLineCommandHandler(ISalesOrderRepository salesOrderRepository, IUnitOfWork unitOfWork)
    {
        _salesOrderRepository = salesOrderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<SalesOrderDto> HandleAsync(RemoveSalesOrderLineCommand command, CancellationToken cancellationToken)
    {
        var salesOrder = await _salesOrderRepository.GetByIdAsync(command.SalesOrderId, cancellationToken)
            ?? throw new NotFoundException($"Sales order with id '{command.SalesOrderId}' was not found.");

        salesOrder.RemoveLine(command.ProductId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return SalesOrderDto.FromDomain(salesOrder);
    }
}
