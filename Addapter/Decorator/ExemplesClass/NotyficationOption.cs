using System;
using System.Collections.Generic;
using System.Text;

namespace Decorator.ExemplesClass
{
    public class NotyficationOption
    {
        public bool SendFBNotification { get; set; }
        public bool SendToMail { get; set; }
        public bool SendToSms { get; set; }
    }
}
