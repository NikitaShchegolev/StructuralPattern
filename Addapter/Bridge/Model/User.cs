using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge.Model
{
    public class User
    {
        //Id
        public Guid Id { get; set; }
        //Имя
        public string Name { get; set; }
        public string Updata { get; set; }
        public string DeleteUsers { get; set; }
    }
}
