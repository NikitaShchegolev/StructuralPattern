using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bridge.Repository.Interface;

namespace Bridge.Solution
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
