using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Bridge.Model
{
    
    public class User
    {
        public Guid Id { get; set; }        
        public string Name { get; set; } = string.Empty;        
        public string LastName { get; set; } = string.Empty;
    }
}
