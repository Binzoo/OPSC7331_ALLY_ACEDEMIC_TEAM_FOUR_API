using System.ComponentModel.DataAnnotations;

namespace OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Models.DTO
{
    public class NoticeBoardDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
        public IFormFile Image { get; set; }
    }
}
