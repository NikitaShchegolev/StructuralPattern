using Proxy.ExemplesClass;
namespace Proxy
{
    internal class Program
    {
        static void Main(string[] args)
        {
            YouTubeThirdPartyManager youTubeThirdPartyManager = new YouTubeThirdPartyManager();
            YouTubeThirdPartyManagerCached youTubeThirdPartyManagerCached = new YouTubeThirdPartyManagerCached(youTubeThirdPartyManager);
            YouManager youManager = new YouManager(youTubeThirdPartyManagerCached);
            youManager.RenderVideoPage(1);
            Console.ReadKey();
        }
    }
}
