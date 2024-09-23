using System.ComponentModel.DataAnnotations;

namespace OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Models.DTO
{
    public class LoginDTO
    {
        [Required]
        public string Email { get; set; } = String.Empty;
        [Required]
        public string Password { get; set; } = String.Empty;
    }
}
