using System;
using System.Collections.Generic;
using System.Text;

using Proxy.Interfaces;

namespace Proxy.ExemplesClass
{
    public class YouTubeThirdPartyManagerDictionaryVideo : IYouTubeThirdPartyManager
    {
        //Договор на выполнение получения информации о видосе GetVideoInfo(int id); по его Id
        public IYouTubeThirdPartyManager _youTubeThirdPartyManager;

        //Библиотеке в которую быдут записываться видосы с номером и информацией о нем
        private Dictionary<int, VideoInfo> dictionaryVideo = new Dictionary<int, VideoInfo>();

        public YouTubeThirdPartyManagerDictionaryVideo(YouTubeThirdPartyManager youTubeThirdPartyManager)
        {
            _youTubeThirdPartyManager = youTubeThirdPartyManager;
        }
        //Метод который добавляет youtube видео в библиотеку по номеру Id информацию о нем
        public VideoInfo GetVideoInfo(int id)
        {
            if (dictionaryVideo.ContainsKey(id))
            {
                return dictionaryVideo[id];
            }
            else 
            {
                VideoInfo videoInfo = _youTubeThirdPartyManager.GetVideoInfo(id);
                dictionaryVideo.Add(id, videoInfo); 
                return videoInfo;
            }
        }
    }
}
