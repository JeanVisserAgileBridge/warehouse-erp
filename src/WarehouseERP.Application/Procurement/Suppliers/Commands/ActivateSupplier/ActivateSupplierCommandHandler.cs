using WarehouseERP.Application.Common;
using WarehouseERP.Application.Common.Exceptions;

namespace WarehouseERP.Application.Procurement.Suppliers.Commands.ActivateSupplier;

public sealed class ActivateSupplierCommandHandler : ICommandHandler<ActivateSupplierCommand, SupplierDto>
{
    private readonly ISupplierRepository _supplierRepository;

    public ActivateSupplierCommandHandler(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }

    public async Task<SupplierDto> HandleAsync(ActivateSupplierCommand command, CancellationToken cancellationToken)
    {
        var supplier = await _supplierRepository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new NotFoundException($"Supplier with id '{command.Id}' was not found.");

        supplier.Activate();

        await _supplierRepository.UpdateAsync(supplier, cancellationToken);

        return SupplierDto.FromDomain(supplier);
    }
}
