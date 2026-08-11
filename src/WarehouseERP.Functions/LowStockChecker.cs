using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using WarehouseERP.Application.Inventory.LowStock;

namespace WarehouseERP.Functions;

public class LowStockChecker
{
    private readonly ILowStockInventoryQueryService _lowStockInventoryQueryService;
    private readonly ILogger<LowStockChecker> _logger;

    public LowStockChecker(
        ILowStockInventoryQueryService lowStockInventoryQueryService,
        ILogger<LowStockChecker> logger)
    {
        _lowStockInventoryQueryService = lowStockInventoryQueryService;
        _logger = logger;
    }

    [Function("LowStockChecker")]
    public async Task RunAsync([TimerTrigger("0 */5 * * * *")] TimerInfo timerInfo, CancellationToken cancellationToken)
    {
        var lowStockItems = await _lowStockInventoryQueryService.GetLowStockItemsAsync(cancellationToken);

        if (lowStockItems.Count == 0)
        {
            _logger.LogInformation("Low stock check complete. No inventory items are at or below their reorder level.");
            return;
        }

        foreach (var item in lowStockItems)
        {
            _logger.LogWarning(
                "Low stock: InventoryItemId={InventoryItemId} ProductId={ProductId} StorageLocationId={StorageLocationId} QuantityOnHand={QuantityOnHand} ReorderLevel={ReorderLevel}",
                item.InventoryItemId,
                item.ProductId,
                item.StorageLocationId,
                item.QuantityOnHand,
                item.ReorderLevel);
        }
    }
}
