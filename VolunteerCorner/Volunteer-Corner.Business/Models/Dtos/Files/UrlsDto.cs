namespace Volunteer_Corner.Business.Models.Dtos.Files;

public class UrlsDto
{
    public UrlsDto(List<string> urls)
    {
        Urls = urls;
    }

    public List<string> Urls { get; }
}