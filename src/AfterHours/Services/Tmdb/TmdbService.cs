using System.Net;
using AfterHours.Services.Tmdb.Dto;

namespace AfterHours.Services.Tmdb;

public sealed class TmdbService(HttpClient client) : ITmdbService
{
    public async Task<TmdbMovieDetailsResponse?> GetMovieDetailsAsync(
        int id,
        CancellationToken ct = default
    )
    {
        var response = await client.GetAsync($"movie/{id}", ct);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<TmdbMovieDetailsResponse>(ct);
    }

    public async Task<TmdbTvDetailsResponse?> GetTvDetailsAsync(
        int id,
        CancellationToken ct = default
    )
    {
        var response = await client.GetAsync($"tv/{id}", ct);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<TmdbTvDetailsResponse>(ct);
    }

    public async Task<TmdbSearchResponse?> SearchAsync(
        string query,
        int page = 1,
        CancellationToken ct = default
    )
    {
        var response = await client.GetAsync(
            $"search/multi?query={Uri.EscapeDataString(query)}&page={page}",
            ct
        );

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<TmdbSearchResponse>(ct);
    }
}
