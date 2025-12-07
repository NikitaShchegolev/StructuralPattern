using System;
using System.Collections.Generic;
using System.Text;

using Proxy.ExemplesClass;

namespace Proxy.Interfaces
{
    public interface IYouTubeThirdPartyManager
    {
        VideoInfo GetVideoInfo(int id);
    }
}
