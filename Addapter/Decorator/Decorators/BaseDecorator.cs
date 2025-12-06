using System;
using System.Collections.Generic;
using System.Text;

using Decorator.ExemplesInterface;

namespace Decorator.Decorators
{
    public class BaseDecorator : INotyficationService
    {
        private readonly INotyficationService notyficationService;

        protected BaseDecorator(INotyficationService notyficationService)
        {
            this.notyficationService = notyficationService;
        }
        public virtual void Notify()
        {
            notyficationService.Notify();
        }
    }
}
