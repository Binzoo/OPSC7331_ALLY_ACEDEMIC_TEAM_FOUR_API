namespace OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Models
{
    public class PasswordReset
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Code { get; set; }
        public DateTime ExpiryTime { get; set; }
    }
}
