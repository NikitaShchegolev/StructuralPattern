using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bridge.Interface;

namespace Bridge.Solution
{
    public class Tv: IDevice
    {
        public bool isEnable;        
        public bool IsEnabled() { return true; }
        public void Enabled() { Console.WriteLine("Tv - Enabled"); }
        public void Disabled() { Console.WriteLine("Tv - Disable"); }
    }
}
