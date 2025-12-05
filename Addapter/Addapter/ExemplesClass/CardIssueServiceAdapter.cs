using Addapter.ExemplesClass;
using Addapter.ExemplesInterface;

namespace Addapter.ExemplesClass
{
    //Класс Адаптер
    /// <summary>
    /// Адаптер для сервиса выпуска карт
    /// </summary>
    public class CardIssueServiceAdapter : ICardIssueServiceAdapter
    {
        //Создал поле для доступка к сервису выпуска карт
        private readonly CardIssueService _cardIssueService;
        
        /// <summary>
        /// Конструктор адаптера сервиса выпуска карт
        /// </summary>
        /// <param name="cardIssueService">Сервис выпуска карт</param>
        public CardIssueServiceAdapter(CardIssueService cardIssueService)
        {
            _cardIssueService = cardIssueService;
        }
        
        /// <summary>
        /// Выпускает карту с заданными параметрами
        /// </summary>
        /// <param name="cardInfo">Информация о карте</param>
        /// <returns>Строка с информацией о выпущенной карте</returns>
        public string Issue(CardInfo cardInfo)
        {
            var message = new Message() 
            { 
                Content = $"" +
                $"ID карты: {cardInfo.Id}\n" +
                $"Дата начала: {cardInfo.StarDate}\n" +
                $"Дата закрытии карты: {cardInfo.ExepirationDate}\n"
            };
            return _cardIssueService.Issue(message);
        }
    }
}
