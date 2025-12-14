using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bridge.Interface;

namespace Bridge.Solution
{
    public class Radio: IDevice
    {
        private bool isEnable = false;
        
        public bool IsEnabled() { return isEnable; }
        
        public void Enabled() 
        { 
            isEnable = true;
            Console.WriteLine("Radio is on"); 
        }
        
        public void Disabled() 
        { 
            isEnable = false;
            Console.WriteLine("Radio is off"); 
        }
    }
}
