namespace WarehouseERP.Shared.Contracts.Dashboard;

public sealed class DashboardSummary
{
    public required int TotalCategories { get; init; }
    public required int ActiveCategories { get; init; }
    public required int TotalProducts { get; init; }
    public required int ActiveProducts { get; init; }
    public required int InactiveProducts { get; init; }
}
