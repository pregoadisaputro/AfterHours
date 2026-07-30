using AfterHours.Data.Enum;
using AfterHours.Services.Tmdb.Dto;

namespace AfterHours.Services.Tmdb;

public interface ITmdbService
{
    Task<TmdbDetailsResponse?> GetDetailsAsync(
        MediaType mediaType,
        int id,
        CancellationToken ct = default
    );
    Task<TmdbSearchResponse?> SearchAsync(
        MediaType mediaType,
        string query,
        int page,
        CancellationToken ct = default
    );
}
