using Volunteer_Corner.Data.Enums;

namespace Volunteer_Corner.Business.Models.Results.HelpRequests;

public class HelpRequestResult
{
    public HelpSeekerResult Owner { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public HelpRequestStatus Status { get; set; }
    public List<HelpRequestDocumentResult>? AdditionalDocuments { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}