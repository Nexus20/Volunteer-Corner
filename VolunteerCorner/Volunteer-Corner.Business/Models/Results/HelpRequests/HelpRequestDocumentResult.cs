using Volunteer_Corner.Business.Models.Results.Abstract;

namespace Volunteer_Corner.Business.Models.Results.HelpRequests;

public class HelpRequestDocumentResult : BaseResult
{
    public string HelpRequestId { get; set; } = null!;
    public string FilePath { get; set; } = null!;
}