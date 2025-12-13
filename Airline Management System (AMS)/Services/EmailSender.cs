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
        var smtpHost = _configuration["SmtpSettings:SmtpServer"];
        var smtpPort = int.Parse(_configuration["SmtpSettings:SmtpPort"]);
        var smtpUser = _configuration["SmtpSettings:SmtpUsername"];
        var smtpPass = _configuration["SmtpSettings:SmtpPassword"];
        var senderEmail = _configuration["SmtpSettings:SenderEmail"];

        var smtp = new SmtpClient(smtpHost)
        {
            Port = smtpPort,
            Credentials = new NetworkCredential(smtpUser, smtpPass),
            EnableSsl = true, // Brevo SMTP requires SSL/TLS
        };

        var mail = new MailMessage
        {
            From = new MailAddress(senderEmail, "Airline Management System"),
            Subject = subject,
            Body = htmlMessage,
            IsBodyHtml = true,
        };

        mail.To.Add(email);

        return smtp.SendMailAsync(mail);
    }
}
