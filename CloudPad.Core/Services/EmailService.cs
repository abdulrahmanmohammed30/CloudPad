using System.ComponentModel.DataAnnotations;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using CloudPad.Core.Configurations;
using CloudPad.Core.Dtos;
using CloudPad.Core.Exceptions;
using CloudPad.Core.ServiceContracts;

public class EmailService(IOptions<SmtpSettings> smtpSettings) : IEmailService
{
    private readonly SmtpSettings _smtpSettings = smtpSettings.Value;

    public async Task SendEmailAsync(EmailRequest emailRequest)
    {
        var context = new ValidationContext(emailRequest);
        var errors = new List<ValidationResult>();

        if (Validator.TryValidateObject(emailRequest, context, errors, true) == false)
        {
            throw new InvalidateEmailRequestException(errors.FirstOrDefault()?.ErrorMessage ?? "Invalid email request");
        }
        
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.SenderEmail));
        email.To.Add(new MailboxAddress(emailRequest.To, emailRequest.To));
        email.Subject = emailRequest.Subject;

        var bodyBuilder = new BodyBuilder();

        if (emailRequest.IsBodyHtml)
        {
            bodyBuilder.HtmlBody = emailRequest.Body;
        }
        else
        {
            bodyBuilder.TextBody = emailRequest.Body;
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
