using System.Collections.Generic;
using System.Threading.Tasks;

namespace SurfLevel.Contracts.Notificators
{
    public interface IMailNotification
    {
        Task SendMailAsync(IEnumerable<string> recipients, string subject, string body, IEnumerable<string> copyTo = null);
    }
}
