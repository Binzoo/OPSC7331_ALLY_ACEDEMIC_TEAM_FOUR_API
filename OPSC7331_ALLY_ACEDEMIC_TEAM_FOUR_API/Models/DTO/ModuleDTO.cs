using System;
using System.ComponentModel.DataAnnotations;

namespace OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Models.DTO;

public class ModuleDTO
{
    [Required]
    public string? ModuleName { get; set; }
    [Required]
    public int Credits { get; set; }
    [Required]
    public int DegreeID { get; set; }
}
