using WarehouseERP.Application.Common;
using WarehouseERP.Application.Common.Exceptions;

namespace WarehouseERP.Application.Sales.SalesOrders.Commands.ConfirmSalesOrder;

public sealed class ConfirmSalesOrderCommandHandler : ICommandHandler<ConfirmSalesOrderCommand, SalesOrderDto>
{
    private readonly ISalesOrderRepository _salesOrderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ConfirmSalesOrderCommandHandler(ISalesOrderRepository salesOrderRepository, IUnitOfWork unitOfWork)
    {
        _salesOrderRepository = salesOrderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<SalesOrderDto> HandleAsync(ConfirmSalesOrderCommand command, CancellationToken cancellationToken)
    {
        var salesOrder = await _salesOrderRepository.GetByIdAsync(command.SalesOrderId, cancellationToken)
            ?? throw new NotFoundException($"Sales order with id '{command.SalesOrderId}' was not found.");

        salesOrder.Confirm();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return SalesOrderDto.FromDomain(salesOrder);
    }
}
