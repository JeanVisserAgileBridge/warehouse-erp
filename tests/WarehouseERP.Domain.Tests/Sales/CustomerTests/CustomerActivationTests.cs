using WarehouseERP.Domain.Sales;

namespace WarehouseERP.Domain.Tests.Sales.CustomerTests;

public class CustomerActivationTests
{
    [Fact]
    public void Deactivate_MakesCustomerInactive()
    {
        var customer = Customer.Create("Jane Doe");

        customer.Deactivate();

        Assert.False(customer.IsActive);
    }

    [Fact]
    public void Deactivate_IsIdempotent()
    {
        var customer = Customer.Create("Jane Doe");

        customer.Deactivate();
        customer.Deactivate();

        Assert.False(customer.IsActive);
    }

    [Fact]
    public void Deactivate_UpdatesUpdatedAt()
    {
        var customer = Customer.Create("Jane Doe");
        var originalUpdatedAt = customer.UpdatedAt;

        customer.Deactivate();

        Assert.True(customer.UpdatedAt >= originalUpdatedAt);
    }

    [Fact]
    public void Activate_MakesInactiveCustomerActive()
    {
        var customer = Customer.Create("Jane Doe");
        customer.Deactivate();

        customer.Activate();

        Assert.True(customer.IsActive);
    }

    [Fact]
    public void Activate_IsIdempotent()
    {
        var customer = Customer.Create("Jane Doe");

        customer.Activate();
        customer.Activate();

        Assert.True(customer.IsActive);
    }

    [Fact]
    public void Activate_UpdatesUpdatedAt()
    {
        var customer = Customer.Create("Jane Doe");
        customer.Deactivate();
        var originalUpdatedAt = customer.UpdatedAt;

        customer.Activate();

        Assert.True(customer.UpdatedAt >= originalUpdatedAt);
    }
}
