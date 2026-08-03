using System.Net.Http.Headers;
using Microsoft.Extensions.Options;

namespace AfterHours.Services.Tmdb;

public static class TmdbExtension
{
    public static IServiceCollection AddTmdbClient(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services
            .AddOptions<TmdbOptions>()
            .Bind(configuration.GetSection(TmdbOptions.Name))
            .Validate(o => !string.IsNullOrWhiteSpace(o.ApiKey), "TMDB Api Key is not configured.")
            .ValidateOnStart();

        services.AddHttpClient<TmdbService>(
            (sp, client) =>
            {
                var options = sp.GetRequiredService<IOptions<TmdbOptions>>().Value;

                client.BaseAddress = new Uri(options.BaseUrl);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer",
                    options.ApiKey
                );
            }
        );

        return services;
    }
}
