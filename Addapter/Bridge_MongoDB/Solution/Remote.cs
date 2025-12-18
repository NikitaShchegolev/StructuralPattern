using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bridge_MongoDB.Interface;

namespace Bridge_MongoDB.Solution
{
    public class Remote
    {
        protected IDevice device;
        public Remote(IDevice device)
        {
            this.device = device;
        }
        public virtual void TogglePower()
        {
            switch (device.IsEnabled())
            {
                case true:
                    device.Disabled();
                    break;
                case false:
                    device.Enabled();
                    break;
            }
        }
    }    
}
