using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Models
{
    public class MultiMedia
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? File { get; set; }
        public string? ModuleName { get; set; }
    }
}