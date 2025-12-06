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
    public class CardManager
    {
        private CardIssueService _cardIssueService;
        private NotyficationOption _notyficationOption;
        public CardManager(NotyficationOption notyficationOption)
        {
            _cardIssueService = new CardIssueService();
            _notyficationOption = notyficationOption;
        }
        public void Issue()
        {
            _cardIssueService.Issue(new Message());
            INotyficationService notyficationService = new ApplicationNotificftionService();

            ArrayList notificationTypes = new ArrayList();
            //Добавляем в cписок
            notificationTypes.Add(NotificationType.Facebook);
            notificationTypes.Add(NotificationType.Sms);
            notificationTypes.Add(NotificationType.Email);

            for (int i = 0; i < notificationTypes.Count; i++)
            {
                NotificationType notificationType;
                bool success = Enum.TryParse<NotificationType>(notificationTypes[i].ToString(), out notificationType);

                switch (success)//проверка преобразования к типу Enum
                {
                    case true:
                        switch (notificationType)
                        {
                            case NotificationType.Facebook:
                                notyficationService = new FaceBookDecorator(notyficationService);
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
