using System;
using System.Collections.Generic;
using System.Text;

using Proxy.Interfaces;

namespace Proxy.ExemplesClass
{
    //YouTubeThirdPartyManager - Proxy Предоставляет суррогат или заполнитель для другого объекта, чтобы контролировать доступ к нему.
    public class YouTubeThirdPartyManager : IYouTubeThirdPartyManager
    {

        //Договор на то что класс YouTubeThirdPartyManager будет выполнять метод GetVideoInfo
        //и здесь этот метод будет возвращать создавать конструктор информации по видео
        public VideoInfo GetVideoInfo(int id)
        {
            return new VideoInfo();
        }
    }
}
