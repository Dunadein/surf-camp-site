﻿using ServerSideTemplates.Interfaces;
using ServerSideTemplates.TemplateType;
using SurfLevel.Contracts.Models.Notificators;
using SurfLevel.Contracts.Notificators;
using SurfLevel.Domain.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SurfLevel.Domain.Notificators
{
    public class OrderPayedNotificatior : Notificator<ClientOrderPayedNotification>
    {
        private readonly IMailNotification _mailer;
        private readonly ITemplateService _templates;

        public OrderPayedNotificatior(IMailNotification mailNotification, ITemplateService templateService)
        {
            _mailer = mailNotification;
            _templates = templateService;
        }

        public override async Task SendNotificationAsync(ClientOrderPayedNotification notification)
        {
            var bodyTemplate = await _templates.GetTemplate(notification.Order.Locale, NotificationTemplateType.OrderPayed);

            var id = notification.Order.GenerateName();
            var replacement = new Dictionary<string, string>
            {
                ["{id}"] = id,
                ["{dateFrom}"] = notification.Order.DateFrom.ToShortDateString()
            };

            var body = PrepareBody(bodyTemplate, replacement);

            var footer = await _templates.GetTemplate(notification.Order.Locale, NotificationTemplateType.Footage);

            var subject = string.Concat(notification.Order.Locale == "ru" ? "Бронь" : "Order", $" № {id}");

            await _mailer.SendMailAsync(notification.Recipient, subject, string.Concat(body, footer));
        }
    }
}
