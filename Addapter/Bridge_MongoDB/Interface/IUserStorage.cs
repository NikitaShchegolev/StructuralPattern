using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bridge_MongoDB.Model;
using Bridge_MongoDB.Services;

namespace Bridge_MongoDB.Interface
{
    public interface IUserStorage
    {
        /// <summary>
        /// Добавить пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Добавить пользователя</returns>
        List<User> GetUser(Guid userId);
        /// <summary>
        /// Сохранить пользователя в базу
        /// </summary>
        /// <param name="user"></param>
        void SaveUser(User user);
        /// <summary>
        /// Найти пользователей по условию Read
        /// </summary>
        /// <param name="predicate"></param>
        List<User> FindUsers(Func<User, bool> predicate);
        /// <summary>
        /// Удалить пользователя из базы данных пользователей
        /// </summary>
        /// <param name="userId"></param>
        void DeleteUserForMongo(Guid userId);
    }
}
