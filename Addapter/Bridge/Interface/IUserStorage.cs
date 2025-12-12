using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bridge.Model;
using Bridge.Services;

namespace Bridge.Interface
{
    public interface IUserStorage
    {
        /// <summary>
        /// Добавить пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Добавить пользователя</returns>
        User GetUser(Guid userId);
        /// <summary>
        /// Сохранить пользователя в базу
        /// </summary>
        /// <param name="user"></param>
        void SaveUser(User user);
        /// <summary>
        /// Удалить пользователя из базы данных пользователей
        /// </summary>
        /// <param name="userId"></param>
        void DeleteUser(string userId);
        /// <summary>
        /// Найти пользователя
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>Найти пользователя</returns>
        List<User> FindUsers(Func<User, bool> predicate);
        /// <summary>
        /// Сохраняет результат подсчета пробелов в базе данных
        /// </summary>
        /// <param name="result">Результат подсчета пробелов</param>
        /// <returns>Задача</returns>
        Task SaveSpaceCountResultAsync(MongoSpaceCountResult result);
        /// <summary>
        /// Сохраняет несколько результатов подсчета пробелов в базе данных
        /// </summary>
        /// <param name="results">Массив результатов подсчета пробелов</param>
        /// <returns>Задача</returns>
        Task SaveSpaceCountResultsAsync(MongoSpaceCountResult[] results);

        /// <summary>
        /// Извлекает содержимое файла из базы данных по пути к файлу
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        /// <returns>Содержимое файла или null, если файл не найден</returns>
        Task<string?> GetFileContentAsync(string filePath);

        /// <summary>
        /// Получает все результаты подсчета пробелов из базы данных
        /// </summary>
        /// <returns>Список результатов подсчета пробелов</returns>
        Task<List<MongoSpaceCountResult>> GetAllSpaceCountResultsAsync();
    }
}
