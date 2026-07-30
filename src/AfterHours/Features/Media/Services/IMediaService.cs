using AfterHours.Data.Enum;
using AfterHours.Features.Media.Dto;

namespace AfterHours.Features.Media.Services;

public interface IMediaService
{
    Task<GetDetailsResponse?> GetDetailsAsync(
        MediaType mediaType,
        int externalId,
        CancellationToken ct = default
    );

    Task CreateAsync(CreateMediaRequest request, CancellationToken ct = default);
    Task UpdateAsync(int id, UpdateMediaRequest request, CancellationToken ct = default);

    Task DeleteAsync(int id, CancellationToken ct = default);
}
