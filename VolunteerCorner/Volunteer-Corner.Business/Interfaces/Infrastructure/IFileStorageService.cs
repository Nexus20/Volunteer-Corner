using Volunteer_Corner.Business.Models.Dtos.Files;

namespace Volunteer_Corner.Business.Interfaces.Infrastructure;

public interface IFileStorageService
{
    Task<UrlsDto> UploadAsync(List<FileDto> files);
    Task<bool> DeleteAsync(UrlsDto urls);
}