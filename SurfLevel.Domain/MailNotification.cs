using Microsoft.Extensions.Options;
using SurfLevel.Contracts.Notificators;
using SurfLevel.Domain.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SurfLevel.Domain
{
    public class MailNotification : IMailNotification
    {
        private readonly SMTPOptions _options;

        public MailNotification(IOptions<SMTPOptions> options)
        {
            _options = options.Value;

            if (_options == null)
                throw new NotImplementedException("Mail options weren't provided.");
        }

        public async Task SendMailAsync(IEnumerable<string> recipients, string subject, string body, IEnumerable<string> copyTo = null)
        {
            var client = new SmtpClient(_options.Host, _options.Port)
            {
                EnableSsl = true,
                Timeout = 15000,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(_options.Credentials.From, _options.Credentials.Password)
            };

            var message = new MailMessage()
            {
                From = new MailAddress(_options.Credentials.From),
                IsBodyHtml = true,
                Subject = subject,
                Body = body                
            };

            recipients.All(p => { message.To.Add(p); return true; });

            copyTo?.All(p => { message.Bcc.Add(p); return true; });

            client.SendCompleted += (e, t) => {
                client.Dispose();
                message.Dispose();
            };
                        
            await client.SendMailAsync(message);
        }
    }
}
