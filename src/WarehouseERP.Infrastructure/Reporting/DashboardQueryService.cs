using Dapper;
using Microsoft.Data.SqlClient;
using WarehouseERP.Application.Reporting.Dashboard;

namespace WarehouseERP.Infrastructure.Reporting;

public class DashboardQueryService : IDashboardQueryService
{
    private const string SummarySql = """
        SELECT
            (SELECT COUNT(*) FROM Categories) AS TotalCategories,
            (SELECT COUNT(*) FROM Categories WHERE IsActive = 1) AS ActiveCategories,
            (SELECT COUNT(*) FROM Products) AS TotalProducts,
            (SELECT COUNT(*) FROM Products WHERE IsActive = 1) AS ActiveProducts,
            (SELECT COUNT(*) FROM Products WHERE IsActive = 0) AS InactiveProducts;
        """;

    private readonly string _connectionString;

    public DashboardQueryService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<DashboardSummary> GetSummaryAsync(CancellationToken cancellationToken)
    {
        using var connection = new SqlConnection(_connectionString);

        var command = new CommandDefinition(SummarySql, cancellationToken: cancellationToken);

        return await connection.QuerySingleAsync<DashboardSummary>(command);
    }
}
