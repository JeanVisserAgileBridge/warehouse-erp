using System.Net.Http.Json;
using System.Text.Json;

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

        var problemDetails = await TryReadProblemDetailsAsync(response, cancellationToken).ConfigureAwait(false);

        var message = problemDetails?.Detail
            ?? problemDetails?.Title
            ?? "The request to the API failed.";

        throw new ApiException((int)response.StatusCode, message);
    }

    // Authentication/authorization failures (401/403) are short-circuited by the API's cookie
    // middleware before reaching any controller, so they carry no ProblemDetails body at all —
    // reading them as JSON would otherwise throw a JsonException instead of a clean ApiException.
    private static async Task<ProblemDetailsResponse?> TryReadProblemDetailsAsync(
        HttpResponseMessage response,
        CancellationToken cancellationToken)
    {
        if (response.Content.Headers.ContentLength is null or 0)
        {
            return null;
        }

        try
        {
            return await response.Content
                .ReadFromJsonAsync<ProblemDetailsResponse>(cancellationToken)
                .ConfigureAwait(false);
        }
        catch (JsonException)
        {
            return null;
        }
    }

    private sealed class ProblemDetailsResponse
    {
        public string? Title { get; init; }
        public string? Detail { get; init; }
    }
}
