namespace AfterHours.Features.Media;

public static class DependencyInjection
{
    public static IServiceCollection AddMediaFeature(this IServiceCollection services)
    {
        services.AddScoped<CreateMedia>();
        services.AddScoped<UpdateMedia>();
        services.AddScoped<DeleteMedia>();

        services.AddScoped<GetMediaDetails>();
        services.AddScoped<GetMedias>();

        services.AddScoped<GetDashboardStats>();

        return services;
    }
}
