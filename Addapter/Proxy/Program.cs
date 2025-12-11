using Proxy.ExemplesClass;
namespace Proxy
{
    internal class Program
    {
        static void Main(string[] args)
        {
            YouTubeThirdPartyManager youTubeThirdPartyManager = new YouTubeThirdPartyManager();
            YouTubeThirdPartyManagerDictionaryVideo youTubeThirdPartyManagerDictionaryVideo = new YouTubeThirdPartyManagerDictionaryVideo(youTubeThirdPartyManager);
            YouTubeManager youManager = new YouTubeManager(youTubeThirdPartyManagerDictionaryVideo);
            youManager.RenderVideoPage(1);
            Console.ReadKey();
        }
    }
}
