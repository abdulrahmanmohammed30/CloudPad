using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using CloudPad.Core.Configurations;
using CloudPad.Core.ServiceContracts;

public class EmailService : IEmailService
{
    private readonly SmtpSettings _smtpSettings;

    public EmailService(IOptions<SmtpSettings> smtpSettings)
    {
        _smtpSettings = smtpSettings.Value;
    }

    public async Task SendEmailAsync(string to, string subject, string body, bool isBodyHtml = false)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.SenderEmail));
        email.To.Add(new MailboxAddress(to, to));
        email.Subject = subject;

        var bodyBuilder = new BodyBuilder();

        if (isBodyHtml)
        {
            bodyBuilder.HtmlBody = body;
        }
        else
        {
            bodyBuilder.TextBody = body;
        }

        email.Body = bodyBuilder.ToMessageBody();

        using (var smtp = new SmtpClient())
        {
            try
            {
                await smtp.ConnectAsync(_smtpSettings.Server, _smtpSettings.Port, _smtpSettings.UseSsl ? MailKit.Security.SecureSocketOptions.StartTls : MailKit.Security.SecureSocketOptions.Auto);
                await smtp.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
                await smtp.SendAsync(email);
            }
            finally
            {
                await smtp.DisconnectAsync(true);
            }
        }

    }
}
