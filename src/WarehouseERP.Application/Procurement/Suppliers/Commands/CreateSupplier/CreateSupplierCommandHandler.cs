using WarehouseERP.Application.Common;
using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Domain.Procurement;

namespace WarehouseERP.Application.Procurement.Suppliers.Commands.CreateSupplier;

public sealed class CreateSupplierCommandHandler : ICommandHandler<CreateSupplierCommand, SupplierDto>
{
    private readonly ISupplierRepository _supplierRepository;

    public CreateSupplierCommandHandler(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }

    public async Task<SupplierDto> HandleAsync(CreateSupplierCommand command, CancellationToken cancellationToken)
    {
        var existingSupplier = await _supplierRepository.GetByNameAsync(command.Name, cancellationToken);
        if (existingSupplier is not null)
        {
            throw new DuplicateNameException($"A supplier named '{command.Name}' already exists.");
        }

        var supplier = Supplier.Create(command.Name, command.Email, command.PhoneNumber, command.Address);

        await _supplierRepository.AddAsync(supplier, cancellationToken);

        return SupplierDto.FromDomain(supplier);
    }
}
