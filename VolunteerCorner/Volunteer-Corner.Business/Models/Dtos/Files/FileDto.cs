using Volunteer_Corner.Business.Extensions;

namespace Volunteer_Corner.Business.Models.Dtos.Files;

public class FileDto
{
    public Stream Content { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    
    public string GetPathWithFileName()
    {
        var uniqueRandomFileName = Path.GetRandomFileName();
        var shortClientSideFileNameWithoutExtension = Path.GetFileNameWithoutExtension(Name).TruncateLongString(10);
        var fileExtension = Path.GetExtension(Name);
        var basePath = "documents/";

        return basePath + uniqueRandomFileName + "_" + shortClientSideFileNameWithoutExtension + fileExtension;
    }
}