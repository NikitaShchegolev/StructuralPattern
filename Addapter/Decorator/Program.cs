using Decorator.ExemplesClass;

namespace Decorator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var notificationOption = new NotyficationOption() 
            {
                SendFBNotification = true,
                SendToMail = true,
                SendToSms = true
            };

            var decorator = new CardManager(notificationOption);
            decorator.Issue();
            FileDecorator.Write();
            Console.WriteLine("Создан txt...");
            FileDecorator.WriteArchived();
            Console.WriteLine("Заархивирован txt...");
            Console.ReadKey();

        }
    }
}
