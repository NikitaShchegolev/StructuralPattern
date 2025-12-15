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

        

        /// <summary>
        /// Сохраняет в базе данных
        /// </summary>
        /// <param name="results">Массив результатов подсчета пробелов</param>
        /// <returns>Задача</returns>
        public async Task SaveSpaceCountResultsAsync(MongoSpaceCountResult[] results)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            using var transaction = await connection.BeginTransactionAsync();

            try
            {
                foreach (var result in results)
                {
                    using var command = new NpgsqlCommand(
                        "INSERT INTO space_count_results (file_path, space_count, processing_time_ms, created_at) VALUES (@file_path, @space_count, @processing_time_ms, @created_at)",
                        connection, transaction);

                    command.Parameters.AddWithValue("@file_path", result.FilePath);
                    command.Parameters.AddWithValue("@space_count", result.SpaceCount);
                    command.Parameters.AddWithValue("@processing_time_ms", result.ProcessingTimeMs);
                    command.Parameters.AddWithValue("@created_at", result.CreatedAt);

                    await command.ExecuteNonQueryAsync();
                }

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Извлекает из базы данных
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        /// <returns></returns>
        public async Task<string?> GetFileContentAsync(string filePath)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new NpgsqlCommand(
                "SELECT content FROM files WHERE file_path = @file_path",
                connection);

            command.Parameters.AddWithValue("@file_path", filePath);

            var result = await command.ExecuteScalarAsync();
            return result?.ToString();
        }

        
    }
}
