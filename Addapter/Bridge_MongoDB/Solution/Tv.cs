using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bridge_MongoDB.Interface;

namespace Bridge_MongoDB.Solution
{
    public class Tv: IDevice
    {
        public bool isEnable;        
        public bool IsEnabled() { return true; }
        public void Enabled() { Console.WriteLine("Tv - Enabled"); }
        public void Disabled() { Console.WriteLine("Tv - Disable"); }
    }
}
