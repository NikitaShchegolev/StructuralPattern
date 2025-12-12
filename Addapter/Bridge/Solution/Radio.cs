using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge.Solution
{
    public class Radio
    {
        public bool isEnable;
        public bool IsEnable() 
        {
            return isEnable;
        }
        public void Enable() { Console.WriteLine("Radio - Enable"); }
        public void Disable() { Console.WriteLine("Radio - Disable"); }
    }
}
