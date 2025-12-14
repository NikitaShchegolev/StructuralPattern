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
        /// <summary>
        /// Id пользователя
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        
        /// <summary>
        /// Имя пользователя
        /// </summary>
        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        [BsonElement("lastname")]
        public string LastName { get; set; } = string.Empty;
        
        /// <summary>
        /// Информация об обновлении
        /// </summary>
        [BsonElement("updata")]
        public string Updata { get; set; } = string.Empty;
        
        /// <summary>
        /// Информация об удалении пользователей
        /// </summary>
        [BsonElement("deleteUsers")]
        public string DeleteUsers { get; set; } = string.Empty;
        
        /// <summary>
        /// Дата создания записи
        /// </summary>
        [BsonElement("createdAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
