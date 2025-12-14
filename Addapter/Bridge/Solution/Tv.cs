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
        private bool isEnable;
        
        public bool IsEnabled() { return isEnable; }
        
        public void Enabled() 
        { 
            isEnable = true;
            Console.WriteLine("TV is on"); 
        }
        
        public void Disabled() 
        { 
            isEnable = false;
            Console.WriteLine("TV is off"); 
        }
    }
}
