using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bridge.Interface;
using Bridge.Model;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace Bridge.Storages
{
    /// <summary>
    /// Реализация хранилища пользователей для MongoDB
    /// </summary>
    public class MongoUserStorage : IUserStorage
    {
        private readonly IMongoDatabase _database;

        /// <summary>
        /// Конструктор сервиса MongoDB
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных MongoDB</param>
        /// <param name="databaseName">Имя базы данных</param>
        public MongoUserStorage(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        /// <summary>
        /// Найти пользователей по условию (READ)
        /// </summary>
        /// <param name="predicate">Условие поиска</param>
        /// <returns>Список пользователей</returns>
        public List<User> FindUsers(Func<User, bool> predicate)
        {
            var collection = _database.GetCollection<User>("Users");
            var allUsers = collection.Find(_ => true).ToList();
            return allUsers.Where(predicate).ToList();
        }

        /// <summary>
        /// Получить пользователя по GUID (READ)
        /// </summary>
        /// <param name="userId">GUID пользователя</param>
        /// <returns>Пользователь</returns>
        public User GetUser(Guid userId)
        {
            var collection = _database.GetCollection<User>("Users");
            var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
            return collection.Find(filter).FirstOrDefault();
        }

        /// <summary>
        /// Сохранить или обновить пользователя в базе MongoDB (CREATE/UPDATE)
        /// </summary>
        /// <param name="user">Пользователь для сохранения</param>
        public void SaveUser(User user)
        {
            var collection = _database.GetCollection<User>("Users");
            
            var filterById = Builders<User>.Filter.Eq(u => u.Id, user.Id);
            var existingUserById = collection.Find(filterById).FirstOrDefault();

            if (existingUserById != null)
            {
                collection.ReplaceOne(filterById, user);
                return;
            }

            var filterByData = Builders<User>.Filter.And(
                Builders<User>.Filter.Eq(u => u.Name, user.Name),
                Builders<User>.Filter.Eq(u => u.LastName, user.LastName),
                Builders<User>.Filter.Eq(u => u.Updata, user.Updata),
                Builders<User>.Filter.Eq(u => u.DeleteUsers, user.DeleteUsers)
            );
            
            var duplicateUser = collection.Find(filterByData).FirstOrDefault();

            if (duplicateUser != null)
            {
                return;
            }

            collection.InsertOne(user);
        }

        /// <summary>
        /// Удалить пользователя из MongoDB (DELETE)
        /// </summary>
        /// <param name="userId">GUID пользователя</param>
        public void DeleteUser(Guid userId)
        {
            var collection = _database.GetCollection<User>("Users");
            var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
            collection.DeleteOne(filter);
        }
    }
}

