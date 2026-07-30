using System.Net;
using AfterHours.Data.Enum;
using AfterHours.Services.Tmdb.Dto;

namespace AfterHours.Services.Tmdb;

public sealed class TmdbService(HttpClient client) : ITmdbService
{
    public async Task<TmdbDetailsResponse?> GetDetailsAsync(
        MediaType mediaType,
        int id,
        CancellationToken ct = default
    )
    {
        var type = mediaType.ToString().ToLowerInvariant();

        var response = await client.GetAsync($"{type}/{id}", ct);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<TmdbDetailsResponse>(ct);
    }

    public async Task<TmdbSearchResponse?> SearchAsync(
        MediaType mediaType,
        string query,
        int page,
        CancellationToken ct = default
    )
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(page, 1);

        var type = mediaType.ToString().ToLowerInvariant();

        var response = await client.GetAsync(
            $"search/{type}?query={Uri.EscapeDataString(query)}&page={page}",
            ct
        );

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<TmdbSearchResponse>(ct);
    }
}
