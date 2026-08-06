using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.Sales;

namespace WarehouseERP.Domain.Tests.Sales.CustomerTests;

public class CustomerChangeAddressTests
{
    [Fact]
    public void ChangeAddress_UpdatesAddress()
    {
        var customer = Customer.Create("Jane Doe", address: "1 Main Street");

        customer.ChangeAddress("2 Commerce Street");

        Assert.Equal("2 Commerce Street", customer.Address);
    }

    [Fact]
    public void ChangeAddress_ClearsAddressWhenNull()
    {
        var customer = Customer.Create("Jane Doe", address: "1 Main Street");

        customer.ChangeAddress(null);

        Assert.Null(customer.Address);
    }

    [Fact]
    public void ChangeAddress_UpdatesUpdatedAt()
    {
        var customer = Customer.Create("Jane Doe");
        var originalUpdatedAt = customer.UpdatedAt;

        customer.ChangeAddress("2 Commerce Street");

        Assert.True(customer.UpdatedAt >= originalUpdatedAt);
    }

    [Fact]
    public void ChangeAddress_RejectsAddressLongerThanMaxLength()
    {
        var customer = Customer.Create("Jane Doe");
        var address = new string('a', Customer.MaxAddressLength + 1);

        Assert.Throws<DomainException>(() => customer.ChangeAddress(address));
    }

    [Fact]
    public void ChangeAddress_AcceptsAddressAtMaxLength()
    {
        var customer = Customer.Create("Jane Doe");
        var address = new string('a', Customer.MaxAddressLength);

        customer.ChangeAddress(address);

        Assert.Equal(address, customer.Address);
    }
}
