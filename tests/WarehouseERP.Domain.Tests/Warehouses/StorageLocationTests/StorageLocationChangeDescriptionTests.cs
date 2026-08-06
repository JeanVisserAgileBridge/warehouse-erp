using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.Warehouses;

namespace WarehouseERP.Domain.Tests.Warehouses.StorageLocationTests;

public class StorageLocationChangeDescriptionTests
{
    [Fact]
    public void ChangeDescription_UpdatesDescription()
    {
        var storageLocation = StorageLocation.Create(Guid.NewGuid(), "A1-B2", "Shelf near loading dock");

        storageLocation.ChangeDescription("Top shelf, aisle 3");

        Assert.Equal("Top shelf, aisle 3", storageLocation.Description);
    }

    [Fact]
    public void ChangeDescription_ClearsDescriptionWhenNull()
    {
        var storageLocation = StorageLocation.Create(Guid.NewGuid(), "A1-B2", "Shelf near loading dock");

        storageLocation.ChangeDescription(null);

        Assert.Null(storageLocation.Description);
    }

    [Fact]
    public void ChangeDescription_UpdatesUpdatedAt()
    {
        var storageLocation = StorageLocation.Create(Guid.NewGuid(), "A1-B2");
        var originalUpdatedAt = storageLocation.UpdatedAt;

        storageLocation.ChangeDescription("Top shelf, aisle 3");

        Assert.True(storageLocation.UpdatedAt >= originalUpdatedAt);
    }

    [Fact]
    public void ChangeDescription_RejectsDescriptionLongerThanMaxLength()
    {
        var storageLocation = StorageLocation.Create(Guid.NewGuid(), "A1-B2");
        var description = new string('a', StorageLocation.MaxDescriptionLength + 1);

        Assert.Throws<DomainException>(() => storageLocation.ChangeDescription(description));
    }

    [Fact]
    public void ChangeDescription_AcceptsDescriptionAtMaxLength()
    {
        var storageLocation = StorageLocation.Create(Guid.NewGuid(), "A1-B2");
        var description = new string('a', StorageLocation.MaxDescriptionLength);

        storageLocation.ChangeDescription(description);

        Assert.Equal(description, storageLocation.Description);
    }
}
