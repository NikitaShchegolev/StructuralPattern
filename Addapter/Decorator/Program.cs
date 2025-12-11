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
            var decorator = new MessageManager(notificationOption);
            decorator.Issue();
            Console.ReadKey();
        }
    }
}
