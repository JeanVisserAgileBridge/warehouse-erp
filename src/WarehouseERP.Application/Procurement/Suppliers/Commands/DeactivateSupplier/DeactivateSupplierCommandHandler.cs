using WarehouseERP.Application.Common;
using WarehouseERP.Application.Common.Exceptions;

namespace WarehouseERP.Application.Procurement.Suppliers.Commands.DeactivateSupplier;

public sealed class DeactivateSupplierCommandHandler : ICommandHandler<DeactivateSupplierCommand, SupplierDto>
{
    private readonly ISupplierRepository _supplierRepository;

    public DeactivateSupplierCommandHandler(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }

    public async Task<SupplierDto> HandleAsync(DeactivateSupplierCommand command, CancellationToken cancellationToken)
    {
        var supplier = await _supplierRepository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new NotFoundException($"Supplier with id '{command.Id}' was not found.");

        supplier.Deactivate();

        await _supplierRepository.UpdateAsync(supplier, cancellationToken);

        return SupplierDto.FromDomain(supplier);
    }
}
