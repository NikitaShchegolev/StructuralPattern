using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge.Solution
{
    public class Tv
    {
        public bool isEnable;
        public bool IsEnable()
        {
            return isEnable;
        }
        public void Enable() { Console.WriteLine("Tv - Enable"); }
        public void Disable() { Console.WriteLine("Tv - Disable"); }
    }
}
