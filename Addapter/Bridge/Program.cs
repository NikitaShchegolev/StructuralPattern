using Bridge.Interface;
using Bridge.Model;
using Bridge.Repository;
using Bridge.Service;
using Bridge.Solution;
using Bridge.Storages;

using Bridge_Postgres.Storages;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bridge
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // Создаем конфигурацию
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Настраиваем DI контейнер
            var services = new ServiceCollection();
            UserService.ConfigureServices(services, configuration);
            var serviceProvider = services.BuildServiceProvider();
            var userRepository = serviceProvider.GetRequiredService<UserRepository>();

            #region Конфигурация подключений

            Console.WriteLine("Подключения к PostgreSQL установлен\n");
            #endregion
            #region СОЗДАНИЕ НОВЫХ ПОЛЬЗОВАТЕЛЕЙ

            User user1 = new User
            {
                Id = Guid.NewGuid(),
                Name = "Петр",
                LastName = "Петров"
            };
            Console.WriteLine($"Создан пользователь: {user1.Name} {user1.LastName} (GUID: {user1.Id})");

            User user2 = new User
            {
                Id = Guid.NewGuid(),
                Name = "Анна",
                LastName = "Сидорова"
            };

            // Добавление пользователей через репозиторий
            userRepository.CreateUser(user1);
            userRepository.CreateUser(user2);

            Console.WriteLine($"Создан пользователь: {user2.Name} {user2.LastName} (GUID: {user2.Id})");

            #endregion
            #region ЧТЕНИЕ (READ)
            Console.WriteLine("\nЧТЕНИЕ новых пользователей:");

            // Чтение пользователей через репозиторий
            var foundUser1 = userRepository.GetUserById(user1.Id);
            Console.WriteLine("User 1 -> " + foundUser1.Name);

            var foundUser2 = userRepository.GetUserById(user2.Id);
            Console.WriteLine("User 2 -> " + foundUser2.Name);
            #endregion
            #region ОБНОВЛЕНИЕ (UPDATE)
            Guid targetGuid = user1.Id; // Используем ID первого пользователя
            Console.WriteLine($"Обновления пользователя с GUID: {targetGuid}...");

            // Получаем пользователя для обновления через репозиторий
            var userToUpdate = userRepository.GetUserById(targetGuid);

            if (userToUpdate != null)
            {
                Console.WriteLine($"ОБНОВЛЕНИЕ пользователя:");
                Console.WriteLine($"  До: {userToUpdate.Name} {userToUpdate.LastName}");

                userToUpdate.Name = $"{userToUpdate.Name} (Обновлен)";
                userToUpdate.LastName = $"{userToUpdate.LastName} (Обновлен)";

                userRepository.UpdateUser(userToUpdate);

                Console.WriteLine($"  После: {userToUpdate.Name} {userToUpdate.LastName}");
                Console.WriteLine($"  GUID: {userToUpdate.Id}");
            }
            else
            {
                Console.WriteLine($"Пользователь с GUID {targetGuid} НЕ НАЙДЕН. Обновление пропущено.");
            }

            #endregion
            #region УДАЛЕНИЕ (DELETE)

            //List<Guid> guids = new List<Guid>();
            //guids.Add(user1.Id);
            //guids.Add(user2.Id);

            //foreach (Guid guid in guids)
            //{
            //    Console.WriteLine($"\nУдаление пользователя с GUID: {guid}");
            //    userRepository.RemoveUser(guid);
            //    Console.WriteLine($"Пользователь с GUID {guid} удален");
            //}

            #endregion
            Console.WriteLine($"Done!");
            Console.ReadKey();
        }


    }
}
