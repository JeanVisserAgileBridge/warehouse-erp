using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.Procurement;

namespace WarehouseERP.Domain.Tests.Procurement.PurchaseOrderLineTests;

public class PurchaseOrderLineReceiveQuantityTests
{
    [Fact]
    public void ReceiveQuantity_IncreasesQuantityReceived()
    {
        var line = PurchaseOrderLine.Create(Guid.NewGuid(), Guid.NewGuid(), 10, 4.50m);

        line.ReceiveQuantity(4);

        Assert.Equal(4, line.QuantityReceived);
    }

    [Fact]
    public void ReceiveQuantity_AccumulatesAcrossMultipleCalls()
    {
        var line = PurchaseOrderLine.Create(Guid.NewGuid(), Guid.NewGuid(), 10, 4.50m);

        line.ReceiveQuantity(4);
        line.ReceiveQuantity(6);

        Assert.Equal(10, line.QuantityReceived);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void ReceiveQuantity_RejectsZeroOrNegativeQuantity(int quantity)
    {
        var line = PurchaseOrderLine.Create(Guid.NewGuid(), Guid.NewGuid(), 10, 4.50m);

        Assert.Throws<DomainException>(() => line.ReceiveQuantity(quantity));
    }

    [Fact]
    public void ReceiveQuantity_ThrowsWhenExceedingQuantityOrdered()
    {
        var line = PurchaseOrderLine.Create(Guid.NewGuid(), Guid.NewGuid(), 10, 4.50m);

        Assert.Throws<DomainException>(() => line.ReceiveQuantity(11));
    }

    [Fact]
    public void ReceiveQuantity_ThrowsWhenCumulativeReceivedWouldExceedQuantityOrdered()
    {
        var line = PurchaseOrderLine.Create(Guid.NewGuid(), Guid.NewGuid(), 10, 4.50m);
        line.ReceiveQuantity(8);

        Assert.Throws<DomainException>(() => line.ReceiveQuantity(3));
    }

    [Fact]
    public void ReceiveQuantity_DoesNotChangeStateWhenRejected()
    {
        var line = PurchaseOrderLine.Create(Guid.NewGuid(), Guid.NewGuid(), 10, 4.50m);
        line.ReceiveQuantity(8);

        Assert.Throws<DomainException>(() => line.ReceiveQuantity(3));

        Assert.Equal(8, line.QuantityReceived);
    }

    [Fact]
    public void ReceiveQuantity_AllowsReceivingExactRemainingQuantity()
    {
        var line = PurchaseOrderLine.Create(Guid.NewGuid(), Guid.NewGuid(), 10, 4.50m);
        line.ReceiveQuantity(7);

        line.ReceiveQuantity(3);

        Assert.Equal(10, line.QuantityReceived);
    }
}
