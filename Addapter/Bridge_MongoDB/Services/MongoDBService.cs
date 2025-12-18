using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bridge_MongoDB.Interface;
using Bridge_MongoDB.Storages;

using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;

using Npgsql;

namespace Bridge_MongoDB.Services
{
    public class MongoDBService
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
    }
}
