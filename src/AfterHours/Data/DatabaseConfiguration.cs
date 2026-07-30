using Microsoft.EntityFrameworkCore;

namespace AfterHours.Data;

public static class DatabaseConfiguration
{
    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlite(configuration.GetConnectionString("Default"));
        });

        return services;
    }
}
