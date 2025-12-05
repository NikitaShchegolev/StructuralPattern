namespace Addapter.ExemplesClass
{
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
}
