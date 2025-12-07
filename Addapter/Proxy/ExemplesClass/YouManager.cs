
using Proxy.Interfaces;

namespace Proxy.ExemplesClass
{
    public class YouManager
    {
        private IYouTubeThirdPartyManager _youTubeManager;
        public YouManager(IYouTubeThirdPartyManager youTubeManager)
        {
            _youTubeManager = youTubeManager;
        }
        public VideoInfo GetVideoInfo(int id) 
        {
            return _youTubeManager.GetVideoInfo(id);
        }
        public void RenderVideoPage(int id) 
        { 
            var videoInfo = GetVideoInfo(id);
            Render(videoInfo);
        }

        private void Render(VideoInfo videoInfo)
        {
            Console.WriteLine("Hi");
        }
    }
}