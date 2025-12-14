using Bridge.Interface;
using Bridge.Model;
using Bridge.Repository;
using Bridge.Solution;
using Bridge.Storages;

namespace Bridge
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            #region Конфигурация подключений
            
            string mongoConnectionString = "mongodb://admin:xxx711717XXX@62.60.156.138:32017/?authSource=admin&directConnection=true";
            string mongoDatabaseName = "space_counter";
            string mongoCollectionName = "CreateUsers";

            string postgresConnectionString = "Host=62.60.156.138;Port=30001;Database=postgres;Username=postgres;Password=xxx711717;";
            
            IUserStorage mongoStorage = new MongoUserStorage(mongoConnectionString, mongoDatabaseName, mongoCollectionName);
            IUserStorage postgresStorage = new PostgresUserStorage(postgresConnectionString);
            
            Console.WriteLine("✅ Подключения к MongoDB и PostgreSQL установлены\n");
            #endregion

            #region Устройства - Демонстрация паттерна Bridge
            var device = new Tv();
            var remote = new Remote(device);
            remote.TogglePower();
            
            var device2 = new Radio();
            remote = new Remote(device2);
            remote.TogglePower();
            #endregion

            #region Проверка существующих пользователей
            var existingUser1Mongo = mongoStorage.FindUsers(u => 
                u.Name.Contains("Иван") && u.Name.Contains("Иванов")
            ).FirstOrDefault();
            
            var existingUser2Mongo = mongoStorage.FindUsers(u => 
                u.Name.Contains("Мария") && u.Name.Contains("Петрова")
            ).FirstOrDefault();
            
            var existingUser3Mongo = mongoStorage.FindUsers(u => 
                u.Name.Contains("Петр") && u.Name.Contains("Петров")
            ).FirstOrDefault();
            
            var existingUser4Mongo = mongoStorage.FindUsers(u => 
                u.Name.Contains("Анна") && u.Name.Contains("Сидорова")
            ).FirstOrDefault();
            #endregion

            #region СОЗДАНИЕ НОВЫХ ПОЛЬЗОВАТЕЛЕЙ 3 и 4
            User user3;
            if (existingUser3Mongo != null)
            {
                user3 = existingUser3Mongo;
            }
            else
            {
                user3 = new User
                {
                    Id = Guid.NewGuid(),
                    Name = "Петр Петров",
                    Updata = "Создан",
                    DeleteUsers = "Нет",
                    CreatedAt = DateTime.UtcNow
                };
                
                mongoStorage.SaveUser(user3);
                postgresStorage.SaveUser(user3);
            }
            
            User user4;
            if (existingUser4Mongo != null)
            {
                user4 = existingUser4Mongo;
            }
            else
            {
                user4 = new User
                {
                    Id = Guid.NewGuid(),
                    Name = "Анна Сидорова",
                    Updata = "Создана",
                    DeleteUsers = "Нет",
                    CreatedAt = DateTime.UtcNow
                };
                
                mongoStorage.SaveUser(user4);
                postgresStorage.SaveUser(user4);
            }
            #endregion

            #region ПОЛУЧЕНИЕ СУЩЕСТВУЮЩИХ ПОЛЬЗОВАТЕЛЕЙ ДЛЯ ОБНОВЛЕНИЯ
            User user1 = existingUser1Mongo;
            User user2 = existingUser2Mongo;
            #endregion

            #region ЧТЕНИЕ (READ) - Проверка сохранения новых пользователей
            var foundUser3InMongo = mongoStorage.GetUser(user3.Id);
            var foundUser3InPostgres = postgresStorage.GetUser(user3.Id);
            
            var foundUser4InMongo = mongoStorage.GetUser(user4.Id);
            var foundUser4InPostgres = postgresStorage.GetUser(user4.Id);
            
            bool user3Match = (foundUser3InMongo.Id == foundUser3InPostgres.Id && 
                              foundUser3InMongo.Name == foundUser3InPostgres.Name);
            bool user4Match = (foundUser4InMongo.Id == foundUser4InPostgres.Id && 
                              foundUser4InMongo.Name == foundUser4InPostgres.Name);
            
            if (user1 != null && user2 != null)
            {
                var foundUser1InMongo = mongoStorage.GetUser(user1.Id);
                var foundUser1InPostgres = postgresStorage.GetUser(user1.Id);
                
                var foundUser2InMongo = mongoStorage.GetUser(user2.Id);
                var foundUser2InPostgres = postgresStorage.GetUser(user2.Id);
                
                bool user1Match = (foundUser1InMongo.Id == foundUser1InPostgres.Id && 
                                  foundUser1InMongo.Name == foundUser1InPostgres.Name);
                bool user2Match = (foundUser2InMongo.Id == foundUser2InPostgres.Id && 
                                  foundUser2InMongo.Name == foundUser2InPostgres.Name);
            }
            #endregion

            #region ОБНОВЛЕНИЕ (UPDATE) - Изменение данных существующих пользователей
            Guid targetGuid = Guid.Parse("e34c12a4-f552-4471-b74d-a5dc074f8987");
            
            User userToUpdate = null;
            
            try
            {
                userToUpdate = mongoStorage.GetUser(targetGuid);
            }
            catch (Exception ex)
            {
                userToUpdate = null;
            }
            
            if (userToUpdate != null)
            {
                string oldName = userToUpdate.Name;
                string oldStatus = userToUpdate.Updata;
                
                userToUpdate.Name = $"{userToUpdate.Name} (Обновлено {DateTime.Now:dd.MM.yyyy HH:mm:ss})";
                userToUpdate.Updata = "Обновлен";
                
                mongoStorage.SaveUser(userToUpdate);
                postgresStorage.SaveUser(userToUpdate);
                
                var updatedUserMongo = mongoStorage.GetUser(userToUpdate.Id);
                var updatedUserPostgres = postgresStorage.GetUser(userToUpdate.Id);
            }
            #endregion

            #region Демонстрация паттерна Bridge
            IUserStorage storage;
            
            storage = mongoStorage;
            var mongoUsers = storage.FindUsers(u => true);
            
            storage = postgresStorage;
            var postgresUsers = storage.FindUsers(u => true);
            #endregion

            #region Итоговая статистика
            //var allMongoUsers = mongoStorage.FindUsers(u => true);
            //var allPostgresUsers = postgresStorage.FindUsers(u => true);
            #endregion

            Console.ReadKey();
        }
    }
}
