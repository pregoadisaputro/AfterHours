using AfterHours.Data.Enum;

namespace AfterHours.Data.Entity;

public class MediaItem
{
    public int Id { get; set; }
    public int ExternalId { get; set; }

    public decimal? Rating { get; set; }
    public MediaStatus MediaStatus { get; set; }

    public string? Title { get; set; }
    public MediaType MediaType { get; set; }
    public string? Overview { get; set; }
    public string? PosterPath { get; set; }
    public string? BackdropPath { get; set; }
    public DateOnly? ReleaseDate { get; set; }
    public int? Runtime { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
