using Volunteer_Corner.Business.Models.Results.Abstract;
using Volunteer_Corner.Business.Models.Results.HelpSeekers;
using Volunteer_Corner.Data.Enums;

namespace Volunteer_Corner.Business.Models.Results.HelpRequests;

public class HelpRequestResult : BaseResult
{
    public HelpSeekerResult Owner { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Location { get; set; } = null!;
    public string? Description { get; set; }
    public HelpRequestStatus Status { get; set; }
    public List<HelpRequestDocumentResult>? AdditionalDocuments { get; set; }
}

public class HelpRequestWithHelpResponsesResult : HelpRequestResult
{
    public List<HelpRequestResponseResult>? Responses;
}