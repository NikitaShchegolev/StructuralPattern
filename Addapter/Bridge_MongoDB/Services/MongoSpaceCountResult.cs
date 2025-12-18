using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge_MongoDB.Services
{
    public class MongoSpaceCountResult
    {
        /// <summary>
        /// Идентификатор записи
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Путь к файлу
        /// </summary>
        public string FilePath { get; set; } = string.Empty;

        /// <summary>
        /// Количество пробелов в файле
        /// </summary>
        public int SpaceCount { get; set; }

        /// <summary>
        /// Время обработки файла в миллисекундах
        /// </summary>
        public long ProcessingTimeMs { get; set; }

        /// <summary>
        /// Дата и время создания записи
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
