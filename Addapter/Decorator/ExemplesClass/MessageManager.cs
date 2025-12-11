using System;
using System.Collections.Generic;
using System.Text;

using Decorator.ExemplesInterface;
using Decorator.ExemplesEnum;
using Decorator.ExemplesClass;
using System.Collections;
using Decorator.Messages;

namespace Decorator.ExemplesClass
{
    /// <summary>
    /// Менеджер для управления картами.
    /// </summary>
    public class MessageManager
    {
        private CardIssueService _cardIssueService;
        private NotyficationOption _notyficationOption;
        
        /// <summary>
        /// Инициализирует новый экземпляр класса CardManager.
        /// </summary>
        /// <param name="notyficationOption">Опции уведомлений.</param>
        public MessageManager(NotyficationOption notyficationOption)
        {
            _cardIssueService = new CardIssueService();
            _notyficationOption = notyficationOption;
        }
        
        /// <summary>
        /// Выпускает карту и отправляет уведомления.
        /// </summary>
        public void Issue()
        {
            _cardIssueService.Issue(new Message());
            INotyficationService notyficationService = new ApplicationNotificftionService();

            ArrayList notificationTypes = new ArrayList();
            //Добавляем в cписок
            notificationTypes.Add(NotificationType.Facebook);
            notificationTypes.Add(NotificationType.Sms);
            notificationTypes.Add(NotificationType.Email);

            foreach (object item in notificationTypes)
            {
                NotificationType notificationType;
                bool success = Enum.TryParse<NotificationType>(item.ToString(), out notificationType);

                switch (success)//проверка преобразования к типу Enum
                {
                    case true:
                        switch (notificationType)
                        {
                            case NotificationType.Facebook:
                                notyficationService = new FaceBookNotificationDecorator(notyficationService);
                                break;
                            case NotificationType.Sms:
                                notyficationService = new SmsNotificationDecorator(notyficationService);
                                break;
                            case NotificationType.Email:
                                notyficationService = new EmailNotificationDecorator(notyficationService);
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        throw new InvalidOperationException("Не удалось выполнить...");
                }

            }
            notyficationService.Notify();
        }
    }
}
