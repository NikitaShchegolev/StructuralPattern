using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bridge_MongoDB.Interface;
using Bridge_MongoDB.Model;

namespace Bridge_MongoDB.Repository
{
    public abstract class UserRepository
    {
        protected readonly IUserStorage _userStorage;//Только чтение
        /// <summary>
        /// обращение классу только через конструктор с параметром IUserStorage
        /// </summary>
        /// <param name="userStorage"></param>
        protected UserRepository(IUserStorage userStorage) { _userStorage = userStorage; }
        /// <summary>
        /// /Абстрактный метод для получения id Пользователя
        /// </summary>
        /// <returns>/Абстрактный метод для получения id Пользователя</returns>
        public abstract User GetUserById();
        /// <summary>
        /// Абстрактный метод для получения создания пользователя
        /// </summary>
        /// <returns>Абстрактный метод для получения создания пользователя</returns>
        public abstract User CreateUser();
        /// <summary>
        /// Абстрактный метод для получения для обновления данных пользователя
        /// </summary>
        /// <returns>Абстрактный метод для получения для обновления данных пользователя</returns>
        public abstract User UpdateUser();
        /// <summary>
        /// Абстрактный метод для получения для удааления пользователя
        /// </summary>
        /// <returns>Абстрактный метод для получения для удааления пользователя</returns>
        public abstract User RemoveUser();
    }
}
