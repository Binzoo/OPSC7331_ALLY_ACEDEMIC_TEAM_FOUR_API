using System;

namespace OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Models.DTO;

public class AttendanceDTO
{
    public DateTime Date { get; set; }
    public string AttendanceStatus { get; set; }
    public string UserId { get; set; }
}
