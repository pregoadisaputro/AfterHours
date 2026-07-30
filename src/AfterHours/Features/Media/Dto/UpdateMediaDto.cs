using AfterHours.Data.Enum;

namespace AfterHours.Features.Media.Dto;

public record UpdateMediaRequest(decimal? Rating, MediaStatus Status);
