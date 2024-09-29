using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Models.DTO
{
    public class ModuleClassDTO
    {
        [Required]
        public string StartTime { get; set; } // Use string to accept time in "HH:mm" format
        [Required]
        public string EndTime { get; set; }   // Use string to accept time in "HH:mm" format
        [Required]
        public int ModuleId { get; set; }
        public DateTime Date { get; set; } // Use string to accept date in "yyyy-MM-dd" format
    }

}