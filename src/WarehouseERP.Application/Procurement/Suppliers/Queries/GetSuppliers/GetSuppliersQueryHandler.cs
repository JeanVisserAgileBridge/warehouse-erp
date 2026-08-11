using WarehouseERP.Application.Common;

namespace WarehouseERP.Application.Procurement.Suppliers.Queries.GetSuppliers;

public sealed class GetSuppliersQueryHandler : IQueryHandler<GetSuppliersQuery, IReadOnlyList<SupplierDto>>
{
    private readonly ISupplierRepository _supplierRepository;

    public GetSuppliersQueryHandler(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }

    public async Task<IReadOnlyList<SupplierDto>> HandleAsync(GetSuppliersQuery query, CancellationToken cancellationToken)
    {
        var suppliers = await _supplierRepository.GetAllAsync(cancellationToken);

        return suppliers.Select(SupplierDto.FromDomain).ToList();
    }
}
