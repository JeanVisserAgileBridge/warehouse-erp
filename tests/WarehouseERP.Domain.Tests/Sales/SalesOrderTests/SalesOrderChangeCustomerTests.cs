using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.Sales;

namespace WarehouseERP.Domain.Tests.Sales.SalesOrderTests;

public class SalesOrderChangeCustomerTests
{
    [Fact]
    public void ChangeCustomer_UpdatesCustomerId()
    {
        var salesOrder = CreateDraftOrder();
        var newCustomerId = Guid.NewGuid();

        salesOrder.ChangeCustomer(newCustomerId);

        Assert.Equal(newCustomerId, salesOrder.CustomerId);
    }

    [Fact]
    public void ChangeCustomer_UpdatesUpdatedAt()
    {
        var salesOrder = CreateDraftOrder();
        var originalUpdatedAt = salesOrder.UpdatedAt;

        salesOrder.ChangeCustomer(Guid.NewGuid());

        Assert.True(salesOrder.UpdatedAt >= originalUpdatedAt);
    }

    [Fact]
    public void ChangeCustomer_RejectsEmptyCustomerId()
    {
        var salesOrder = CreateDraftOrder();

        Assert.Throws<DomainException>(() => salesOrder.ChangeCustomer(Guid.Empty));
    }

    [Fact]
    public void ChangeCustomer_ThrowsWhenOrderIsConfirmed()
    {
        var salesOrder = CreateDraftOrder();
        salesOrder.AddLine(Guid.NewGuid(), 10, 4.50m);
        salesOrder.Confirm();

        Assert.Throws<DomainException>(() => salesOrder.ChangeCustomer(Guid.NewGuid()));
    }

    [Fact]
    public void ChangeCustomer_ThrowsWhenOrderIsCancelled()
    {
        var salesOrder = CreateDraftOrder();
        salesOrder.Cancel();

        Assert.Throws<DomainException>(() => salesOrder.ChangeCustomer(Guid.NewGuid()));
    }

    private static SalesOrder CreateDraftOrder()
    {
        return SalesOrder.Create(Guid.NewGuid(), "SO-001", DateTime.UtcNow);
    }
}
