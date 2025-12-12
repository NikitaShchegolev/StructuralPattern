using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bridge.Interface;

using Npgsql;

namespace Bridge.Services
{
    public class PostgreSQLService : IPostgreSQLService
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
        /// Сохраняет результат подсчета пробелов в базе данных
        /// </summary>
        /// <param name="result">Результат подсчета пробелов</param>
        /// <returns>Задача</returns>
        public async Task SaveSpaceCountResultAsync(MongoSpaceCountResult result)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new NpgsqlCommand(
                "INSERT INTO space_count_results (file_path, space_count, processing_time_ms, created_at) VALUES (@file_path, @space_count, @processing_time_ms, @created_at)",
                connection);

            command.Parameters.AddWithValue("@file_path", result.FilePath);
            command.Parameters.AddWithValue("@space_count", result.SpaceCount);
            command.Parameters.AddWithValue("@processing_time_ms", result.ProcessingTimeMs);
            command.Parameters.AddWithValue("@created_at", result.CreatedAt);

            await command.ExecuteNonQueryAsync();
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

        /// <summary>
        /// Получает все результаты подсчета пробелов из базы данных
        /// </summary>
        /// <returns>Список результатов подсчета пробелов</returns>
        public async Task<List<MongoSpaceCountResult>> GetAllSpaceCountResultsAsync()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new NpgsqlCommand(
                "SELECT id, file_path, space_count, processing_time_ms, created_at FROM space_count_results ORDER BY created_at DESC",
                connection);

            var results = new List<MongoSpaceCountResult>();
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                results.Add(new MongoSpaceCountResult
                {
                    Id = Guid.NewGuid(),
                    FilePath = reader.GetString(1),
                    SpaceCount = reader.GetInt32(2),
                    ProcessingTimeMs = reader.GetInt64(3),
                    CreatedAt = reader.GetDateTime(4)
                });
            }

            return results;
        }
    }
}
