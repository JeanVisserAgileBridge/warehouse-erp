using WarehouseERP.Application.Common;
using WarehouseERP.Application.Common.Exceptions;

namespace WarehouseERP.Application.Warehouses.Warehouses.Queries.GetWarehouseById;

public sealed class GetWarehouseByIdQueryHandler : IQueryHandler<GetWarehouseByIdQuery, WarehouseDto>
{
    private readonly IWarehouseRepository _warehouseRepository;

    public GetWarehouseByIdQueryHandler(IWarehouseRepository warehouseRepository)
    {
        _warehouseRepository = warehouseRepository;
    }

    public async Task<WarehouseDto> HandleAsync(GetWarehouseByIdQuery query, CancellationToken cancellationToken)
    {
        var warehouse = await _warehouseRepository.GetByIdAsync(query.Id, cancellationToken)
            ?? throw new NotFoundException($"Warehouse with id '{query.Id}' was not found.");

        return WarehouseDto.FromDomain(warehouse);
    }
}
