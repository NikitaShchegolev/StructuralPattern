using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bridge.Interface;
using Bridge.Model;

namespace Bridge.Repository
{
    public abstract class UserRepository
    {
        protected readonly IUserStorage _userStorage;//Только чтение
        protected UserRepository(IUserStorage userStorage) { _userStorage = userStorage; } //обращение классу только через конструктор с параметром IUserStorage
        public abstract User GetUserById();//Абстрактный метод для получения id Пользователя
        public abstract User CreateUser();//Абстрактный метод для получения для создания пользователя
        public abstract User UpdateUser();//Абстрактный метод для получения для обновления данных пользователя
        public abstract User RemoveUser();//Абстрактный метод для получения для удааления пользователя
    }
}
