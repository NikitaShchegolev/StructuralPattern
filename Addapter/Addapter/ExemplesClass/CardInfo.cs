using Addapter.ExemplesClass;

namespace Addapter.ExemplesClass
{
    /// <summary>
    /// Поля для идентификации создаваемой карты
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
}
