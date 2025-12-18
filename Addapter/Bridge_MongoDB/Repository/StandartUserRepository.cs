using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bridge_MongoDB.Interface;
using Bridge_MongoDB.Model;

namespace Bridge_MongoDB.Repository
{
    public class StandartUserRepository : UserRepository
    {
        /// <summary>
        /// Конструктор с договором по использованию класса при его использовании, должно быть реализована логика методов
        /// 1. GetUser,
        /// 2. SaveUser,
        /// 3. DeleteUserForMongo,
        /// 4. FindUsers
        /// </summary>
        /// <param name="userStorage"></param>
        public StandartUserRepository(IUserStorage userStorage) : base(userStorage) { }
        /// <summary>
        /// Cоздать пользователя
        /// </summary>
        /// <returns>Cоздать пользователя</returns>
        public override User CreateUser() { return new User() { Name = "Пользователь 2"}; }
        /// <summary>
        /// Получить Guid пользователя
        /// </summary>
        /// <returns>Получить Guid пользователя</returns>
        public override User GetUserById() { return new User() { Id = Guid.NewGuid() }; }
        /// <summary>
        /// Удалить данные пользователя
        /// </summary>
        /// <returns>Удалить данные пользователя</returns>
        public override User RemoveUser() { return new User() { DeleteUsers = "Пользователь 2 удален" }; }
        /// <summary>
        /// Обновить данные пользователя
        /// </summary>
        /// <returns>Обновить данные пользователя</returns>
        public override User UpdateUser() { return new User() { Updata = "Данные пользователя 2 обновлены" }; }
    }
}
