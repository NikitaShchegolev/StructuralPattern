using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bridge.Interface;
using Bridge.Model;
using Bridge.Services;

using Npgsql;

namespace Bridge.Storages
{
    public class PostgresUserStorage: IUserStorage
    {
        private readonly string _connectionString;
        private readonly IPostgreSQLService _postgreSQLService;
        
        public PostgresUserStorage(string connectionString)
        {
            _connectionString = connectionString;
            _postgreSQLService = new PostgreSQLService(connectionString);
            InitializeDatabaseAsync().Wait();
        }
        
        /// <summary>
        /// Инициализация таблиц в базе данных PostgreSQL
        /// </summary>
        private async Task InitializeDatabaseAsync()
        {
            try
            {
                Console.WriteLine("[PostgreSQL] Начало инициализации базы данных...");
                Console.WriteLine($"[PostgreSQL] Строка подключения: {_connectionString.Replace(_connectionString.Split("Password=")[1].Split(";")[0], "***")}");
                
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();
                Console.WriteLine("[PostgreSQL] Подключение к базе данных успешно установлено");

                var createUsersTable = @"
                    CREATE TABLE IF NOT EXISTS users (
                        id UUID PRIMARY KEY,
                        name VARCHAR(255) NOT NULL,
                        updata VARCHAR(255),
                        delete_users VARCHAR(255),
                        created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
                    )";

                var createSpaceCountResultsTable = @"
                    CREATE TABLE IF NOT EXISTS space_count_results (
                        id UUID PRIMARY KEY,
                        file_path VARCHAR(500) NOT NULL,
                        space_count INTEGER NOT NULL,
                        processing_time_ms BIGINT NOT NULL,
                        created_at TIMESTAMP NOT NULL
                    )";

                Console.WriteLine("[PostgreSQL] Создание таблицы users...");
                using (var command = new NpgsqlCommand(createUsersTable, connection))
                {
                    await command.ExecuteNonQueryAsync();
                }
                Console.WriteLine("[PostgreSQL] Таблица users создана или уже существует");

                Console.WriteLine("[PostgreSQL] Создание таблицы space_count_results...");
                using (var command = new NpgsqlCommand(createSpaceCountResultsTable, connection))
                {
                    await command.ExecuteNonQueryAsync();
                }
                Console.WriteLine("[PostgreSQL] Таблица space_count_results создана или уже существует");

                Console.WriteLine("[PostgreSQL] ✅ Таблицы PostgreSQL инициализированы успешно");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[PostgreSQL] ❌ ОШИБКА при инициализации базы данных: {ex.Message}");
                Console.WriteLine($"[PostgreSQL] Детали ошибки: {ex}");
                throw;
            }
        }
        
        /// <summary>
        /// Найти пользователей по условию
        /// </summary>
        /// <param name="predicate">Условие поиска</param>
        /// <returns>Список пользователей</returns>
        public List<User> FindUsers(Func<User, bool> predicate)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var command = new NpgsqlCommand("SELECT id, name, updata, delete_users, created_at FROM users", connection);
            using var reader = command.ExecuteReader();

            var allUsers = new List<User>();
            while (reader.Read())
            {
                allUsers.Add(new User
                {
                    Id = reader.GetGuid(0),
                    Name = reader.GetString(1),
                    Updata = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    DeleteUsers = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    CreatedAt = reader.GetDateTime(4)
                });
            }

            return allUsers.Where(predicate).ToList();
        }
        
        /// <summary>
        /// Получить пользователя по Guid
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <returns>Пользователь или null</returns>
        public User GetUser(Guid userId)
        {
            try
            {
                Console.WriteLine($"[PostgreSQL] Поиск пользователя с Id: {userId}");
                
                using var connection = new NpgsqlConnection(_connectionString);
                connection.Open();
                Console.WriteLine("[PostgreSQL] Подключение к базе данных установлено");

                using var command = new NpgsqlCommand(
                    "SELECT id, name, updata, delete_users, created_at FROM users WHERE id = @id", 
                    connection);
                command.Parameters.AddWithValue("@id", userId);

                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    var user = new User
                    {
                        Id = reader.GetGuid(0),
                        Name = reader.GetString(1),
                        Updata = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                        DeleteUsers = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                        CreatedAt = reader.GetDateTime(4)
                    };
                    Console.WriteLine($"[PostgreSQL] Пользователь найден в PostgreSQL: {user.Name} (Id: {userId})");
                    return user;
                }

                Console.WriteLine($"[PostgreSQL] Пользователь с Id {userId} не найден в PostgreSQL");
                return new User() { Id = userId, Name = "Пользователь не найден" };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[PostgreSQL] ОШИБКА при получении пользователя: {ex.Message}");
                Console.WriteLine($"[PostgreSQL] Детали ошибки: {ex}");
                throw;
            }
        }
        
        /// <summary>
        /// Сохранить пользователя в базе PostgreSQL
        /// </summary>
        /// <param name="user">Пользователь для сохранения</param>
        public void SaveUser(User user)
        {
            try
            {
                Console.WriteLine($"[PostgreSQL] Попытка сохранения пользователя: {user.Name} (Id: {user.Id})");
                
                using var connection = new NpgsqlConnection(_connectionString);
                connection.Open();
                Console.WriteLine("[PostgreSQL] Подключение к базе данных установлена");

                var checkCommand = new NpgsqlCommand("SELECT COUNT(*) FROM users WHERE id = @id", connection);
                checkCommand.Parameters.AddWithValue("@id", user.Id);
                var exists = Convert.ToInt32(checkCommand.ExecuteScalar()) > 0;
                
                Console.WriteLine($"[PostgreSQL] Пользователь существует: {exists}");

                if (exists)
                {
                    var updateCommand = new NpgsqlCommand(
                        @"UPDATE users 
                          SET name = @name, updata = @updata, delete_users = @delete_users, created_at = @created_at 
                          WHERE id = @id",
                        connection);
                    
                    updateCommand.Parameters.AddWithValue("@id", user.Id);
                    updateCommand.Parameters.AddWithValue("@name", user.Name);
                    updateCommand.Parameters.AddWithValue("@updata", user.Updata ?? string.Empty);
                    updateCommand.Parameters.AddWithValue("@delete_users", user.DeleteUsers ?? string.Empty);
                    updateCommand.Parameters.AddWithValue("@created_at", user.CreatedAt);

                    var rowsAffected = updateCommand.ExecuteNonQuery();
                    Console.WriteLine($"[PostgreSQL] Пользователь {user.Name} (Id: {user.Id}) обновлен в PostgreSQL. Затронуто строк: {rowsAffected}");
                }
                else
                {
                    var insertCommand = new NpgsqlCommand(
                        @"INSERT INTO users (id, name, updata, delete_users, created_at) 
                          VALUES (@id, @name, @updata, @delete_users, @created_at)",
                        connection);
                    
                    insertCommand.Parameters.AddWithValue("@id", user.Id);
                    insertCommand.Parameters.AddWithValue("@name", user.Name);
                    insertCommand.Parameters.AddWithValue("@updata", user.Updata ?? string.Empty);
                    insertCommand.Parameters.AddWithValue("@delete_users", user.DeleteUsers ?? string.Empty);
                    insertCommand.Parameters.AddWithValue("@created_at", user.CreatedAt);

                    var rowsAffected = insertCommand.ExecuteNonQuery();
                    Console.WriteLine($"[PostgreSQL] Пользователь {user.Name} (Id: {user.Id}) сохранен в PostgreSQL. Затронуто строк: {rowsAffected}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[PostgreSQL] ОШИБКА при сохранении пользователя: {ex.Message}");
                Console.WriteLine($"[PostgreSQL] Детали ошибки: {ex}");
                throw;
            }
        }
        
        /// <summary>
        /// Удалить пользователя из PostgreSQL
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        public void DeleteUser(string userId)
        {
            if (Guid.TryParse(userId, out Guid guidId))
            {
                using var connection = new NpgsqlConnection(_connectionString);
                connection.Open();

                using var command = new NpgsqlCommand("DELETE FROM users WHERE id = @id", connection);
                command.Parameters.AddWithValue("@id", guidId);

                var rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine($"Пользователь {userId} удален из PostgreSQL");
                }
                else
                {
                    Console.WriteLine($"Пользователь {userId} не найден в PostgreSQL");
                }
            }
            else
            {
                Console.WriteLine($"Неверный формат Id пользователя: {userId}");
            }
        }

        /// <summary>
        /// Сохраняет результат подсчета пробелов используя PostgreSQLService
        /// </summary>
        /// <param name="result">Результат подсчета пробелов</param>
        /// <returns>Задача</returns>
        public async Task SaveSpaceCountResultAsync(MongoSpaceCountResult result)
        {
            await _postgreSQLService.SaveSpaceCountResultAsync(result);
            Console.WriteLine($"Результат подсчета пробелов для файла '{result.FilePath}' сохранен в PostgreSQL через PostgreSQLService");
        }

        /// <summary>
        /// Сохраняет несколько результатов используя PostgreSQLService
        /// </summary>
        /// <param name="results">Массив результатов подсчета пробелов</param>
        /// <returns>Задача</returns>
        public async Task SaveSpaceCountResultsAsync(MongoSpaceCountResult[] results)
        {
            await _postgreSQLService.SaveSpaceCountResultsAsync(results);
            Console.WriteLine($"Сохранено {results.Length} результатов подсчета пробелов в PostgreSQL через PostgreSQLService");
        }

        /// <summary>
        /// Извлекает содержимое файла используя PostgreSQLService
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        /// <returns>Содержимое файла или null, если файл не найден</returns>
        public async Task<string?> GetFileContentAsync(string filePath)
        {
            return await _postgreSQLService.GetFileContentAsync(filePath);
        }

        /// <summary>
        /// Получает все результаты используя PostgreSQLService
        /// </summary>
        /// <returns>Список результатов подсчета пробелов</returns>
        public async Task<List<MongoSpaceCountResult>> GetAllSpaceCountResultsAsync()
        {
            var results = await _postgreSQLService.GetAllSpaceCountResultsAsync();
            Console.WriteLine($"Получено {results.Count} результатов подсчета пробелов из PostgreSQL через PostgreSQLService");
            return results;
        }
    }
}
