using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bridge_MongoDB.Interface;

namespace Bridge_MongoDB.Solution
{
    public class Radio: IDevice
    {
        public bool isEnable;
        public bool IsEnabled() {return isEnable;}
        public void Enabled() { Console.WriteLine("Radio - Enabled"); }
        public void Disabled() { Console.WriteLine("Radio - Disable"); }

    }
}
