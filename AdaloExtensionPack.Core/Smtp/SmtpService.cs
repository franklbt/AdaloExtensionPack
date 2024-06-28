using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace AdaloExtensionPack.Core.Smtp;

public class SmtpService(IOptions<SmtpOptions> configuration) : ISmtpService
{
    private readonly SmtpOptions _configuration = configuration.Value;

    public async Task SendEmail(string body, string subject, string to)
    {
        var client = new SmtpClient(_configuration.SmtpServer, _configuration.SmtpPort)
        {
            UseDefaultCredentials = false,
            EnableSsl = true,
            Credentials = new NetworkCredential(_configuration.SmtpUsername, _configuration.SmtpPassword)
        };
            
        var mailMessage = new MailMessage
        {
            From = new MailAddress(_configuration.SmtpFrom, _configuration.SmtpDisplayName),
            Body = body,
            Subject = subject,
            IsBodyHtml = false
        };

        mailMessage.To.Add(to);
        await client.SendMailAsync(mailMessage);
    }
}