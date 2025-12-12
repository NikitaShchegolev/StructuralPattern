using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bridge.Interface;
using Bridge.Model;

namespace Bridge.Repository
{
    public class CachingUserRepository: UserRepository
    {
        /// <summary>
        /// Конструктор с договором по использованию класса при его использовании, должно быть реализована логика методов
        /// </summary>
        /// <param name="storage"></param>
        public CachingUserRepository(IUserStorage storage) : base(storage) { }
        /// <summary>
        /// Создать пользователя
        /// </summary>
        /// <returns>Создать пользователя</returns>
        public override User CreateUser()
        {
            //TODO: Доделать
            return new User() { Name = "Пользователь_1" };
        }
        /// <summary>
        /// Получить Id
        /// </summary>
        /// <returns>Получить Id</returns>
        public override User GetUserById()
        {
            //TODO: Доделать
            return new User() { Id = Guid.NewGuid() };
        }
        /// <summary>
        /// Удалить пользователя
        /// </summary>
        /// <returns>Удалить пользователя</returns>
        public override User RemoveUser()
        {
            //TODO: Доделать
            return new User() { DeleteUsers = "Данные пользователя удалены" };
        }
        /// <summary>
        /// Обновить данные пользователя
        /// </summary>
        /// <returns>Обновить данные пользователя</returns>
        public override User UpdateUser()
        {
            return new User() { Updata = "Данные пользователя обновлены" };
        }
    }
}
