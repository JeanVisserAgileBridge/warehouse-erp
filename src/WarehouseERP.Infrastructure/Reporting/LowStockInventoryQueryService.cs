using Dapper;
using Microsoft.Data.SqlClient;
using WarehouseERP.Application.Inventory.LowStock;

namespace WarehouseERP.Infrastructure.Reporting;

public class LowStockInventoryQueryService : ILowStockInventoryQueryService
{
    private const string LowStockItemsSql = """
        SELECT
            Id AS InventoryItemId,
            ProductId,
            StorageLocationId,
            QuantityOnHand,
            ReorderLevel
        FROM InventoryItems
        WHERE QuantityOnHand <= ReorderLevel;
        """;

    private readonly string _connectionString;

    public LowStockInventoryQueryService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IReadOnlyList<LowStockInventoryItem>> GetLowStockItemsAsync(CancellationToken cancellationToken)
    {
        using var connection = new SqlConnection(_connectionString);

        var command = new CommandDefinition(LowStockItemsSql, cancellationToken: cancellationToken);

        var items = await connection.QueryAsync<LowStockInventoryItem>(command);

        return items.ToList();
    }
}
