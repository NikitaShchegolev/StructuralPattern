using Proxy.ExemplesClass;
namespace Proxy
{
    internal class Program
    {
        static void Main(string[] args)
        {
            YouTubeThirdPartyManager youTubeThirdPartyManager = new YouTubeThirdPartyManager();//Прокси
            YouTubeThirdPartyManagerDictionaryVideo youTubeThirdPartyManagerDictionaryVideo = new YouTubeThirdPartyManagerDictionaryVideo(youTubeThirdPartyManager);//список видосов
            YouTubeManager youManager = new YouTubeManager(youTubeThirdPartyManagerDictionaryVideo);//Добавляет информацию о видео
            youManager.RenderVideoPage();//Обновляет и выводит информацию о видео
            Console.ReadKey();
        }
    }
}
