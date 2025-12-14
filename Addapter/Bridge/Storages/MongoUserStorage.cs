using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bridge.Interface;
using Bridge.Model;
using Bridge.Services;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace Bridge.Storages
{
    public class MongoUserStorage : IUserStorage
    {
        private readonly IMongoCollection<User> _usersCollection;
        private readonly IMongoCollection<MongoSpaceCountResult> _spaceCountCollection;
        private readonly IMongoDBService _mongoDBService;

        /// <summary>
        /// Конструктор сервиса MongoDB
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных MongoDB</param>
        /// <param name="databaseName">Имя базы данных</param>
        /// <param name="collectionName">Имя коллекции для результатов подсчета</param>
        /// <param name="mongoDBService">Сервис MongoDB</param>
        public MongoUserStorage(string connectionString, string databaseName, string collectionName, IMongoDBService? mongoDBService = null)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _spaceCountCollection = database.GetCollection<MongoSpaceCountResult>(collectionName);
            _usersCollection = database.GetCollection<User>("Users");
            _mongoDBService = mongoDBService ?? new MongoDBService(connectionString, databaseName, collectionName);
        }

        /// <summary>
        /// Найти пользователя
        /// </summary>
        /// <param name="predicate">Условие поиска</param>
        /// <returns>Список пользователей</returns>
        public List<User> FindUsers(Func<User, bool> predicate)
        {
            var allUsers = _usersCollection.Find(_ => true).ToList();
            return allUsers.Where(predicate).ToList();
        }

        /// <summary>
        /// Получить пользователя по Guid
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <returns>Пользователь</returns>
        public User GetUser(Guid userId)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
            var user = _usersCollection.Find(filter).FirstOrDefault();
            
            if (user != null)
            {
                Console.WriteLine($"Id пользователя {userId}");
                return user;
            }
            
            Console.WriteLine($"Пользователь с Id {userId} не найден");
            return new User() { Id = userId, Name = "Пользователь не найден" };
        }

        /// <summary>
        /// Сохранить пользователя в базе MongoDB
        /// </summary>
        /// <param name="user">Пользователь для сохранения</param>
        public void SaveUser(User user)
        {
            try
            {
                Console.WriteLine($"[MongoDB] Попытка сохранения пользователя: {user.Name} (Id: {user.Id})");
                
                var filter = Builders<User>.Filter.Eq(u => u.Id, user.Id);
                var existingUser = _usersCollection.Find(filter).FirstOrDefault();

                if (existingUser != null)
                {
                    _usersCollection.ReplaceOne(filter, user);
                    Console.WriteLine($"[MongoDB] Пользователь {user.Name} (Id: {user.Id}) обновлен в MongoDB");
                }
                else
                {
                    _usersCollection.InsertOne(user);
                    Console.WriteLine($"[MongoDB] Пользователь {user.Name} (Id: {user.Id}) сохранен в MongoDB");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[MongoDB] ОШИБКА при сохранении пользователя: {ex.Message}");
                Console.WriteLine($"[MongoDB] Детали ошибки: {ex}");
                throw;
            }
        }

        /// <summary>
        /// Удалить пользователя из MongoDB
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        public void DeleteUser(string userId)
        {
            if (Guid.TryParse(userId, out Guid guidId))
            {
                var filter = Builders<User>.Filter.Eq(u => u.Id, guidId);
                var result = _usersCollection.DeleteOne(filter);
                
                if (result.DeletedCount > 0)
                {
                    Console.WriteLine($"Пользователь {userId} удален из MongoDB");
                }
                else
                {
                    Console.WriteLine($"Пользователь {userId} не найден в MongoDB");
                }
            }
            else
            {
                Console.WriteLine($"Неверный формат Id пользователя: {userId}");
            }
        }

        /// <summary>
        /// Сохраняет результат подсчета пробелов в базе данных MongoDB
        /// </summary>
        /// <param name="result">Результат подсчета пробелов</param>
        /// <returns>Задача</returns>
        public async Task SaveSpaceCountResultAsync(MongoSpaceCountResult result)
        {
            result.Id = Guid.NewGuid();
            result.CreatedAt = DateTime.UtcNow;
            await _mongoDBService.SaveSpaceCountResultAsync(result);
            Console.WriteLine($"Результат подсчета пробелов для файла '{result.FilePath}' сохранен в MongoDB");
        }

        /// <summary>
        /// Сохраняет несколько результатов подсчета пробелов в базе данных MongoDB
        /// </summary>
        /// <param name="results">Массив результатов подсчета пробелов</param>
        /// <returns>Задача</returns>
        public async Task SaveSpaceCountResultsAsync(MongoSpaceCountResult[] results)
        {
            foreach (var result in results)
            {
                result.Id = Guid.NewGuid();
                result.CreatedAt = DateTime.UtcNow;
            }
            
            await _spaceCountCollection.InsertManyAsync(results);
            Console.WriteLine($"Сохранено {results.Length} результатов подсчета пробелов в MongoDB");
        }

        /// <summary>
        /// Извлекает содержимое файла из базы данных по пути к файлу
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        /// <returns>Содержимое файла или null, если файл не найден</returns>
        public async Task<string?> GetFileContentAsync(string filePath)
        {
            var filter = Builders<MongoSpaceCountResult>.Filter.Eq(r => r.FilePath, filePath);
            var result = await _spaceCountCollection.Find(filter).FirstOrDefaultAsync();
            
            if (result != null)
            {
                Console.WriteLine($"Найден результат для файла: {filePath}");
                return $"FilePath: {result.FilePath}, SpaceCount: {result.SpaceCount}, ProcessingTime: {result.ProcessingTimeMs}ms";
            }
            
            Console.WriteLine($"Файл {filePath} не найден в базе данных");
            return null;
        }

        /// <summary>
        /// Получает все результаты подсчета пробелов из базы данных
        /// </summary>
        /// <returns>Список результатов подсчета пробелов</returns>
        public async Task<List<MongoSpaceCountResult>> GetAllSpaceCountResultsAsync()
        {
            var results = await _spaceCountCollection.Find(_ => true).ToListAsync();
            Console.WriteLine($"Получено {results.Count} результатов подсчета пробелов из MongoDB");
            return results;
        }
    }
}

