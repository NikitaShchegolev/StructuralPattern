using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bridge.Interface;
using Bridge.Model;
using Bridge.Services;

namespace Bridge.Storages
{
    internal class PostgresUserStorage: IUserStorage
    {
        /// <summary>
        /// Строка подключения к базе
        /// </summary>
        private readonly string connectForPostgres;
        public PostgresUserStorage(string connectForPostgres)
        {
            this.connectForPostgres = connectForPostgres;
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

        public Task SaveSpaceCountResultAsync(MongoSpaceCountResult result)
        {
            throw new NotImplementedException();
        }

        public Task SaveSpaceCountResultsAsync(MongoSpaceCountResult[] results)
        {
            throw new NotImplementedException();
        }

        public Task<string?> GetFileContentAsync(string filePath)
        {
            throw new NotImplementedException();
        }

        public Task<List<MongoSpaceCountResult>> GetAllSpaceCountResultsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
