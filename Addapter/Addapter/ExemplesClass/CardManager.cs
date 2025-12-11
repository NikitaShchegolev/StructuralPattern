using System;
using System.Collections.Generic;
using System.Text;

using Addapter.ExemplesInterface;
using Addapter.ExemplesClass;

namespace Addapter.ExemplesClass
{
    /// <summary>
    /// CardIssueService работает с объектами Message для выпуска карт
    /// </summary>
    public class CardManager
    {
        /// <summary>
        /// Выпускает карту с заданными параметрами
        /// </summary>
        /// <param name="cardInfo">Информация о карте</param>
        /// <returns>Строка с информацией о выпущенной карте</returns>
        public string Issue(CardInfo cardInfo)
        {
            var cardInfoService = new CardIssueService();
            //класс адаптер
            ICardIssueServiceAdapter adaptor = new CardIssueServiceAdapter(cardInfoService);
            adaptor.Issue(cardInfo);
            Console.WriteLine(adaptor.Issue(cardInfo));
            return adaptor.Issue(cardInfo);
        }
    }
}
 