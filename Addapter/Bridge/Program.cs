using Bridge.Interface;
using Bridge.Model;
using Bridge.Service;
using Bridge.Services;
using Bridge.Solution;
using Bridge.Storages;

namespace Bridge
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            #region Устройства - Демонстрация паттерна Bridge
            var device = new Tv();
            var remote = new Remote(device);
            remote.TogglePower();

            var device2 = new Radio();
            remote = new Remote(device2);
            remote.TogglePower();
            #endregion

            #region Конфигурация подключений
            string mongoConnectionString = "mongodb://admin:xxx711717XXX@62.60.156.138:32017/?authSource=admin&directConnection=true";
            string mongoDatabaseName = "Users";

            string postgresConnectionString = "Host=78.40.217.5;Port=5432;Database=toolsdb;Username=postgres;Password=xxx711717;";

            IUserStorage mongoStorage = new MongoUserStorage(mongoConnectionString, mongoDatabaseName);
            IUserStorage postgresStorage = new PostgresUserStorage(postgresConnectionString);

            Console.WriteLine("Подключения к MongoDB и PostgreSQL установлены\n");
            #endregion

            #region Проверка всех существующих пользователей
             List<User> userMogo = UserService.ValidateAllUsersMongo(mongoStorage);
            Console.WriteLine(userMogo.Count);
             List<User> userPostgres = UserService.ValidateAllUsersPostgres(postgresStorage);
            Console.WriteLine(userPostgres.Count);
            #endregion

            #region СОЗДАНИЕ НОВЫХ ПОЛЬЗОВАТЕЛЕЙ
            var allExistingUsers = mongoStorage.FindUsers(u => true);

            var existingPetrov = allExistingUsers.FirstOrDefault(u =>
                u.Name == "Петр" && u.LastName == "Петров"
            );

            var existingSidorova = allExistingUsers.FirstOrDefault(u =>
                u.Name == "Анна" && u.LastName == "Сидорова"
            );

            User user3;
            if (existingPetrov != null)
            {
                user3 = existingPetrov;
                Console.WriteLine($"Используется существующий пользователь: {user3.Name} {user3.LastName}");
            }
            else
            {
                user3 = new User
                {
                    Id = Guid.NewGuid(),
                    Name = "Петр",
                    LastName = "Петров",
                    Updata = "Создан",
                    DeleteUsers = "Нет",
                    CreatedAt = DateTime.UtcNow
                };

                mongoStorage.SaveUser(user3);
                postgresStorage.SaveUser(user3);
                Console.WriteLine($"Создан пользователь: {user3.Name} {user3.LastName} (GUID: {user3.Id})");
            }

            User user4;
            if (existingSidorova != null)
            {
                user4 = existingSidorova;
                Console.WriteLine($"Используется существующий пользователь: {user4.Name} {user4.LastName}");
            }
            else
            {
                user4 = new User
                {
                    Id = Guid.NewGuid(),
                    Name = "Анна",
                    LastName = "Сидорова",
                    Updata = "Создана",
                    DeleteUsers = "Нет",
                    CreatedAt = DateTime.UtcNow
                };

                mongoStorage.SaveUser(user4);
                postgresStorage.SaveUser(user4);
                Console.WriteLine($"Создан пользователь: {user4.Name} {user4.LastName} (GUID: {user4.Id})");
            }
            #endregion

            #region ЧТЕНИЕ (READ)
            Console.WriteLine("\nЧТЕНИЕ новых пользователей:");

            var foundUser3InMongo = mongoStorage.GetUser(user3.Id);
            var foundUser3InPostgres = postgresStorage.GetUser(user3.Id);

            var foundUser4InMongo = mongoStorage.GetUser(user4.Id);
            var foundUser4InPostgres = postgresStorage.GetUser(user4.Id);
            #endregion

            #region ОБНОВЛЕНИЕ (UPDATE)
            Guid targetGuid = Guid.Parse("eb8defbf-0f71-410c-8cc4-49ee393cb5b2");

            Console.WriteLine($"\nПопытка обновления пользователя с GUID: {targetGuid}");

            // Проверяем существование пользователя В СПИСКЕ всех пользователей
            var allUsers = mongoStorage.FindUsers(u => true);
            var userToUpdate = allUsers.FirstOrDefault(u => u.Id == targetGuid);

            if (userToUpdate != null)
            {
                Console.WriteLine($"ОБНОВЛЕНИЕ пользователя:");
                Console.WriteLine($"  До: {userToUpdate.Name} {userToUpdate.LastName}");

                userToUpdate.Name = $"{userToUpdate.Name} (Обновлено)";
                userToUpdate.LastName = $"{userToUpdate.LastName} (Обновлено)";
                userToUpdate.Updata = "Обновлен";

                mongoStorage.SaveUser(userToUpdate);
                postgresStorage.SaveUser(userToUpdate);

                Console.WriteLine($"  После: {userToUpdate.Name} {userToUpdate.LastName}");
                Console.WriteLine($"  GUID: {userToUpdate.Id} (не изменен)");
            }
            else
            {
                Console.WriteLine($"Пользователь с GUID {targetGuid} НЕ НАЙДЕН. Обновление пропущено.");
            }
            #endregion

            #region УДАЛЕНИЕ (DELETE)
            Guid deleteTargetGuid = Guid.Parse("d4a8f2b4-c775-418b-b24a-d96567b2b8a9");

            Console.WriteLine($"\nПопытка удаления пользователя с GUID: {deleteTargetGuid}");

            // Проверяем существование пользователя В СПИСКЕ всех пользователей
            var allUsersForDelete = mongoStorage.FindUsers(u => true);
            var userToDelete = allUsersForDelete.FirstOrDefault(u => u.Id == deleteTargetGuid);

            if (userToDelete != null)
            {
                Console.WriteLine($"УДАЛЕНИЕ пользователя:");
                Console.WriteLine($"  Id: {userToDelete.Id}");
                Console.WriteLine($"  Name: {userToDelete.Name}");
                Console.WriteLine($"  LastName: {userToDelete.LastName}");

                mongoStorage.DeleteUser(deleteTargetGuid);
                postgresStorage.DeleteUser(deleteTargetGuid);

                Console.WriteLine($"  Удален из обеих БД");
            }
            else
            {
                Console.WriteLine($"Пользователь с GUID {deleteTargetGuid} НЕ НАЙДЕН. Удаление пропущено.");
            }
            #endregion

            Console.ReadKey();
        }
    }
}