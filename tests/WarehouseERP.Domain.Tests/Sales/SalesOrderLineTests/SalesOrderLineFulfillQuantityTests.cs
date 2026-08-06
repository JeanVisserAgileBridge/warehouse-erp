using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.Sales;

namespace WarehouseERP.Domain.Tests.Sales.SalesOrderLineTests;

public class SalesOrderLineFulfillQuantityTests
{
    [Fact]
    public void FulfillQuantity_IncreasesQuantityFulfilled()
    {
        var line = SalesOrderLine.Create(Guid.NewGuid(), Guid.NewGuid(), 10, 4.50m);

        line.FulfillQuantity(4);

        Assert.Equal(4, line.QuantityFulfilled);
    }

    [Fact]
    public void FulfillQuantity_AccumulatesAcrossMultipleCalls()
    {
        var line = SalesOrderLine.Create(Guid.NewGuid(), Guid.NewGuid(), 10, 4.50m);

        line.FulfillQuantity(4);
        line.FulfillQuantity(6);

        Assert.Equal(10, line.QuantityFulfilled);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void FulfillQuantity_RejectsZeroOrNegativeQuantity(int quantity)
    {
        var line = SalesOrderLine.Create(Guid.NewGuid(), Guid.NewGuid(), 10, 4.50m);

        Assert.Throws<DomainException>(() => line.FulfillQuantity(quantity));
    }

    [Fact]
    public void FulfillQuantity_ThrowsWhenExceedingQuantityOrdered()
    {
        var line = SalesOrderLine.Create(Guid.NewGuid(), Guid.NewGuid(), 10, 4.50m);

        Assert.Throws<DomainException>(() => line.FulfillQuantity(11));
    }

    [Fact]
    public void FulfillQuantity_ThrowsWhenCumulativeFulfilledWouldExceedQuantityOrdered()
    {
        var line = SalesOrderLine.Create(Guid.NewGuid(), Guid.NewGuid(), 10, 4.50m);
        line.FulfillQuantity(8);

        Assert.Throws<DomainException>(() => line.FulfillQuantity(3));
    }

    [Fact]
    public void FulfillQuantity_DoesNotChangeStateWhenRejected()
    {
        var line = SalesOrderLine.Create(Guid.NewGuid(), Guid.NewGuid(), 10, 4.50m);
        line.FulfillQuantity(8);

        Assert.Throws<DomainException>(() => line.FulfillQuantity(3));

        Assert.Equal(8, line.QuantityFulfilled);
    }

    [Fact]
    public void FulfillQuantity_AllowsFulfillingExactRemainingQuantity()
    {
        var line = SalesOrderLine.Create(Guid.NewGuid(), Guid.NewGuid(), 10, 4.50m);
        line.FulfillQuantity(7);

        line.FulfillQuantity(3);

        Assert.Equal(10, line.QuantityFulfilled);
    }
}
