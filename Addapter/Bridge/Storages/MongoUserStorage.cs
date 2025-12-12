using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bridge.Interface;
using Bridge.Model;
using Bridge.Services;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

using SharpCompress.Common;

namespace Bridge.Storages
{
    public class MongoUserStorage :  IMongoDBService
    {
        /// <summary>
        /// Строка подключения к базе
        /// </summary>
        private readonly IMongoCollection<MongoSpaceCountResult> _collection;
        private readonly IMongoDBService? _mongoDBService;

        /// <summary>
        /// Конструктор сервиса MongoDB
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных MongoDB</param>
        /// <param name="databaseName">Имя базы данных</param>
        /// <param name="collectionName">Имя коллекции</param>
        public MongoUserStorage(string connectionString, string databaseName, string collectionName, IMongoDBService? mongoDBService = null)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _collection = database.GetCollection<MongoSpaceCountResult>(collectionName);
            _mongoDBService = mongoDBService;
        }
        /// <summary>
        /// Найти пользователя
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>Имя пользователя</returns>
        public List<User> FindUsers(Func<User, bool> predicate)
        {
            return new List<User>();
        }
        /// <summary>
        /// Получить Guid пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Guid пользователя</returns>
        public User GetUser(Guid userId)
        {
            Console.WriteLine($"Id пользователя {userId}");
            return new User() { Id = userId, Name = "Пользователь 3. Mongo User" };
        }
        /// <summary>
        /// Сохранить пользователя в базе
        /// </summary>
        /// <param name="user"></param>
        public void SaveUser(User user)
        {
            Console.WriteLine("Пользователь сохранен");
        }
        /// <summary>
        /// Удалить пользователя
        /// </summary>
        /// <param name="userId"></param>
        public void DeleteUser(string userId)
        {
            Console.WriteLine($"Пользователь {userId} удален");
        }

        /// <summary>
        /// Сохраняет результат подсчета пробелов в базе данных MongoDB
        /// </summary>
        /// <param name="result">Результат подсчета пробелов</param>
        /// <returns>Задача</returns>
        public async Task SaveSpaceCountResultAsync(MongoSpaceCountResult result)
        {
            var mongoResult = new MongoSpaceCountResult
            {
                Id = Guid.NewGuid(),
                SpaceCount = result.SpaceCount,
                
            };
            await _mongoDBService.SaveSpaceCountResultAsync(mongoResult);
        }
        
    }
}

