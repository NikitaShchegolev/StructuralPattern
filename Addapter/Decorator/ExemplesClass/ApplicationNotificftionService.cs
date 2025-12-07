using System;
using System.Collections.Generic;
using System.Text;
using Decorator.ExemplesInterface;

namespace Decorator.ExemplesClass
{
    /// <summary>
    /// Реализация сервиса уведомлений через приложение.
    /// </summary>
    public class ApplicationNotificftionService : INotyficationService
    {
        /// <summary>
        /// Отправляет уведомление через консоль.
        /// </summary>
        public void Notify()
        {
            Console.WriteLine("Выполнен - ApplicationNotificftionService");
        }
    }
}
