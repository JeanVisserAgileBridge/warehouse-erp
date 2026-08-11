using WarehouseERP.Application.Inventory.InventoryItems;
using WarehouseERP.Domain.Inventory;

namespace WarehouseERP.Application.Tests.Inventory.InventoryItems.Fakes;

public sealed class FakeInventoryItemRepository : IInventoryItemRepository
{
    private readonly List<InventoryItem> _inventoryItems = new();

    public CancellationToken? LastCancellationToken { get; private set; }
    public int AddCallCount { get; private set; }
    public int UpdateCallCount { get; private set; }

    public void Seed(InventoryItem inventoryItem)
    {
        _inventoryItems.Add(inventoryItem);
    }

    public Task<InventoryItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        LastCancellationToken = cancellationToken;
        return Task.FromResult(_inventoryItems.FirstOrDefault(inventoryItem => inventoryItem.Id == id));
    }

    public Task<IReadOnlyList<InventoryItem>> GetAllAsync(CancellationToken cancellationToken)
    {
        LastCancellationToken = cancellationToken;
        return Task.FromResult<IReadOnlyList<InventoryItem>>(_inventoryItems.ToList());
    }

    public Task<IReadOnlyList<InventoryItem>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken)
    {
        LastCancellationToken = cancellationToken;
        return Task.FromResult<IReadOnlyList<InventoryItem>>(
            _inventoryItems.Where(inventoryItem => inventoryItem.ProductId == productId).ToList());
    }

    public Task<IReadOnlyList<InventoryItem>> GetByStorageLocationIdAsync(Guid storageLocationId, CancellationToken cancellationToken)
    {
        LastCancellationToken = cancellationToken;
        return Task.FromResult<IReadOnlyList<InventoryItem>>(
            _inventoryItems.Where(inventoryItem => inventoryItem.StorageLocationId == storageLocationId).ToList());
    }

    public Task<InventoryItem?> GetByProductIdAndStorageLocationIdAsync(Guid productId, Guid storageLocationId, CancellationToken cancellationToken)
    {
        LastCancellationToken = cancellationToken;
        return Task.FromResult(_inventoryItems.FirstOrDefault(
            inventoryItem => inventoryItem.ProductId == productId && inventoryItem.StorageLocationId == storageLocationId));
    }

    public Task AddAsync(InventoryItem inventoryItem, CancellationToken cancellationToken)
    {
        LastCancellationToken = cancellationToken;
        AddCallCount++;
        _inventoryItems.Add(inventoryItem);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(InventoryItem inventoryItem, CancellationToken cancellationToken)
    {
        LastCancellationToken = cancellationToken;
        UpdateCallCount++;
        return Task.CompletedTask;
    }
}
