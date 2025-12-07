using System;
using System.Collections.Generic;
using System.Text;

using Decorator.ExemplesInterface;

namespace Decorator.Decorators
    {
        /// <summary>
        /// Базовый декоратор для сервиса уведомлений.
        /// </summary>
        public class BaseDecorator : INotyficationService
        {
            private readonly INotyficationService notyficationService;

            /// <summary>
            /// Инициализирует новый экземпляр класса BaseDecorator.
            /// </summary>
            /// <param name="notyficationService">Сервис уведомлений, который будет декорирован.</param>
            protected BaseDecorator(INotyficationService notyficationService)
            {
                this.notyficationService = notyficationService;
            }
            
            /// <summary>
            /// Отправляет уведомление.
            /// </summary>
            public virtual void Notify()
            {
                notyficationService.Notify();
            }
        }
    }
