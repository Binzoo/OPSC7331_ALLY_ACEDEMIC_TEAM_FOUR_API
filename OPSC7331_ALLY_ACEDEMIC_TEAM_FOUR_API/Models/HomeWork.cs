using System;
using System.Text.Json.Serialization;
using Microsoft.Identity.Client;

namespace OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Models;

public class HomeWork
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }

    public DateTime DueDate { get; set; }
    public int ModuleID { get; set; }
    [JsonIgnore]
    public Module Module { get; set; }
}
