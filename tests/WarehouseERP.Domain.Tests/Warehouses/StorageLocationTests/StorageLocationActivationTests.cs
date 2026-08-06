using WarehouseERP.Domain.Warehouses;

namespace WarehouseERP.Domain.Tests.Warehouses.StorageLocationTests;

public class StorageLocationActivationTests
{
    [Fact]
    public void Deactivate_MakesStorageLocationInactive()
    {
        var storageLocation = StorageLocation.Create(Guid.NewGuid(), "A1-B2");

        storageLocation.Deactivate();

        Assert.False(storageLocation.IsActive);
    }

    [Fact]
    public void Deactivate_IsIdempotent()
    {
        var storageLocation = StorageLocation.Create(Guid.NewGuid(), "A1-B2");

        storageLocation.Deactivate();
        storageLocation.Deactivate();

        Assert.False(storageLocation.IsActive);
    }

    [Fact]
    public void Deactivate_UpdatesUpdatedAt()
    {
        var storageLocation = StorageLocation.Create(Guid.NewGuid(), "A1-B2");
        var originalUpdatedAt = storageLocation.UpdatedAt;

        storageLocation.Deactivate();

        Assert.True(storageLocation.UpdatedAt >= originalUpdatedAt);
    }

    [Fact]
    public void Activate_MakesInactiveStorageLocationActive()
    {
        var storageLocation = StorageLocation.Create(Guid.NewGuid(), "A1-B2");
        storageLocation.Deactivate();

        storageLocation.Activate();

        Assert.True(storageLocation.IsActive);
    }

    [Fact]
    public void Activate_IsIdempotent()
    {
        var storageLocation = StorageLocation.Create(Guid.NewGuid(), "A1-B2");

        storageLocation.Activate();
        storageLocation.Activate();

        Assert.True(storageLocation.IsActive);
    }

    [Fact]
    public void Activate_UpdatesUpdatedAt()
    {
        var storageLocation = StorageLocation.Create(Guid.NewGuid(), "A1-B2");
        storageLocation.Deactivate();
        var originalUpdatedAt = storageLocation.UpdatedAt;

        storageLocation.Activate();

        Assert.True(storageLocation.UpdatedAt >= originalUpdatedAt);
    }
}
