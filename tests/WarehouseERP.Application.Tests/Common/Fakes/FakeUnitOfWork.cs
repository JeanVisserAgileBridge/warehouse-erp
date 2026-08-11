using WarehouseERP.Application.Common;

namespace WarehouseERP.Application.Tests.Common.Fakes;

public sealed class FakeUnitOfWork : IUnitOfWork
{
    public int SaveChangesCallCount { get; private set; }
    public CancellationToken? LastCancellationToken { get; private set; }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        SaveChangesCallCount++;
        LastCancellationToken = cancellationToken;
        return Task.CompletedTask;
    }
}
