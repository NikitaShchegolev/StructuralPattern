using Decorator.Decorators;
using Decorator.ExemplesInterface;

namespace Decorator.Messages
{
    public class SmsNotificationDecorator : BaseDecorator
    {
        //1. public EmailNotificationDecorator(...) - это объявление конструктора класса EmailNotificationDecorator.Конструктор имеет такое же имя, как и класс.

        //2. (INotyficationService notyficationService) - параметр конструктора.Он принимает объект, реализующий интерфейс INotyficationService.Это позволяет передать в декоратор существующий сервис уведомлений, который он будет "оборачивать".

        //3. : base(notyficationService) - вызов конструктора базового класса. Это означает, что параметр notyficationService передается в конструктор родительского класса (вероятно, абстрактного класса декоратора).

        //4. Пустое тело { } - в данном случае конструктор не выполняет никакой дополнительной логики, кроме передачи параметра в базовый класс.

        //Такой паттерн используется в паттерне "Декоратор", где:

        //У вас есть базовый интерфейс (INotyficationService)
        //Конкретные реализации этого интерфейса
        //Декораторы, которые "оборачивают" базовый сервис, добавляя дополнительную функциональность

        public SmsNotificationDecorator(INotyficationService notyficationService) : base(notyficationService) { }

        public override void Notify()
        {
            base.Notify();

            Console.WriteLine("Выполнен - SmsNotificationDecorator");
        }
    }
}