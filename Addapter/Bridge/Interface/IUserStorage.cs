using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bridge.Model;

namespace Bridge.Interface
{
    public interface IUserStorage
    {
        /// <summary>
        /// Добавить пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Добавить пользователя</returns>
        User GetUser(string userId);
        /// <summary>
        /// Сохранить пользователя в базу
        /// </summary>
        /// <param name="user"></param>
        void SaveUser(User user);
        /// <summary>
        /// Удалить пользователя из базы данных пользователей
        /// </summary>
        /// <param name="userId"></param>
        void DeleteUser(string userId);
        /// <summary>
        /// Найти пользователя
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>Найти пользователя</returns>
        List<User> FindUsers(Func<User, bool> predicate);
    }
}
