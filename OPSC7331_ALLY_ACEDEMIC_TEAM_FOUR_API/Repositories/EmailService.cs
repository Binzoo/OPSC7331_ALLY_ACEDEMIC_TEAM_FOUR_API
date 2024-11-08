﻿using OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Interface;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using System.Net.Mail;
using MimeKit;

namespace OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Repositories
{
    public class EmailService : IEmailSender
    {
        private readonly EmailConfiguration _emailConfig;
        private readonly ILogger<EmailService> _logger;

        public EmailService(EmailConfiguration emailConfig, ILogger<EmailService> logger)
        {
            _emailConfig = emailConfig;
            _logger = logger;
        }
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_emailConfig.FromName, _emailConfig.FromAddress));
            message.To.Add(new MailboxAddress("", to));
            message.Subject = subject;

            message.Body = new TextPart("plain")
            {
                Text = body
            };

            using (var client = new SmtpClient())
            {
                try
                {
                    _logger.LogInformation("Connecting to SMTP server...");
                    await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, MailKit.Security.SecureSocketOptions.SslOnConnect);
                    _logger.LogInformation("Connected to SMTP server");

                    _logger.LogInformation("Authenticating...");
                    await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);
                    _logger.LogInformation("Authenticated");

                    _logger.LogInformation("Sending email...");
                    await client.SendAsync(message);
                    _logger.LogInformation("Email sent successfully");

                    await client.DisconnectAsync(true);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while sending email");
                    throw;
                }
            }
        }
    }

    public class EmailConfiguration
    {
        public string FromName { get; set; }
        public string FromAddress { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public bool UseSsl { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
