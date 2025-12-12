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
            string mongoDatabaseName = "space_counter";
            string mongoCollectionName = "CreateUsers";

            IMongoDBService mongoDBService = new MongoUserStorage(mongoConnectionString, mongoDatabaseName, mongoCollectionName);
            ISpaceCounterService spaceCounterService = new SpaceCounterService( mongoDBService);


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



public class SpaceCounterService : ISpaceCounterService
{


    private IMongoDBService _collection;

    /// <summary>
    /// Конструктор сервиса подсчета пробелов
    /// </summary>
    /// <param name="postgreSQLService">Сервис PostgreSQL (опционально)</param>


    public SpaceCounterService(IMongoDBService mongoDBService)
    {
        this._collection = mongoDBService;
    }

    public Task<int[]> CountSpacesInFiles(string[] filePaths)
    {
        throw new NotImplementedException();
    }

    public Task CountSpacesInFolder(string folderPath)
    {
        throw new NotImplementedException();
    }

    public async Task CountSpacesInRemoteFolder(string remoteDirectory)
    {
        

            // Создаем результат для сохранения в PostgreSQL
            var spaceCountResult = new MongoSpaceCountResult
            {
                FilePath = remoteDirectory,
                CreatedAt = DateTime.UtcNow
            };

            var mongoResult = new MongoSpaceCountResult
            {
                ProcessingTimeMs = spaceCountResult.ProcessingTimeMs,
                CreatedAt = spaceCountResult.CreatedAt
            };
            await _collection.SaveSpaceCountResultAsync(mongoResult);
            Console.WriteLine("Результат успешно сохранен в MongoDB");

    }
}
