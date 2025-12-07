using System;
using System.Collections.Generic;
using System.Text;

using Proxy.Interfaces;

namespace Proxy.ExemplesClass
{
    public class YouTubeThirdPartyManagerCached : IYouTubeThirdPartyManager
    {
        //Договор на выполнение получения информации о видосе GetVideoInfo(int id); по его Id
        public IYouTubeThirdPartyManager _youTubeThirdPartyManager;

        //Библиотеке в которую быдут записываться видосы с номером и информацией о нем
        private Dictionary<int, VideoInfo> cache = new Dictionary<int, VideoInfo>();

        public YouTubeThirdPartyManagerCached(YouTubeThirdPartyManager youTubeThirdPartyManager)
        {
            _youTubeThirdPartyManager = youTubeThirdPartyManager;
        }
        //Метод который добавляет youtube видео в библиотеку по номеру Id информацию о нем
        public VideoInfo GetVideoInfo(int id)
        {
            if (cache.ContainsKey(id))
            {
                return cache[id];
            }
            else 
            {
                VideoInfo videoInfo = _youTubeThirdPartyManager.GetVideoInfo(id);
                cache.Add(id, videoInfo); 
                return videoInfo;
            }
        }
    }
}
