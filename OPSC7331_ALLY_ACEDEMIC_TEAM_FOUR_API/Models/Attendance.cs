using System;
using OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Data;

namespace OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Models;

public class Attendance
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string AttendanceStatus { get; set; }
    public string UserId { get; set; }
    public AppUser User { get; set; }
}
