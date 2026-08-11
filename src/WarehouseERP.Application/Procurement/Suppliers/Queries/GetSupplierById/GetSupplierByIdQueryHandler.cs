using WarehouseERP.Application.Common;
using WarehouseERP.Application.Common.Exceptions;

namespace WarehouseERP.Application.Procurement.Suppliers.Queries.GetSupplierById;

public sealed class GetSupplierByIdQueryHandler : IQueryHandler<GetSupplierByIdQuery, SupplierDto>
{
    private readonly ISupplierRepository _supplierRepository;

    public GetSupplierByIdQueryHandler(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }

    public async Task<SupplierDto> HandleAsync(GetSupplierByIdQuery query, CancellationToken cancellationToken)
    {
        var supplier = await _supplierRepository.GetByIdAsync(query.Id, cancellationToken)
            ?? throw new NotFoundException($"Supplier with id '{query.Id}' was not found.");

        return SupplierDto.FromDomain(supplier);
    }
}
