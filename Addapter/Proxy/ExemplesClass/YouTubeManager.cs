
using Proxy.Interfaces;

namespace Proxy.ExemplesClass
{
    public class YouTubeManager
    {
        private IYouTubeThirdPartyManager _youTubeManager;
        public YouTubeManager(IYouTubeThirdPartyManager youTubeManager)
        {
            _youTubeManager = youTubeManager;
        }
        public VideoInfo GetVideoInfo(int id) 
        {
            return _youTubeManager.GetVideoInfo(id);
        }
        public void RenderVideoPage() 
        { 
            var videoInfo = GetVideoInfo(1);
            Render(videoInfo);
        }

        private void Render(VideoInfo videoInfo)
        {
            videoInfo.Id = Guid.NewGuid();
            videoInfo.DownloadDate = DateTime.UtcNow;
            Console.WriteLine($"Видео создано: {videoInfo.DownloadDate}\nGuid: {videoInfo.Id}");
        }
    }
}