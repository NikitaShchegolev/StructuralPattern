using Addapter.ExemplesClass;
using System;

namespace Addapter
{
    /// <summary>
    /// Взаимодействие между компонентами:
    /// 1. Program создает экземпляр CardManager
    /// 2. CardManager в методе Issue создает CardIssueService и CardIssueServiceAdapter
    /// 3. CardIssueServiceAdapter адаптирует интерфейс CardInfo к интерфейсу Message
    /// 4. CardIssueService работает с объектами Message для выпуска карт
    /// 5. Результат возвращается через цепочку вызовов обратно в Program
    /// </summary>
    internal class Program
    {        
        static void Main(string[] args)
        {
            CardInfo cardInfo = new CardInfo() 
            {
                //Инициализация свойств значениями
                Id = Guid.NewGuid(),
                StarDate = DateTime.UtcNow,
                ExepirationDate = DateTime.UtcNow
            };
            //Здесь класс CardInfo - адаптируемый файл
            //А класс CardIssueServiceAdapter - адаптор для класса CardInfo
            var cardManager = new CardManager();
            cardManager.Issue(cardInfo);
            Console.ReadKey();
        }
    }
}
