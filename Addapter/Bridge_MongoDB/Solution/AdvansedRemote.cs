using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bridge_MongoDB.Interface;

namespace Bridge_MongoDB.Solution
{
    internal class AdvansedRemote: Remote
    {
        public AdvansedRemote(IDevice device): base(device) { }

        public override void TogglePower()
        {
            switch (!device.IsEnabled())
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
