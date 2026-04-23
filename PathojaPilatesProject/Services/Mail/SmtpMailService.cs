using CoreLayer.Utilities.Mail;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace PathojaPilatesProject.Services.Mail
{
    public class SmtpMailService : IMailService
    {
        private readonly MailSettings _settings;

        public SmtpMailService(IOptions<MailSettings> options)   
        {
            _settings = options.Value;
        }

        public async Task SendMailAsync(MailRequest request, CancellationToken cancellationToken = default)
        {
            using var message = new MailMessage
            {
                From = new MailAddress(_settings.From, _settings.FromName),
                Subject = request.Subject,
                Body = request.Body,
                IsBodyHtml = request.IsHtml
            };

            message.To.Add(request.To);

            using var smtp = new SmtpClient
            {
                Host = _settings.Host,              // smtp.gmail.com
                Port = _settings.Port,              // 587
                EnableSsl = _settings.EnableSsl,    // true
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,      // ÖNEMLİ
                Credentials = new NetworkCredential(
            _settings.UserName,             // aybuke.onay7@gmail.com
            _settings.Password              // gsrdwgvogyvvrif
        )
            };

                await smtp.SendMailAsync(message);

        }
    }
}
