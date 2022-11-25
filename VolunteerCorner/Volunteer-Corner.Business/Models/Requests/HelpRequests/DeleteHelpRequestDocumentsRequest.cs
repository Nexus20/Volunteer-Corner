using System.ComponentModel.DataAnnotations;

namespace Volunteer_Corner.Business.Models.Requests.HelpRequests;

public class DeleteHelpRequestDocumentsRequest
{
    [Required] public List<string> DocumentsIds { get; set; } = null!;
}