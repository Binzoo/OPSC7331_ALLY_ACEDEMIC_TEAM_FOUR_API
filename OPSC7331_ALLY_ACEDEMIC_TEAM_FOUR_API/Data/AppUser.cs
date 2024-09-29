using Microsoft.AspNetCore.Identity;

namespace OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Data
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; } = String.Empty;
        public string LastName { get; set; } = String.Empty;
        public string College { get; set; } = string.Empty;
        public string Degree { get; set; } = string.Empty;
        public string UserImageUrl { get; set; } = string.Empty;
    }
}
