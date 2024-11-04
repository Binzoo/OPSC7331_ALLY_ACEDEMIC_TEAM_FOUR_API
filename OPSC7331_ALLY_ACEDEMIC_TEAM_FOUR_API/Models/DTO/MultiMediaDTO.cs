using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Models.DTO
{
    public class MultiMediaDTO
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public IFormFile File { get; set; }
        [Required]
        public string ModuleName { get; set; }
    }
}