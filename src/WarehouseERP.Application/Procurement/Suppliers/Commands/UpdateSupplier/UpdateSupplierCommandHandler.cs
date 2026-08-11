using WarehouseERP.Application.Common;
using WarehouseERP.Application.Common.Exceptions;

namespace WarehouseERP.Application.Procurement.Suppliers.Commands.UpdateSupplier;

public sealed class UpdateSupplierCommandHandler : ICommandHandler<UpdateSupplierCommand, SupplierDto>
{
    private readonly ISupplierRepository _supplierRepository;

    public UpdateSupplierCommandHandler(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }

    public async Task<SupplierDto> HandleAsync(UpdateSupplierCommand command, CancellationToken cancellationToken)
    {
        var supplier = await _supplierRepository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new NotFoundException($"Supplier with id '{command.Id}' was not found.");

        var supplierWithSameName = await _supplierRepository.GetByNameAsync(command.Name, cancellationToken);
        if (supplierWithSameName is not null && supplierWithSameName.Id != supplier.Id)
        {
            throw new DuplicateNameException($"A supplier named '{command.Name}' already exists.");
        }

        supplier.Rename(command.Name);
        supplier.ChangeEmail(command.Email);
        supplier.ChangePhoneNumber(command.PhoneNumber);
        supplier.ChangeAddress(command.Address);

        await _supplierRepository.UpdateAsync(supplier, cancellationToken);

        return SupplierDto.FromDomain(supplier);
    }
}
