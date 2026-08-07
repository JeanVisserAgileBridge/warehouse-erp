using System.Net.Http.Json;

namespace WarehouseERP.Blazor.Infrastructure.Http;

public static class HttpResponseMessageExtensions
{
    public static async Task EnsureSuccessOrThrowApiExceptionAsync(
        this HttpResponseMessage response,
        CancellationToken cancellationToken = default)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var problemDetails = await response.Content
            .ReadFromJsonAsync<ProblemDetailsResponse>(cancellationToken)
            .ConfigureAwait(false);

        var message = problemDetails?.Detail
            ?? problemDetails?.Title
            ?? "The request to the API failed.";

        throw new ApiException((int)response.StatusCode, message);
    }

    private sealed class ProblemDetailsResponse
    {
        public string? Title { get; init; }
        public string? Detail { get; init; }
    }
}
