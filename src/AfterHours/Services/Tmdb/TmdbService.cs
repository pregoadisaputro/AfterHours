using AfterHours.Services.Tmdb.Dto;

namespace AfterHours.Services.Tmdb;

public sealed class TmdbService(HttpClient client)
{
    public async Task<TmdbMovieDetailsResponse?> GetMovieDetailsAsync(
        int id,
        CancellationToken ct = default
    ) => await client.GetFromJsonAsync<TmdbMovieDetailsResponse>($"movie/{id}", ct);

    public async Task<TmdbTvDetailsResponse?> GetTvDetailsAsync(
        int id,
        CancellationToken ct = default
    ) => await client.GetFromJsonAsync<TmdbTvDetailsResponse>($"tv/{id}", ct);

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
