using WarehouseERP.Domain.Sales;

namespace WarehouseERP.Domain.Tests.Sales.SalesOrderTests;

public class SalesOrderLinesEncapsulationTests
{
    [Fact]
    public void Lines_IsExposedAsReadOnlyCollection()
    {
        var salesOrder = SalesOrder.Create(Guid.NewGuid(), "SO-001", DateTime.UtcNow);
        salesOrder.AddLine(Guid.NewGuid(), 10, 4.50m);

        Assert.IsAssignableFrom<IReadOnlyCollection<SalesOrderLine>>(salesOrder.Lines);
    }

    [Fact]
    public void Lines_ThrowsWhenCallerAttemptsToMutateTheUnderlyingCollection()
    {
        var salesOrder = SalesOrder.Create(Guid.NewGuid(), "SO-001", DateTime.UtcNow);
        salesOrder.AddLine(Guid.NewGuid(), 10, 4.50m);

        var mutableView = Assert.IsAssignableFrom<ICollection<SalesOrderLine>>(salesOrder.Lines);
        var extraLine = SalesOrderLine.Create(salesOrder.Id, Guid.NewGuid(), 1, 1m);

        Assert.Throws<NotSupportedException>(() => mutableView.Add(extraLine));
        Assert.Single(salesOrder.Lines);
    }
}
