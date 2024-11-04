namespace OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Interface
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}
