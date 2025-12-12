using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bridge.Interface;
using Bridge.Storages;

using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;

using Npgsql;

namespace Bridge.Services
{
    public class MongoDBService : IMongoDBService
    {
        private readonly IMongoCollection<MongoSpaceCountResult> _collection;

        /// <summary>
        /// Конструктор сервиса MongoDB
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных MongoDB</param>
        /// <param name="databaseName">Имя базы данных</param>
        /// <param name="collectionName">Имя коллекции</param>
        public MongoDBService(string connectionString, string databaseName, string collectionName)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _collection = database.GetCollection<MongoSpaceCountResult>(collectionName);
        }

        /// <summary>
        /// Сохраняет результат подсчета пробелов в базе данных MongoDB
        /// </summary>
        /// <param name="result">Результат подсчета пробелов</param>
        /// <returns>Задача</returns>
        public async Task SaveSpaceCountResultAsync(MongoSpaceCountResult result)
        {
            await _collection.InsertOneAsync(result);
        }
       
    }
}
