using System;

namespace OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Models.DTO;

public class HomeWorkDTO
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int ModuleID { get; set; }
    public DateTime DueDate { get; set; }

    public DateTime LastUpdated { get; set; } // New field for tracking updates

}
