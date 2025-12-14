using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bridge.Interface;
using Bridge.Model;

using Npgsql;

namespace Bridge.Storages
{
    /// <summary>
    /// Реализация хранилища пользователей для PostgreSQL
    /// </summary>
    public class PostgresUserStorage: IUserStorage
    {
        private readonly string _connectionString;
        
        public PostgresUserStorage(string connectionString)
        {
            _connectionString = connectionString;
            InitializeDatabaseAsync().Wait();
        }
        
        /// <summary>
        /// Инициализация таблицы users в базе данных PostgreSQL
        /// </summary>
        private async Task InitializeDatabaseAsync()
        {
            try
            {
                Console.WriteLine("[PostgreSQL] Начало инициализации базы данных...");
                
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

                Console.WriteLine("[PostgreSQL] Создание таблицы users...");
                using (var command = new NpgsqlCommand(createUsersTable, connection))
                {
                    await command.ExecuteNonQueryAsync();
                }
                Console.WriteLine("[PostgreSQL] ✅ Таблица users создана или уже существует");
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
        /// Получить пользователя по GUID
        /// </summary>
        /// <param name="userId">GUID пользователя</param>
        /// <returns>Пользователь</returns>
        public User GetUser(Guid userId)
        {
            try
            {
                Console.WriteLine($"[PostgreSQL] Поиск пользователя с Id: {userId}");
                
                using var connection = new NpgsqlConnection(_connectionString);
                connection.Open();

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
                    Console.WriteLine($"[PostgreSQL] Пользователь найден: {user.Name} (Id: {userId})");
                    return user;
                }

                Console.WriteLine($"[PostgreSQL] Пользователь с Id {userId} не найден");
                return new User() { Id = userId, Name = "Пользователь не найден" };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[PostgreSQL] ОШИБКА при получении пользователя: {ex.Message}");
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
                Console.WriteLine($"\n[PostgreSQL] ========================================");
                Console.WriteLine($"[PostgreSQL] Попытка сохранения пользователя:");
                Console.WriteLine($"[PostgreSQL]   Name: '{user.Name}'");
                Console.WriteLine($"[PostgreSQL]   Updata: '{user.Updata}'");
                Console.WriteLine($"[PostgreSQL]   DeleteUsers: '{user.DeleteUsers}'");
                Console.WriteLine($"[PostgreSQL]   GUID: {user.Id}");
                
                using var connection = new NpgsqlConnection(_connectionString);
                connection.Open();
                Console.WriteLine("[PostgreSQL] ✓ Подключение к базе данных установлено");

                // Проверяем, существует ли пользователь с таким GUID
                var checkIdCommand = new NpgsqlCommand("SELECT COUNT(*) FROM users WHERE id = @id", connection);
                checkIdCommand.Parameters.AddWithValue("@id", user.Id);
                var existsById = Convert.ToInt32(checkIdCommand.ExecuteScalar()) > 0;
                
                if (existsById)
                {
                    // Пользователь с таким GUID существует - обновляем
                    Console.WriteLine($"[PostgreSQL] ✓ Найден пользователь с таким же GUID");
                    
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
                    Console.WriteLine($"[PostgreSQL] ✅ Пользователь ОБНОВЛЕН. Затронуто строк: {rowsAffected}");
                    Console.WriteLine($"[PostgreSQL] ========================================\n");
                    return;
                }

                Console.WriteLine($"[PostgreSQL] ✗ Пользователь с GUID {user.Id} не найден");
                Console.WriteLine($"[PostgreSQL] Проверяем наличие дубликатов по данным...");
                
                // Проверяем, есть ли пользователь с ТОЧНО такими же данными
                var checkDuplicateCommand = new NpgsqlCommand(
                    @"SELECT id, name, updata, delete_users FROM users 
                      WHERE name = @name 
                        AND updata = @updata 
                        AND delete_users = @delete_users",
                    connection);
                
                checkDuplicateCommand.Parameters.AddWithValue("@name", user.Name);
                checkDuplicateCommand.Parameters.AddWithValue("@updata", user.Updata ?? string.Empty);
                checkDuplicateCommand.Parameters.AddWithValue("@delete_users", user.DeleteUsers ?? string.Empty);
                
                Console.WriteLine($"[PostgreSQL] Поиск по критериям:");
                Console.WriteLine($"[PostgreSQL]   Name = '{user.Name}'");
                Console.WriteLine($"[PostgreSQL]   Updata = '{user.Updata ?? string.Empty}'");
                Console.WriteLine($"[PostgreSQL]   DeleteUsers = '{user.DeleteUsers ?? string.Empty}'");
                
                using var reader = checkDuplicateCommand.ExecuteReader();
                if (reader.Read())
                {
                    // Найден пользователь с идентичными данными - НЕ СОХРАНЯЕМ
                    var existingGuid = reader.GetGuid(0);
                    var existingName = reader.GetString(1);
                    var existingUpdata = reader.GetString(2);
                    var existingDeleteUsers = reader.GetString(3);
                    
                    Console.WriteLine($"[PostgreSQL] ✓ Найден дубликат!");
                    Console.WriteLine($"[PostgreSQL]   Существующий GUID: {existingGuid}");
                    Console.WriteLine($"[PostgreSQL]   Существующие данные:");
                    Console.WriteLine($"[PostgreSQL]     Name: '{existingName}'");
                    Console.WriteLine($"[PostgreSQL]     Updata: '{existingUpdata}'");
                    Console.WriteLine($"[PostgreSQL]     DeleteUsers: '{existingDeleteUsers}'");
                    Console.WriteLine($"[PostgreSQL]   Попытка создать с GUID: {user.Id}");
                    Console.WriteLine($"[PostgreSQL] ⚠️ ДУБЛИКАТ НЕ СОХРАНЕН!");
                    Console.WriteLine($"[PostgreSQL] Используйте существующий GUID: {existingGuid}");
                    Console.WriteLine($"[PostgreSQL] ========================================\n");
                    reader.Close();
                    return; // НЕ сохраняем дубликат
                }
                reader.Close();

                Console.WriteLine($"[PostgreSQL] ✗ Дубликат не найден");
                Console.WriteLine($"[PostgreSQL] Это новый уникальный пользователь");

                // Это новый уникальный пользователь - сохраняем
                var insertCommand = new NpgsqlCommand(
                    @"INSERT INTO users (id, name, updata, delete_users, created_at) 
                      VALUES (@id, @name, @updata, @delete_users, @created_at)",
                    connection);
                
                insertCommand.Parameters.AddWithValue("@id", user.Id);
                insertCommand.Parameters.AddWithValue("@name", user.Name);
                insertCommand.Parameters.AddWithValue("@updata", user.Updata ?? string.Empty);
                insertCommand.Parameters.AddWithValue("@delete_users", user.DeleteUsers ?? string.Empty);
                insertCommand.Parameters.AddWithValue("@created_at", user.CreatedAt);

                var rowsAffected2 = insertCommand.ExecuteNonQuery();
                Console.WriteLine($"[PostgreSQL] ✅ Пользователь сохранен как НОВЫЙ. Затронуто строк: {rowsAffected2}");
                Console.WriteLine($"[PostgreSQL]   Name: '{user.Name}'");
                Console.WriteLine($"[PostgreSQL]   GUID: {user.Id}");
                Console.WriteLine($"[PostgreSQL] ========================================\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[PostgreSQL] ❌ ОШИБКА при сохранении пользователя: {ex.Message}");
                Console.WriteLine($"[PostgreSQL] Детали ошибки: {ex}");
                Console.WriteLine($"[PostgreSQL] ========================================\n");
                throw;
            }
        }

        /// <summary>
        /// Удалить пользователя из PostgreSQL (DELETE)
        /// </summary>
        /// <param name="userId">GUID пользователя</param>
        public void DeleteUser(Guid userId)
        {
            try
            {
                Console.WriteLine($"\n[PostgreSQL] ========================================");
                Console.WriteLine($"[PostgreSQL] Попытка удаления пользователя:");
                Console.WriteLine($"[PostgreSQL]   GUID: {userId}");
                
                using var connection = new NpgsqlConnection(_connectionString);
                connection.Open();

                using var command = new NpgsqlCommand("DELETE FROM users WHERE id = @id", connection);
                command.Parameters.AddWithValue("@id", userId);

                var rowsAffected = command.ExecuteNonQuery();
                
                if (rowsAffected > 0)
                {
                    Console.WriteLine($"[PostgreSQL] ✅ Пользователь с Id {userId} УДАЛЕН из PostgreSQL");
                    Console.WriteLine($"[PostgreSQL]   Удалено строк: {rowsAffected}");
                }
                else
                {
                    Console.WriteLine($"[PostgreSQL] ⚠️ Пользователь с Id {userId} НЕ НАЙДЕН в PostgreSQL");
                    Console.WriteLine($"[PostgreSQL]   Удалено строк: 0");
                }
                
                Console.WriteLine($"[PostgreSQL] ========================================\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[PostgreSQL] ❌ ОШИБКА при удалении пользователя: {ex.Message}");
                Console.WriteLine($"[PostgreSQL] Детали ошибки: {ex}");
                Console.WriteLine($"[PostgreSQL] ========================================\n");
                throw;
            }
        }
    }
}
