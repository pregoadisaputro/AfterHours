using AfterHours.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace AfterHours.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<MediaItem> MediaItems => Set<MediaItem>();
}
