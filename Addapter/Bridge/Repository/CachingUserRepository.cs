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
        private readonly Dictionary<Guid, User> _cache = new Dictionary<Guid, User>();
        /// <summary>
        /// Конструктор с договором по использованию класса при его использовании, должно быть реализована логика методов
        /// </summary>
        /// <param name="storage"></param>
        public CachingUserRepository(IUserStorage storage) : base(storage) { }
        /// <summary>
        /// Получить Id
        /// </summary>
        /// <returns>Получить Id</returns>
        public override User GetUserById(Guid userId)
        {
            switch (_cache.TryGetValue(userId, out User cachedUser))
            {
                case true:
                    return cachedUser;
                    case false:
                    break;
            }
            var user = _userStorage.GetUser(userId);
            _cache[userId] = user;
            return user;
        }
        /// <summary>
        /// Создать пользователя
        /// </summary>
        /// <returns>Создать пользователя</returns>
        public override void CreateUser(User user)
                {
                    _userStorage.SaveUser(user);
                    _cache[user.Id] = user;
                }
        /// <summary>
        /// Удалить пользователя
        /// </summary>
        /// <returns>Удалить пользователя</returns>
        public override void RemoveUser(Guid userId)
                {
                    _userStorage.DeleteUser(userId);
                    _cache.Remove(userId);
                }
        /// <summary>
        /// Обновить данные пользователя
        /// </summary>
        /// <returns>Обновить данные пользователя</returns>
        public override void UpdateUser(User user)
                {
                    _userStorage.SaveUser(user);
                    _cache[user.Id] = user;
                }
    }
}
