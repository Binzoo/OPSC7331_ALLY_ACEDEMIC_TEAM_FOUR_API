using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Models
{
    public class ModuleClass
    {
        public int Id { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public DateTime Date { get; set; }
        public int ModuleId { get; set; }
        [JsonIgnore]
        public Module? Module { get; set; }
    }
}