using System.ComponentModel.DataAnnotations;

namespace OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Models.DTO
{
    public class ForgotPasswrodDTO
    {
        [Required]
        public string Email { get; set; } = string.Empty;
    }
}
