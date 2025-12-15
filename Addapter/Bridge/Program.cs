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
            #region Устройства
            var device = new Tv();
            var remote = new Remote(device);
            remote.TogglePower();
            var device2 = new Radio();
            remote = new Remote(device2);
            remote.TogglePower();
            #endregion

            // Создаем строку подключения к MongoDB
            string mongoConnectionString = "mongodb://admin:xxx711717XXX@62.60.156.138:32017/?authSource=admin&directConnection=true";
            string postgresConnectionString = "Host=62.60.156.138;Port=30001;Database=postgres;Username=postgres;Password=xxx711717";
            string mongoDatabaseName = "Users";
            string mongoCollectionName = "CreateUsers";

            IUserStorage mongoDBService = new MongoUserStorage(mongoConnectionString, mongoDatabaseName);
            IUserStorage postgresDBService = new PostgresUserStorage(postgresConnectionString);


            // Подсчет пробелов во всех файлах которые содержаться в папке TextFile
            await spaceCounterService.CountSpacesInRemoteFolder("TextFile");


            string postgresHost = "62.60.156.138";
            string postgresPortStr = "30001";
            string postgresDatabase = "postgres";
            string postgresUser = "postgres";
            string postgresPassword = "xxx711717";

            // Создаем строку подключения к PostgreSQL            
            string postgresConnectionString = $"Host={postgresHost};Port={postgresPortStr};Database={postgresDatabase};Username={postgresUser};Password={postgresPassword};";
            PostgresUserStorage postgresUserStorage = new PostgresUserStorage(postgresConnectionString);
            IPostgreSQLService postgreSQLService = new PostgreSQLService(postgresConnectionString);


            Console.ReadKey();
        }
    }
}