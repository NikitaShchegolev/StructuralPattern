using System;
using System.Collections.Generic;
using System.Text;

using Proxy.Interfaces;

namespace Proxy.ExemplesClass
{
    public class YouTubeThirdPartyManager : IYouTubeThirdPartyManager
    {
        public VideoInfo GetVideoInfo(int id)
        {
            return new VideoInfo();
        }
    }
}
