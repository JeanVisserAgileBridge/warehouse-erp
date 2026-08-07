namespace WarehouseERP.Blazor.Configuration;

public sealed class ApiOptions
{
    public const string SectionName = "ApiSettings";

    public required string BaseUrl { get; init; }
}
