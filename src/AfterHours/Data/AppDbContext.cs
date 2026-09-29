using AfterHours.Data.Entity;
using AfterHours.Data.Enum;
using Microsoft.EntityFrameworkCore;

namespace AfterHours.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<MediaItem> MediaItems => Set<MediaItem>();

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder.Properties<MediaStatus>().HaveConversion<string>();
        configurationBuilder.Properties<MediaType>().HaveConversion<string>();
    }
}
