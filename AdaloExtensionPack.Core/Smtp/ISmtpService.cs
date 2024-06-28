using System.Threading.Tasks;

namespace AdaloExtensionPack.Core.Smtp;

public interface ISmtpService
{
    Task SendEmail(string body, string subject, string to);
}