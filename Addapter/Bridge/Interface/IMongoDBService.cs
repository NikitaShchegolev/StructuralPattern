using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bridge.Services;
using Bridge.Storages;

namespace Bridge.Interface
{
    public interface IMongoDBService
    {
        /// <summary>
        /// Сохраняет результат подсчета пробелов в базе данных MongoDB
        /// </summary>
        /// <param name="result">Результат подсчета пробелов</param>
        /// <returns>Задача</returns>
        Task SaveSpaceCountResultAsync(MongoSpaceCountResult result);

        
    }
}
