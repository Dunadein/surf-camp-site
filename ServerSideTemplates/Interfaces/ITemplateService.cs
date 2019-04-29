using ServerSideTemplates.TemplateType;
using System.Threading.Tasks;

namespace ServerSideTemplates.Interfaces
{
    public interface ITemplateService
    {
        Task<string> GetTemplate(string locale, NotificationTemplateType templateType);
    }
}
