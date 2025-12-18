using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bridge.Interface;
using Bridge.Model;
using Bridge.Repository;
using Bridge.Service;

using Bridge_Postgres.Storages;

using Microsoft.Extensions.Configuration;

using Microsoft.Extensions.DependencyInjection;

using Npgsql;

namespace Bridge.Storages
{
    //Работа с Postgres
    internal class PostgresUserStorage : IUserStorage
    {
        /// <summary>
        /// Строка подключения к базе
        /// </summary>
        private readonly string connectForPostgres;
        public PostgresUserStorage(string connectForPostgres)
        {
            this.connectForPostgres = connectForPostgres;
            //Ждем подключения к Postgres
            InitiolalizeDataBase().Wait();
        }
        private async Task InitiolalizeDataBase()
        {
            using var connection = new NpgsqlConnection(connectForPostgres);
            await connection.OpenAsync();
            //Команда поиска таблицы в Postgres
            string findTable = @"SELECT EXISTS (SELECT FROM information_schema.tables  WHERE table_name = 'users')";
            //Проверка существования таблицы в базе
            using var command = new NpgsqlCommand(findTable, connection);
            var searchResult = await command.ExecuteScalarAsync();
            bool result = searchResult?.Equals(true) == true;
            //Команда создания таблицы если ее нет 
            var createTable = @"
                    CREATE TABLE IF NOT EXISTS users (
                        id UUID PRIMARY KEY,
                        name VARCHAR(255) NOT NULL,
                        lastname VARCHAR(255) NOT NULL,
                        created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
                    )";
            switch (result)
            {
                case true:
                    break;
                case false:
                    using (var createCommand = new NpgsqlCommand(createTable, connection))
                    {
                        await createCommand.ExecuteNonQueryAsync();//ждем выполнения
                    }
                    ;
                    break;
            }
        }
        /// <summary>
        /// Найти пользователя
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>Имя пользователя</returns>
        public List<User> FindUsers(Func<User, bool> predicate)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(connectForPostgres);
            connection.Open();//открыть соединение
            string commandWord = @"SELECT id, name, lastname, created_at FROM users";
            using NpgsqlCommand command = new NpgsqlCommand(commandWord, connection);
            using NpgsqlDataReader reader = command.ExecuteReader();
            List<User> allUsers = new List<User>();
            while (reader.Read())
            {
                allUsers.Add(new User()
                {
                    Id = reader.GetGuid("id"),
                    Name = reader.GetString("name"),
                    LastName = reader.GetString("lastname")
                });
            }
            return allUsers.Where(predicate).ToList();
        }
        /// <summary>
        /// Получить Guid пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Guid пользователя</returns>
        public User GetUser(Guid userId)
        {
            using var connection = new NpgsqlConnection(connectForPostgres);//создание соединения с базой
            connection.Open();//открываю соединение
            using var command = new NpgsqlCommand(@"SELECT id, name, lastname, created_at 
                                          FROM users 
                                          WHERE id = @id", connection);
            command.Parameters.AddWithValue("@id", userId);
            using var reader = command.ExecuteReader();
            var user = new User();
            switch (reader.Read())
            {
                case true:
                    user = new User()
                    {
                        Id = reader.GetGuid("id"),
                        Name = reader.GetString("name"),
                        LastName = reader.GetString("lastname")
                    };
                    return user;
                case false:
                    throw new InvalidOperationException($"Пользователь с Id: {userId} не найден");
            }

        }
        /// <summary>
        /// Сохранить пользователя в базе
        /// </summary>
        /// <param name="user"></param>
        public void SaveUser(User user)
        {
            using var connection = new NpgsqlConnection(connectForPostgres);//создание соединения с базой
            connection.Open();//открываю соединение
            var checkIdCommand = new NpgsqlCommand("SELECT COUNT(*) FROM users WHERE id=@id", connection);
            checkIdCommand.Parameters.AddWithValue("@id", user.Id);
            bool existsById = Convert.ToInt32(checkIdCommand.ExecuteScalar()) > 0;
            
            switch (existsById)
            {
                case true:
                    /*Пользователь существует, обновляем*/
                    var updateCommand = new NpgsqlCommand(@"UPDATE users 
SET name = @name, 
lastname = @lastname, 
created_at = @created_at
WHERE id = @id", connection);
                    updateCommand.Parameters.AddWithValue("@id", user.Id);
                    updateCommand.Parameters.AddWithValue("@name", user.Name);
                    updateCommand.Parameters.AddWithValue("@lastname", user.LastName);
                    updateCommand.Parameters.AddWithValue("@created_at", DateTime.UtcNow);
                    updateCommand.ExecuteNonQuery();
                    break;
                case false:
                    // Пользователь с таким GUID НЕ существует - создаем нового
                    var insertCommand = new NpgsqlCommand(@"INSERT INTO users 
(id, name, lastname, created_at)
VALUES 
(@id, @name, @lastname, @created_at)", connection);

                    insertCommand.Parameters.AddWithValue("@id", user.Id);
                    insertCommand.Parameters.AddWithValue("@name", user.Name);
                    insertCommand.Parameters.AddWithValue("@lastname", user.LastName);
                    insertCommand.Parameters.AddWithValue("@created_at", DateTime.UtcNow);
                    insertCommand.ExecuteNonQuery(); // Выполняем команду
                    break;

            }
        }
        /// <summary>
        /// PostgresUserStorage. Удалить пользователя
        /// </summary>
        /// <param name="userId"></param>
        public void DeleteUser(Guid userId)
        {
            using var connection = new NpgsqlConnection(connectForPostgres);
            connection.Open();
            using var updateCommand = new NpgsqlCommand(@"DELETE FROM users WHERE id = @id", connection);
            updateCommand.Parameters.AddWithValue("@id", userId);
            var rowsAffected = updateCommand.ExecuteNonQuery();
        }
                
    }
}
