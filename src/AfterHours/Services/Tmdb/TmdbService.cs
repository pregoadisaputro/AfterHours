using AfterHours.Services.Tmdb.Dto;
using Microsoft.Extensions.Caching.Memory;

namespace AfterHours.Services.Tmdb;

public sealed class TmdbService(HttpClient client, IMemoryCache cache)
{
    private static readonly TimeSpan DetailsDuration = TimeSpan.FromHours(6);

    public async Task<TmdbMovieDetailsResponse?> GetMovieDetailsAsync(
        int id,
        CancellationToken ct = default
    )
    {
        var key = $"tmdb:movie:{id}";

        if (cache.TryGetValue(key, out TmdbMovieDetailsResponse? cached))
        {
            return cached;
        }

        var movie = await client.GetFromJsonAsync<TmdbMovieDetailsResponse>($"movie/{id}", ct);

        if (movie is not null)
        {
            cache.Set(key, movie, DetailsDuration);
        }

        return movie;
    }

    public async Task<TmdbTvDetailsResponse?> GetTvDetailsAsync(
        int id,
        CancellationToken ct = default
    )
    {
        var key = $"tmdb:tv:{id}";

        if (cache.TryGetValue(key, out TmdbTvDetailsResponse? cached))
        {
            return cached;
        }

        var tv = await client.GetFromJsonAsync<TmdbTvDetailsResponse>($"tv/{id}", ct);

        if (tv is not null)
        {
            cache.Set(key, tv, DetailsDuration);
        }

        return tv;
    }

    public async Task<TmdbSearchResponse?> SearchAsync(
        string query,
        int page = 1,
        CancellationToken ct = default
    )
    {
        var url = $"search/multi?query={Uri.EscapeDataString(query)}&page={page}";

        var result = await client.GetFromJsonAsync<TmdbSearchResponse>(url, ct);

        if (result is null)
        {
            return null;
        }

        return new TmdbSearchResponse(
            result.Page,
            result.TotalPages,
            result.TotalResults,
            result.Results.Where(x => x.MediaType == "movie" || x.MediaType == "tv").ToList()
        );
    }
}
