using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.Warehouses;

namespace WarehouseERP.Domain.Tests.Warehouses.StorageLocationTests;

public class StorageLocationChangeCodeTests
{
    [Fact]
    public void ChangeCode_UpdatesCode()
    {
        var storageLocation = StorageLocation.Create(Guid.NewGuid(), "A1-B2");

        storageLocation.ChangeCode("A1-B3");

        Assert.Equal("A1-B3", storageLocation.Code);
    }

    [Fact]
    public void ChangeCode_UpdatesUpdatedAt()
    {
        var storageLocation = StorageLocation.Create(Guid.NewGuid(), "A1-B2");
        var originalUpdatedAt = storageLocation.UpdatedAt;

        storageLocation.ChangeCode("A1-B3");

        Assert.True(storageLocation.UpdatedAt >= originalUpdatedAt);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ChangeCode_RejectsNullEmptyOrWhitespaceCode(string? code)
    {
        var storageLocation = StorageLocation.Create(Guid.NewGuid(), "A1-B2");

        Assert.Throws<DomainException>(() => storageLocation.ChangeCode(code!));
    }

    [Fact]
    public void ChangeCode_RejectsCodeLongerThanMaxLength()
    {
        var storageLocation = StorageLocation.Create(Guid.NewGuid(), "A1-B2");
        var code = new string('a', StorageLocation.MaxCodeLength + 1);

        Assert.Throws<DomainException>(() => storageLocation.ChangeCode(code));
    }

    [Fact]
    public void ChangeCode_AcceptsCodeAtMaxLength()
    {
        var storageLocation = StorageLocation.Create(Guid.NewGuid(), "A1-B2");
        var code = new string('a', StorageLocation.MaxCodeLength);

        storageLocation.ChangeCode(code);

        Assert.Equal(code, storageLocation.Code);
    }
}
