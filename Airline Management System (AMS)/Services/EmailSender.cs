using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var smtp = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential("notifications.ams.1@gmail.com", "qmps rgby crle dzgz"),
            EnableSsl = true,
        };

        var mail = new MailMessage
        {
            From = new MailAddress("notifications.ams.1@gmail.com"),
            Subject = subject,
            Body = htmlMessage,
            IsBodyHtml = true,
        };
        mail.To.Add(email);

        return smtp.SendMailAsync(mail);
    }
}
