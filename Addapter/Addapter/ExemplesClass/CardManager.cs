using System;
using System.Collections.Generic;
using System.Text;

namespace Addapter.ExemplesClass
{
    /// <summary>
    /// Менеджер для управления картами
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
            ICardIssueServiceAdapter adaptor = new CardIssueServiceAdapter(cardInfoService);
            adaptor.Issue(cardInfo);
            return adaptor.Issue(cardInfo);

        }
    }
    /// <summary>
    /// Информация о карте
    /// </summary>
    public class CardInfo
    {
        /// <summary>
        /// Уникальный идентификатор карты
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Дата начала действия карты
        /// </summary>
        public DateTime StarDate { get; set; }
        /// <summary>
        /// Дата окончания действия карты
        /// </summary>
        public DateTime ExepirationDate { get; set; }
        /// <summary>
        /// Сообщение, связанное с картой
        /// </summary>
        public Message message { get; set; } = new Message();
    }
    //Класс Адаптер
    /// <summary>
    /// Адаптер для сервиса выпуска карт
    /// </summary>
    public class CardIssueServiceAdapter : ICardIssueServiceAdapter
    {
        //Создал поле для доступка к 
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

    /// <summary>
    /// Интерфейс адаптера сервиса выпуска карт
    /// </summary>
    public interface ICardIssueServiceAdapter
    {
        /// <summary>
        /// Выпускает карту с заданными параметрами
        /// </summary>
        /// <param name="cardInfo">Информация о карте</param>
        /// <returns>Строка с информацией о выпущенной карте</returns>
        string Issue(CardInfo cardInfo);
    }

    /// <summary>
    /// Сервис выпуска карт
    /// </summary>
    public class CardIssueService
    {
        /// <summary>
        /// Выпускает карту с заданным сообщением
        /// </summary>
        /// <param name="message">Сообщение с информацией о карте</param>
        /// <returns>Строка с информацией о выпущенной карте</returns>
        public string Issue(Message message) 
        {
            
            return message.Content; 
        }
    }

    /// <summary>
    /// Сообщение
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Содержимое сообщения
        /// </summary>
        public string Content { get; set; }
    }
}
