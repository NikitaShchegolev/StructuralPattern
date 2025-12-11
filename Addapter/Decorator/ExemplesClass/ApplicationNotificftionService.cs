using Decorator.ExemplesInterface;

namespace Decorator.ExemplesClass
{
    internal class ApplicationNotificftionService : INotyficationService
    {
        void INotyficationService.Notify()
        {
            Console.WriteLine("Выполненн - ApplicationNotificftionService");
        }
    }
}