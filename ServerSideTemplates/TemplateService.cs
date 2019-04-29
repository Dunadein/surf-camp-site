using ServerSideTemplates.Interfaces;
using ServerSideTemplates.TemplateType;
using System.IO;
using System.Threading.Tasks;

namespace ServerSideTemplates
{
    public class TemplateService : ITemplateService
    {
        public TemplateService()
        {
        
        }

        public async Task<string> GetTemplate(string locale, NotificationTemplateType templateType)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), $"Templates/{locale}/{templateType.ToString()}.html");

            if (File.Exists(path))
            {
                using (var reader = File.OpenText(path))
                {
                    var template = await reader.ReadToEndAsync();

                    return template;
                }

            }

            return null;
        }
    }
}
