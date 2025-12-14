using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bridge.Services;

namespace Bridge.Interface
{
    /// <summary>
    /// Интерфейс сервиса для работы с базой данных PostgreSQL
    /// </summary>
    public interface IPostgreSQLService
    {
        /// <summary>
        /// Сохраняет результат подсчета пробелов в базе данных PostgreSQL
        /// </summary>
        /// <param name="result">Результат подсчета пробелов</param>
        /// <returns>Задача</returns>
        Task SaveSpaceCountResultAsync(MongoSpaceCountResult result);

        /// <summary>
        /// Сохраняет несколько результатов подсчета пробелов в базе данных PostgreSQL
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
