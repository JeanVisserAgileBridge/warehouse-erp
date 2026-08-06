using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.Sales;

namespace WarehouseERP.Domain.Tests.Sales.CustomerTests;

public class CustomerRenameTests
{
    [Fact]
    public void Rename_UpdatesName()
    {
        var customer = Customer.Create("Jane Doe");

        customer.Rename("Jane Smith");

        Assert.Equal("Jane Smith", customer.Name);
    }

    [Fact]
    public void Rename_UpdatesUpdatedAt()
    {
        var customer = Customer.Create("Jane Doe");
        var originalUpdatedAt = customer.UpdatedAt;

        customer.Rename("Jane Smith");

        Assert.True(customer.UpdatedAt >= originalUpdatedAt);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Rename_RejectsNullEmptyOrWhitespaceName(string? name)
    {
        var customer = Customer.Create("Jane Doe");

        Assert.Throws<DomainException>(() => customer.Rename(name!));
    }

    [Fact]
    public void Rename_RejectsNameLongerThanMaxLength()
    {
        var customer = Customer.Create("Jane Doe");
        var name = new string('a', Customer.MaxNameLength + 1);

        Assert.Throws<DomainException>(() => customer.Rename(name));
    }
}
