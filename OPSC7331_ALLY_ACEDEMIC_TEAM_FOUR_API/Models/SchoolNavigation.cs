using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Models;
public class SchoolNavigation
{
    public int Id { get; set; }
    public string? StartPlace { get; set; }
    public string? EndPlace { get; set; }
    public string? VideoUrl { get; set; }
}