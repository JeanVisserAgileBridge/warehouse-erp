using WarehouseERP.Domain.Exceptions;
using WarehouseERP.Domain.Sales;

namespace WarehouseERP.Domain.Tests.Sales.SalesOrderTests;

public class SalesOrderCreateTests
{
    [Fact]
    public void Create_ReturnsSalesOrderWithNonEmptyGuid()
    {
        var salesOrder = SalesOrder.Create(Guid.NewGuid(), "SO-001", DateTime.UtcNow);

        Assert.NotEqual(Guid.Empty, salesOrder.Id);
    }

    [Fact]
    public void Create_StoresSuppliedValues()
    {
        var customerId = Guid.NewGuid();
        var orderDate = new DateTime(2026, 1, 15, 0, 0, 0, DateTimeKind.Utc);

        var salesOrder = SalesOrder.Create(customerId, "SO-001", orderDate, "Deliver on weekday");

        Assert.Equal(customerId, salesOrder.CustomerId);
        Assert.Equal("SO-001", salesOrder.OrderNumber);
        Assert.Equal(orderDate, salesOrder.OrderDate);
        Assert.Equal("Deliver on weekday", salesOrder.Notes);
    }

    [Fact]
    public void Create_StartsInDraftStatus()
    {
        var salesOrder = SalesOrder.Create(Guid.NewGuid(), "SO-001", DateTime.UtcNow);

        Assert.Equal(SalesOrderStatus.Draft, salesOrder.Status);
    }

    [Fact]
    public void Create_StartsWithNoLines()
    {
        var salesOrder = SalesOrder.Create(Guid.NewGuid(), "SO-001", DateTime.UtcNow);

        Assert.Empty(salesOrder.Lines);
    }

    [Fact]
    public void Create_SetsCreatedAtAndUpdatedAtToSameValue()
    {
        var salesOrder = SalesOrder.Create(Guid.NewGuid(), "SO-001", DateTime.UtcNow);

        Assert.Equal(salesOrder.CreatedAt, salesOrder.UpdatedAt);
    }

    [Fact]
    public void Create_AcceptsNullNotes()
    {
        var salesOrder = SalesOrder.Create(Guid.NewGuid(), "SO-001", DateTime.UtcNow);

        Assert.Null(salesOrder.Notes);
    }

    [Fact]
    public void Create_RejectsEmptyCustomerId()
    {
        Assert.Throws<DomainException>(() => SalesOrder.Create(Guid.Empty, "SO-001", DateTime.UtcNow));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_RejectsNullEmptyOrWhitespaceOrderNumber(string? orderNumber)
    {
        Assert.Throws<DomainException>(() => SalesOrder.Create(Guid.NewGuid(), orderNumber!, DateTime.UtcNow));
    }

    [Fact]
    public void Create_RejectsOrderNumberLongerThanMaxLength()
    {
        var orderNumber = new string('a', SalesOrder.MaxOrderNumberLength + 1);

        Assert.Throws<DomainException>(() => SalesOrder.Create(Guid.NewGuid(), orderNumber, DateTime.UtcNow));
    }

    [Fact]
    public void Create_AcceptsOrderNumberAtMaxLength()
    {
        var orderNumber = new string('a', SalesOrder.MaxOrderNumberLength);

        var salesOrder = SalesOrder.Create(Guid.NewGuid(), orderNumber, DateTime.UtcNow);

        Assert.Equal(orderNumber, salesOrder.OrderNumber);
    }

    [Fact]
    public void Create_RejectsNotesLongerThanMaxLength()
    {
        var notes = new string('a', SalesOrder.MaxNotesLength + 1);

        Assert.Throws<DomainException>(() => SalesOrder.Create(Guid.NewGuid(), "SO-001", DateTime.UtcNow, notes));
    }

    [Fact]
    public void Create_AcceptsNotesAtMaxLength()
    {
        var notes = new string('a', SalesOrder.MaxNotesLength);

        var salesOrder = SalesOrder.Create(Guid.NewGuid(), "SO-001", DateTime.UtcNow, notes);

        Assert.Equal(notes, salesOrder.Notes);
    }
}
