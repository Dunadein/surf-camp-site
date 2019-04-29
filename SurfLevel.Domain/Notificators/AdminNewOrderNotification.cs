using ServerSideTemplates.Interfaces;
using ServerSideTemplates.TemplateType;
using SurfLevel.Contracts.Models.Notificators;
using SurfLevel.Contracts.Notificators;
using SurfLevel.Domain.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SurfLevel.Domain.Notificators
{
    public class AdminNewOrderNotification : Notificator<AdminNotification>
    {
        private readonly IMailNotification _mailer;
        private readonly ITemplateService _templates;

        public AdminNewOrderNotification(IMailNotification mailNotification, ITemplateService templateService)
        {
            _mailer = mailNotification;
            _templates = templateService;
        }

        public override async Task SendNotificationAsync(AdminNotification notification)
        {
            var bodyTemplate = await _templates.GetTemplate("admin", NotificationTemplateType.AdminNewOrder);

            var id = notification.Order.GenerateName();
            var replacement = new Dictionary<string, string>
            {
                [nameof(id)] = id,
                [nameof(notification.Order.DateFrom)] = notification.Order.DateFrom.ToShortDateString()
            };

            var body = PrepareBody(bodyTemplate, replacement);

            await _mailer.SendMailAsync(notification.Recipient, "New Booking created!", body);
        }
    }
}
