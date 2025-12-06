using System;
using System.Collections.Generic;
using System.Text;
using Decorator.ExemplesInterface;

namespace Decorator.ExemplesClass
{
    public class ApplicationNotificftionService : INotyficationService
    {
        public void Notify()
        {
            Console.WriteLine("Выполнен - ApplicationNotificftionService");
        }
    }
}
