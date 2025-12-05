using Addapter.ExemplesClass;

namespace Addapter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CardInfo cardInfo = new CardInfo() 
            {
                Id = Guid.NewGuid(),
                StarDate = DateTime.UtcNow,
                message = new Message() { Content = "Привет!"}
            };

            var cardManager = new CardManager();
            cardManager.Issue(cardInfo);
            Console.WriteLine(cardManager.Issue(cardInfo));
            Console.ReadKey();
            
        }
    }
}
