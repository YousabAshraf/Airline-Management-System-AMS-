using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailSender : IEmailSender
{
    private readonly IConfiguration _configuration;

    public EmailSender(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var smtpHost = _configuration["Email:SmtpHost"];
        var smtpPort = int.Parse(_configuration["Email:SmtpPort"]);
        var smtpUser = _configuration["Email:SmtpUser"];
        var smtpPass = _configuration["Email:SmtpPass"];

        var smtp = new SmtpClient(smtpHost)
        {
            Port = smtpPort,
            Credentials = new NetworkCredential(smtpUser, smtpPass),
            EnableSsl = true,
        };

        var mail = new MailMessage
        {
            From = new MailAddress(smtpUser),
            Subject = subject,
            Body = htmlMessage,
            IsBodyHtml = true,
        };
        mail.To.Add(email);

        return smtp.SendMailAsync(mail);
    }
}
