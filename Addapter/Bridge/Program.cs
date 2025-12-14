using Bridge.Interface;
using Bridge.Model;
using Bridge.Services;
using Bridge.Solution;
using Bridge.Storages;

namespace Bridge
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== ЗАПУСК ПРОГРАММЫ ===\n");
            
            #region Устройства
            var device = new Tv();
            var remote = new Remote(device);
            remote.TogglePower();
            var device2 = new Radio();
            remote = new Remote(device2);
            remote.TogglePower();
            #endregion

            Console.WriteLine("\n=== ИНИЦИАЛИЗАЦИЯ ПОДКЛЮЧЕНИЙ ===\n");
            
            string mongoConnectionString = "mongodb://admin:xxx711717XXX@62.60.156.138:32017/?authSource=admin&directConnection=true";
            string mongoDatabaseName = "space_counter";
            string mongoCollectionName = "CreateUsers";

            Console.WriteLine("Создание MongoUserStorage...");
            IUserStorage mongoUserStorage = new MongoUserStorage(mongoConnectionString, mongoDatabaseName, mongoCollectionName);
            Console.WriteLine("MongoUserStorage создан\n");

            string postgresHost = "62.60.156.138";
            string postgresPortStr = "30001";
            string postgresDatabase = "postgres";
            string postgresUser = "postgres";
            string postgresPassword = "xxx711717";
            string postgresConnectionString = $"Host={postgresHost};Port={postgresPortStr};Database={postgresDatabase};Username={postgresUser};Password={postgresPassword};";

            Console.WriteLine("Создание PostgresUserStorage...");
            IUserStorage postgresUserStorage = new PostgresUserStorage(postgresConnectionString);
            Console.WriteLine("PostgresUserStorage создан\n");

            #region Тестирование работы с пользователями в MongoDB
            Console.WriteLine("\n=== РАБОТА С ПОЛЬЗОВАТЕЛЯМИ В MONGODB ===\n");
            
            var mongoUser1 = new User
            {
                Id = Guid.NewGuid(),
                Name = "Иван Иванов (MongoDB)",
                Updata = "Обновлен",
                DeleteUsers = "Нет",
                CreatedAt = DateTime.UtcNow
            };
            
            Console.WriteLine($"Создан объект mongoUser1: {mongoUser1.Name}, Id: {mongoUser1.Id}");
            mongoUserStorage.SaveUser(mongoUser1);
            
            Console.WriteLine($"\nПопытка получить пользователя из MongoDB...");
            var foundMongoUser = mongoUserStorage.GetUser(mongoUser1.Id);
            Console.WriteLine($"Результат: {foundMongoUser.Name}\n");
            #endregion

            #region Тестирование работы с пользователями в PostgreSQL
            Console.WriteLine("\n=== РАБОТА С ПОЛЬЗОВАТЕЛЯМИ В POSTGRESQL ===\n");
            
            var postgresUser1 = new User
            {
                Id = Guid.NewGuid(),
                Name = "Алексей Сидоров (PostgreSQL)",
                Updata = "Создан",
                DeleteUsers = "Нет",
                CreatedAt = DateTime.UtcNow
            };
            
            Console.WriteLine($"Создан объект postgresUser1: {postgresUser1.Name}, Id: {postgresUser1.Id}");
            postgresUserStorage.SaveUser(postgresUser1);
            
            Console.WriteLine($"\nПопытка получить пользователя из PostgreSQL...");
            var foundPostgresUser = postgresUserStorage.GetUser(postgresUser1.Id);
            Console.WriteLine($"Результат: {foundPostgresUser.Name}\n");
            
            Console.WriteLine("\n=== ОБНОВЛЕНИЕ ПОЛЬЗОВАТЕЛЯ В POSTGRESQL ===\n");
            postgresUser1.Name = "Алексей Александрович Сидоров (PostgreSQL)";
            postgresUser1.Updata = "Обновлен";
            postgresUserStorage.SaveUser(postgresUser1);
            
            Console.WriteLine($"\nПроверка обновления...");
            var updatedUser = postgresUserStorage.GetUser(postgresUser1.Id);
            Console.WriteLine($"Результат после обновления: {updatedUser.Name}\n");
            #endregion

            Console.WriteLine("\n=== ПРОГРАММА ЗАВЕРШЕНА ===");
            Console.ReadKey();
        }
    }
}
