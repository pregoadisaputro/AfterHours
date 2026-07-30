using System.Net.Http.Headers;

namespace AfterHours.Services.Tmdb;

public static class TmdbExtension
{
    public static IServiceCollection AddTmdbClient(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var tmdbToken =
            configuration["Tmdb:ApiKey"]
            ?? throw new InvalidOperationException("TMDB key is not configured.");

        services.AddHttpClient<TmdbServices>(client =>
        {
            client.BaseAddress = new Uri("https://api.themoviedb.org/3/");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                tmdbToken
            );
        });

        return services;
    }
}
