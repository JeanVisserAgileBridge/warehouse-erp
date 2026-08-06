using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.Warehouses;

namespace WarehouseERP.Domain.Tests.Warehouses.StorageLocationTests;

public class StorageLocationCreateTests
{
    [Fact]
    public void Create_ReturnsStorageLocationWithNonEmptyGuid()
    {
        var storageLocation = StorageLocation.Create(Guid.NewGuid(), "A1-B2");

        Assert.NotEqual(Guid.Empty, storageLocation.Id);
    }

    [Fact]
    public void Create_StoresSuppliedValues()
    {
        var warehouseId = Guid.NewGuid();

        var storageLocation = StorageLocation.Create(warehouseId, "A1-B2", "Shelf near loading dock");

        Assert.Equal(warehouseId, storageLocation.WarehouseId);
        Assert.Equal("A1-B2", storageLocation.Code);
        Assert.Equal("Shelf near loading dock", storageLocation.Description);
    }

    [Fact]
    public void Create_MakesStorageLocationActive()
    {
        var storageLocation = StorageLocation.Create(Guid.NewGuid(), "A1-B2");

        Assert.True(storageLocation.IsActive);
    }

    [Fact]
    public void Create_SetsCreatedAtAndUpdatedAtToSameValue()
    {
        var storageLocation = StorageLocation.Create(Guid.NewGuid(), "A1-B2");

        Assert.Equal(storageLocation.CreatedAt, storageLocation.UpdatedAt);
    }

    [Fact]
    public void Create_AcceptsNullDescription()
    {
        var storageLocation = StorageLocation.Create(Guid.NewGuid(), "A1-B2");

        Assert.Null(storageLocation.Description);
    }

    [Fact]
    public void Create_RejectsEmptyWarehouseId()
    {
        Assert.Throws<DomainException>(() => StorageLocation.Create(Guid.Empty, "A1-B2"));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_RejectsNullEmptyOrWhitespaceCode(string? code)
    {
        Assert.Throws<DomainException>(() => StorageLocation.Create(Guid.NewGuid(), code!));
    }

    [Fact]
    public void Create_RejectsCodeLongerThanMaxLength()
    {
        var code = new string('a', StorageLocation.MaxCodeLength + 1);

        Assert.Throws<DomainException>(() => StorageLocation.Create(Guid.NewGuid(), code));
    }

    [Fact]
    public void Create_AcceptsCodeAtMaxLength()
    {
        var code = new string('a', StorageLocation.MaxCodeLength);

        var storageLocation = StorageLocation.Create(Guid.NewGuid(), code);

        Assert.Equal(code, storageLocation.Code);
    }

    [Fact]
    public void Create_RejectsDescriptionLongerThanMaxLength()
    {
        var description = new string('a', StorageLocation.MaxDescriptionLength + 1);

        Assert.Throws<DomainException>(() => StorageLocation.Create(Guid.NewGuid(), "A1-B2", description));
    }

    [Fact]
    public void Create_AcceptsDescriptionAtMaxLength()
    {
        var description = new string('a', StorageLocation.MaxDescriptionLength);

        var storageLocation = StorageLocation.Create(Guid.NewGuid(), "A1-B2", description);

        Assert.Equal(description, storageLocation.Description);
    }
}
