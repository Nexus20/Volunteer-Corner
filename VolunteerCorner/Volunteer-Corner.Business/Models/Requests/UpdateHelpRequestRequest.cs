using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volunteer_Corner.Business.Models.Requests
{
    public class UpdateHelpRequestRequest
    {
        [Required]
        public string OwnerId { get; set; } = null!;
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Location { get; set; } = null!;
        public string? Description { get; set; }
    }
}
