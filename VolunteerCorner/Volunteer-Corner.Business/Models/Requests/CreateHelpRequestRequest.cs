﻿using System.ComponentModel.DataAnnotations;

namespace Volunteer_Corner.Business.Models.Requests;

public class CreateHelpRequestRequest
{
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string Location { get; set; } = null!;
    public string? Description { get; set; }
}