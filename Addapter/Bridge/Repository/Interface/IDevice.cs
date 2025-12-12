using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge.Repository.Interface
{
    public interface IDevice
    {
        public bool IsEnabled();
        public void Enabled();
        public void Disabled();
    }
}
