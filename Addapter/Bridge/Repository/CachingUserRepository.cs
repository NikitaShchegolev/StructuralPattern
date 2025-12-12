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
        public CachingUserRepository(IUserStorage storage) : base(storage) { }
        /// <summary>
        /// Создать пользователя
        /// </summary>
        /// <returns>Создать пользователя</returns>
        public override User CreateUser()
        {
            //TODO: Доделать
            User user = new User();
            return user;
        }
        /// <summary>
        /// Получить Id
        /// </summary>
        /// <returns>Получить Id</returns>
        public override User GetUserById()
        {
            //TODO: Доделать
            User user = new User();
            return user;
        }
        /// <summary>
        /// Удалить пользователя
        /// </summary>
        /// <returns>Удалить пользователя</returns>
        public override User RemoveUser()
        {
            //TODO: Доделать
            User user = new User();
            return user;
        }
        /// <summary>
        /// Обновить данные пользователя
        /// </summary>
        /// <returns>Обновить данные пользователя</returns>
        public override User UpdateUser()
        {
            //TODO: Доделать
            User user = new User();
            return user;
        }
    }
}
