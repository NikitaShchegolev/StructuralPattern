using System;
using System.Collections.Generic;
using System.Text;

namespace Decorator.ExemplesClass
{
    /// <summary>
    /// Опции для настройки уведомлений.
    /// </summary>
    public class NotyficationOption
    {
        /// <summary>
        /// Отправлять уведомление в Facebook.
        /// </summary>
        public bool SendFBNotification { get; set; }
        
        /// <summary>
        /// Отправлять уведомление по электронной почте.
        /// </summary>
        public bool SendToMail { get; set; }
        
        /// <summary>
        /// Отправлять SMS-уведомление.
        /// </summary>
        public bool SendToSms { get; set; }
    }
}
