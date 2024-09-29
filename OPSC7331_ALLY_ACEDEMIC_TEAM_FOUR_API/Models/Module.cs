using System;
using System.Text.Json.Serialization;

namespace OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Models;

public class Module
{
    public int ModuleID { get; set; }
    public string? ModuleName { get; set; }
    public int Credits { get; set; }
    public List<HomeWork> HomeWorks { get; set; }
    public List<ModuleClass> ModuleClasses { get; set; }
    // Foreign Key
    public int DegreeID { get; set; }
    [JsonIgnore]
    public Degree? Degree { get; set; }
}
