using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge_MongoDB.Interface
{
    public interface ISpaceCounterService
    {               
        Task CountSpacesInRemoteFolder(string remoteDirectory);
    }
}
