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
        private readonly IMongoCollection<User> _usersCollection;

        /// <summary>
        /// Конструктор сервиса MongoDB
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных MongoDB</param>
        /// <param name="databaseName">Имя базы данных</param>
        /// <param name="collectionName">Имя коллекции для пользователей</param>
        public MongoUserStorage(string connectionString, string databaseName, string collectionName)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _usersCollection = database.GetCollection<User>("Users");
        }

        /// <summary>
        /// Найти пользователей по условию (READ)
        /// </summary>
        /// <param name="predicate">Условие поиска</param>
        /// <returns>Список пользователей</returns>
        public List<User> FindUsers(Func<User, bool> predicate)
        {
            var allUsers = _usersCollection.Find(_ => true).ToList();
            return allUsers.Where(predicate).ToList();
        }

        /// <summary>
        /// Получить пользователя по GUID (READ)
        /// </summary>
        /// <param name="userId">GUID пользователя</param>
        /// <returns>Пользователь</returns>
        public User GetUser(Guid userId)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
            var user = _usersCollection.Find(filter).FirstOrDefault();
            
            if (user != null)
            {
                Console.WriteLine($"[MongoDB] Найден пользователь с Id: {userId}");
                return user;
            }
            
            Console.WriteLine($"[MongoDB] Пользователь с Id {userId} не найден");
            return new User() { Id = userId, Name = "Пользователь не найден" };
        }

        /// <summary>
        /// Сохранить или обновить пользователя в базе MongoDB (CREATE/UPDATE)
        /// </summary>
        /// <param name="user">Пользователь для сохранения</param>
        public void SaveUser(User user)
        {
            try
            {
                Console.WriteLine($"\n[MongoDB] ========================================");
                Console.WriteLine($"[MongoDB] Попытка сохранения пользователя:");
                Console.WriteLine($"[MongoDB]   Name: '{user.Name}'");
                Console.WriteLine($"[MongoDB]   Updata: '{user.Updata}'");
                Console.WriteLine($"[MongoDB]   DeleteUsers: '{user.DeleteUsers}'");
                Console.WriteLine($"[MongoDB]   GUID: {user.Id}");
                
                // Проверяем, существует ли пользователь с таким GUID
                var filterById = Builders<User>.Filter.Eq(u => u.Id, user.Id);
                var existingUserById = _usersCollection.Find(filterById).FirstOrDefault();

                if (existingUserById != null)
                {
                    // Пользователь с таким GUID уже существует - обновляем
                    Console.WriteLine($"[MongoDB] ✓ Найден пользователь с таким же GUID");
                    _usersCollection.ReplaceOne(filterById, user);
                    Console.WriteLine($"[MongoDB] ✅ Пользователь {user.Name} (Id: {user.Id}) ОБНОВЛЕН в MongoDB");
                    Console.WriteLine($"[MongoDB] ========================================\n");
                    return;
                }

                Console.WriteLine($"[MongoDB] ✗ Пользователь с GUID {user.Id} не найден");
                Console.WriteLine($"[MongoDB] Проверяем наличие дубликатов по данным...");
                
                // Проверяем, есть ли пользователь с ТОЧНО такими же данными
                var filterByData = Builders<User>.Filter.And(
                    Builders<User>.Filter.Eq(u => u.Name, user.Name),
                    Builders<User>.Filter.Eq(u => u.Updata, user.Updata),
                    Builders<User>.Filter.Eq(u => u.DeleteUsers, user.DeleteUsers)
                );
                
                Console.WriteLine($"[MongoDB] Поиск по критериям:");
                Console.WriteLine($"[MongoDB]   Name = '{user.Name}'");
                Console.WriteLine($"[MongoDB]   Updata = '{user.Updata}'");
                Console.WriteLine($"[MongoDB]   DeleteUsers = '{user.DeleteUsers}'");
                
                var duplicateUser = _usersCollection.Find(filterByData).FirstOrDefault();

                if (duplicateUser != null)
                {
                    // Найден пользователь с идентичными данными - НЕ СОХРАНЯЕМ
                    Console.WriteLine($"[MongoDB] ✓ Найден дубликат!");
                    Console.WriteLine($"[MongoDB]   Существующий GUID: {duplicateUser.Id}");
                    Console.WriteLine($"[MongoDB]   Попытка создать с GUID: {user.Id}");
                    Console.WriteLine($"[MongoDB] ⚠️ ДУБЛИКАТ НЕ СОХРАНЕН!");
                    Console.WriteLine($"[MongoDB] Используйте существующий GUID: {duplicateUser.Id}");
                    Console.WriteLine($"[MongoDB] ========================================\n");
                    return; // НЕ сохраняем дубликат
                }

                Console.WriteLine($"[MongoDB] ✗ Дубликат не найден");
                Console.WriteLine($"[MongoDB] Это новый уникальный пользователь");
                
                // Это новый уникальный пользователь - сохраняем
                _usersCollection.InsertOne(user);
                Console.WriteLine($"[MongoDB] ✅ Пользователь сохранен в MongoDB как НОВЫЙ");
                Console.WriteLine($"[MongoDB]   Name: '{user.Name}'");
                Console.WriteLine($"[MongoDB]   GUID: {user.Id}");
                Console.WriteLine($"[MongoDB] ========================================\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[MongoDB] ❌ ОШИБКА при сохранении пользователя: {ex.Message}");
                Console.WriteLine($"[MongoDB] Детали ошибки: {ex}");
                Console.WriteLine($"[MongoDB] ========================================\n");
                throw;
            }
        }

        /// <summary>
        /// Удалить пользователя из MongoDB (DELETE)
        /// </summary>
        /// <param name="userId">GUID пользователя</param>
        public void DeleteUser(Guid userId)
        {
            try
            {
                Console.WriteLine($"\n[MongoDB] ========================================");
                Console.WriteLine($"[MongoDB] Попытка удаления пользователя:");
                Console.WriteLine($"[MongoDB]   GUID: {userId}");
                
                var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
                var result = _usersCollection.DeleteOne(filter);
                
                if (result.DeletedCount > 0)
                {
                    Console.WriteLine($"[MongoDB] ✅ Пользователь с Id {userId} УДАЛЕН из MongoDB");
                    Console.WriteLine($"[MongoDB]   Удалено документов: {result.DeletedCount}");
                }
                else
                {
                    Console.WriteLine($"[MongoDB] ⚠️ Пользователь с Id {userId} НЕ НАЙДЕН в MongoDB");
                    Console.WriteLine($"[MongoDB]   Удалено документов: 0");
                }
                
                Console.WriteLine($"[MongoDB] ========================================\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[MongoDB] ❌ ОШИБКА при удалении пользователя: {ex.Message}");
                Console.WriteLine($"[MongoDB] Детали ошибки: {ex}");
                Console.WriteLine($"[MongoDB] ========================================\n");
                throw;
            }
        }
    }
}

