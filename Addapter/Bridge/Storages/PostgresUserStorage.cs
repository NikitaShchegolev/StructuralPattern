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
            int tableCount;
            bool result = int.TryParse(searchResult?.ToString(), out tableCount);
            var createTable = @"
                    CREATE TABLE IF NOT EXISTS users (
                        id UUID PRIMARY KEY,
                        name VARCHAR(255) NOT NULL,
                        updata VARCHAR(255),
                        created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
                    )";
            switch (result)
            {
                case true:
                    break;
                case false:

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
        public void DeleteUser(Guid userId)
        {
            Console.WriteLine($"Пользователь {userId} удален");
        }
    }
}
