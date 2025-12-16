using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bridge.Interface;

using Npgsql;

namespace Bridge.Services
{
    public class PostgreSQLService
    {
        private readonly string _connectionString;

        /// <summary>
        /// Конструктор сервиса PostgreSQL
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных</param>
        public PostgreSQLService(string connectionString)
        {
            _connectionString = connectionString;
        }
    }
}
