using NUnit.Framework;
using ServerSideTemplates;
using ServerSideTemplates.Interfaces;
using ServerSideTemplates.TemplateType;
using System.Threading.Tasks;

namespace Template.Test
{
    public class TemplateServiceTest
    {
        private ITemplateService _template;

        [SetUp]
        public void Setup()
        {
            _template = new TemplateService();
        }

        [Test]
        public async Task Assure_RU_Template_Readed()
        {
            var result = await _template.GetTemplate("ru", NotificationTemplateType.NewOrderCreated);

            Assert.NotNull(result);
        }
    }
}