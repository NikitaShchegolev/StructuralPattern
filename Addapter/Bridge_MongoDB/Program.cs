using System.Xml.Linq;

using Bridge_MongoDB.Interface;
using Bridge_MongoDB.Model;
using Bridge_MongoDB.Service;
using Bridge_MongoDB.Solution;
using Bridge_MongoDB.Storages;

namespace Bridge_MongoDB
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
            string mongoConnectionString = "mongodb://admin:xxx711717XXX@72.56.72.77:32017/?authSource=admin&directConnection=true&tls=false";
            string mongoDatabaseName = "Users";


            IUserStorage mongoStorage = new MongoUserStorage(mongoConnectionString, mongoDatabaseName);

            Console.WriteLine("Подключения к MongoDB и PostgreSQL установлены\n");
            #endregion

            #region Проверка всех существующих пользователей
            List<User> userMogo = UserService.ValidateAllUsersMongo(mongoStorage);
            Console.WriteLine("Записано пользователей в MongoDb -> " + userMogo.Count);

            #endregion

            #region СОЗДАНИЕ НОВЫХ ПОЛЬЗОВАТЕЛЕЙ
            var allExistingUsers = mongoStorage.FindUsers(u => true);

            var existingPetrov = allExistingUsers.FirstOrDefault(u =>
                u.Name == "Петр" && u.LastName == "Петров"
            );

            var existingSidorova = allExistingUsers.FirstOrDefault(u =>
                u.Name == "Анна" && u.LastName == "Сидорова"
            );

            User user1 = new User
            {
                Id = Guid.NewGuid(),
                Name = "Петр",
                LastName = "Петров",
                Updata = "Создан",
                DeleteUsers = "Нет",
                CreatedAt = DateTime.UtcNow
            };
            mongoStorage.SaveUser(user1);
            //postgresStorage.SaveUser(user1);
            Console.WriteLine($"Создан пользователь: {user1.Name} {user1.LastName} (GUID: {user1.Id})");

            User user2 = new User
            {
                Id = Guid.NewGuid(),
                Name = "Анна",
                LastName = "Сидорова",
                Updata = "Создана",
                DeleteUsers = "Нет",
                CreatedAt = DateTime.UtcNow
            };

            mongoStorage.SaveUser(user2);
            //postgresStorage.SaveUser(user2);
            Console.WriteLine($"Создан пользователь: {user2.Name} {user2.LastName} (GUID: {user2.Id})");

            #endregion

            #region ЧТЕНИЕ (READ)
            //Console.WriteLine("\nЧТЕНИЕ новых пользователей:");
            List<User> foundUser1InMongo = mongoStorage.GetUser(user1.Id);
            foundUser1InMongo.ForEach(x => Console.WriteLine(x.Name));

            List<User> foundUser2InMongo = mongoStorage.GetUser(user2.Id);
            //var foundUser4InPostgres = postgresStorage.GetUser(user2.Id);
            foundUser2InMongo.ForEach(x => Console.WriteLine(x.Name));
            
            
            #endregion

            #region ОБНОВЛЕНИЕ (UPDATE)
            Guid targetGuid = Guid.Parse("67681925-c221-42b8-a543-0e781aac6322");

            Console.WriteLine($"\nПопытка обновления пользователя с GUID: {targetGuid}");

            //Проверяем существование пользователя В СПИСКЕ всех пользователей
           var allUsers = mongoStorage.FindUsers(u => true);
            var userToUpdate = allUsers.FirstOrDefault(u => u.Id == targetGuid);

            if (userToUpdate != null)
            {
                Console.WriteLine($"ОБНОВЛЕНИЕ пользователя:");
                Console.WriteLine($"  До: {userToUpdate.Name} {userToUpdate.LastName}");

                userToUpdate.Name = $"{userToUpdate.Name}";
                userToUpdate.LastName = $"{userToUpdate.LastName}";
                userToUpdate.Updata = "Обновлен";

                mongoStorage.SaveUser(userToUpdate);

                Console.WriteLine($"  После: {userToUpdate.Name} {userToUpdate.LastName}");
                Console.WriteLine($"  GUID: {userToUpdate.Id} (не изменен)");
            }
            else
            {
                Console.WriteLine($"Пользователь с GUID {targetGuid} НЕ НАЙДЕН. Обновление пропущено.");
            }
            #endregion

            #region УДАЛЕНИЕ (DELETE)
            Guid deleteTargetGuid = Guid.Parse("cd24b6b5-1f43-477c-b8cb-9b73d79606d7");

            Console.WriteLine($"\nПопытка удаления пользователя с GUID: {deleteTargetGuid}");

            //Проверяем существование пользователя В СПИСКЕ всех пользователей
           var allUsersForDelete = mongoStorage.FindUsers(u => true);
            var userToDelete = allUsersForDelete.FirstOrDefault(u => u.Id == deleteTargetGuid);

            if (userToDelete != null)
            {
                Console.WriteLine($"УДАЛЕНИЕ пользователя:");
                Console.WriteLine($"  Id: {userToDelete.Id}");
                Console.WriteLine($"  Name: {userToDelete.Name}");
                Console.WriteLine($"  LastName: {userToDelete.LastName}");

                mongoStorage.DeleteUserForMongo(deleteTargetGuid);

                Console.WriteLine($"  Удален из обеих БД");
            }
            else
            {
                Console.WriteLine($"Пользователь с GUID {deleteTargetGuid} НЕ НАЙДЕН. Удаление пропущено.");
            }
            #endregion
            Console.WriteLine($"Done!");
            Console.ReadKey();
        }
    }
}
