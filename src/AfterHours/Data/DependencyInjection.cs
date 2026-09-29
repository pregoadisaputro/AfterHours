using Microsoft.EntityFrameworkCore;

namespace AfterHours.Data;

public static class DependencyInjection
{
    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddDbContextFactory<AppDbContext>(options =>
        {
            options.UseSqlite(configuration.GetConnectionString("Default"));
        });

        return services;
    }
}
