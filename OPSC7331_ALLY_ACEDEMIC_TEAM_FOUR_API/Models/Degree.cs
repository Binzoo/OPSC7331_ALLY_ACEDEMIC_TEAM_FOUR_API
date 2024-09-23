using System;
using System.Text.Json.Serialization;

namespace OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Models;

public class Degree
{
    public int DegreeID { get; set; }
    public string? DegreeName { get; set; }

    // Navigation Property: One degree can have many modules
    public List<Module>? Modules { get; set; }
}
