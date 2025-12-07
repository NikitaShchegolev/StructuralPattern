using System;
using System.Collections.Generic;
using System.Text;

namespace Decorator.ExemplesInterface
    {
        /// <summary>
        /// Интерфейс сервиса уведомлений.
        /// </summary>
        public interface INotyficationService
        {
            /// <summary>
            /// Отправляет уведомление.
            /// </summary>
            void Notify();
        }
    }
