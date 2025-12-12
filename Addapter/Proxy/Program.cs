using Proxy.ExemplesClass;
namespace Proxy
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var manager = new YouTubeThirdPartyManager();//Прокси
            var dictionaryVideo = new YouTubeThirdPartyManagerDictionaryVideo(manager);//список видосов
            var youT_Manager = new YouTubeManager(dictionaryVideo);//Добавляет информацию о видео
            youT_Manager.RenderVideoPage();//Обновляет и выводит информацию о видео
            Console.ReadKey();
        }
    }
}
