using WarehouseERP.Application.Common;

namespace WarehouseERP.Application.Warehouses.Warehouses.Queries.GetWarehouses;

public sealed class GetWarehousesQueryHandler : IQueryHandler<GetWarehousesQuery, IReadOnlyList<WarehouseDto>>
{
    private readonly IWarehouseRepository _warehouseRepository;

    public GetWarehousesQueryHandler(IWarehouseRepository warehouseRepository)
    {
        _warehouseRepository = warehouseRepository;
    }

    public async Task<IReadOnlyList<WarehouseDto>> HandleAsync(GetWarehousesQuery query, CancellationToken cancellationToken)
    {
        var warehouses = await _warehouseRepository.GetAllAsync(cancellationToken);

        return warehouses.Select(WarehouseDto.FromDomain).ToList();
    }
}
