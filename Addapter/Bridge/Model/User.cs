using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Bridge.Model
{
    /// <summary>
    ///[BsonId] и тд - Эти атрибуты используются в MongoDB(не в PostgreSQL) для настройки отображения свойств C# классов в документы базы данных.
    /// </summary>
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
        [BsonElement("Name")]
        public string Name { get; set; }
        /// <summary>
        /// Фамилия
        /// </summary>
        [BsonElement("LastName")]
        public string LastName { get; set; }
        /// <summary>
        /// Обновление
        /// </summary>
        [BsonElement("Updata")]
        public string Updata { get; set; }
        /// <summary>
        /// Удаление пользователя
        /// </summary>
        [BsonElement("DeleteUsers")]
        public string DeleteUsers { get; set; }

        [BsonElement("CreatedAt")] //= "Это поле - ID документа"
        [BsonDateTimeOptions(Kind =DateTimeKind.Utc)] //= "Храни ID как строку, а не ObjectId"
        internal DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
