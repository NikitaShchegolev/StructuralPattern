using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bridge.Interface;
using Bridge.Model;
using Bridge.Repository;
using Bridge.Storages;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Npgsql;

namespace Bridge.Service
{
    public class UserService
    {
        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public static void ConfigureServices(IServiceCollection service, IConfiguration configuration)
        {

            service.AddSingleton<IUserStorage>(serviceProvider =>
            {
                var storageType = configuration["StorageType"];
                var connectionStrting = configuration.GetConnectionString("PostgresConnection");
                return new PostgresUserStorage(connectionStrting);
            });
            //UseCaching: "true" - включает использование кэширования, что приведет к созданию CachingUserRepository
            service.AddSingleton<UserRepository>(serviceDI =>
            {
                var storageType = serviceDI.GetRequiredService<IUserStorage>();
                return bool.Parse(configuration["UseCaching"])
                    ? new CachingUserRepository(storageType)
                    : new StandartUserRepository(storageType);
            });
        }
        
        public List<User> FindUsers(Func<User, bool> predicate)
        {
            // Получаем всех пользователей через репозиторий
            // Для упрощения предполагаем, что репозиторий имеет метод GetAll()
            // В реальной реализации может потребоваться добавить такой метод
            throw new NotImplementedException("Метод должен быть реализован в репозитории");
        }
        /// <summary>
        /// Получить Guid пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Guid пользователя</returns>
        public User GetUser(Guid userId)
        {
            return _userRepository.GetUserById(userId);
        }
        /// <summary>
        /// Сохранить пользователя в базе
        /// </summary>
        /// <param name="user"></param>
        public void SaveUser(User user)
        {
            // Проверяем, существует ли пользователь
            try
            {
                var existingUser = _userRepository.GetUserById(user.Id);
                // Если пользователь существует, обновляем его
                _userRepository.UpdateUser(user);
            }
            catch (InvalidOperationException)
            {
                // Если пользователь не найден, создаем нового
                _userRepository.CreateUser(user);
            }
        }
        /// <summary>
        /// PostgresUserStorage. Удалить пользователя
        /// </summary>
        /// <param name="userId"></param>
        public void DeleteUser(Guid userId)
        {
            _userRepository.RemoveUser(userId);
        }
    }
}
